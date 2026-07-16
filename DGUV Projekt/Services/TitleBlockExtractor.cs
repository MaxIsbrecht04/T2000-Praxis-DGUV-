using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig.Content;

namespace DGUV_Projekt.Services
{
    /// <summary>
    /// Ergebnis der Schriftfeld-Auswertung einer Seite.
    /// </summary>
    public class TitleBlockResult
    {
        /// <summary>Lfd.-Nr. / Lage aus dem Schriftfeld, z.B. "#0310".</summary>
        public string Lage { get; set; }

        /// <summary>Ortskennzeichen aus dem Schriftfeld, z.B. "+P141".</summary>
        public string Ort { get; set; }

        /// <summary>Reiner Text der ausgewerteten Schriftfeld-Region (fuer Log/KI).</summary>
        public string RawText { get; set; }

        public bool IsComplete => !string.IsNullOrEmpty(Lage) && !string.IsNullOrEmpty(Ort);

        public bool HasCandidates =>
            !string.IsNullOrEmpty(RawText) && (RawText.Contains("#") || RawText.Contains("+"));
    }

    /// <summary>
    /// Liest gezielt das EPLAN-Schriftfeld (unten rechts) einer Seite aus.
    ///
    /// Hintergrund: '+' und '#' kommen auf einer Seite mehrfach vor
    /// (Geraete-Querverweise wie +P141-XG122, Klemmen usw.). Nur im Schriftfeld
    /// stehen die gesuchten Werte: die Lfd.-Nr. (#) und das Ortskennzeichen (+).
    /// Deshalb wird ueber die Wort-Koordinaten nur der Schriftfeld-Bereich
    /// betrachtet - so faellt das Rauschen aus dem Schaltplan weg.
    /// </summary>
    public class TitleBlockExtractor
    {
        // Kandidaten-Regionen fuer das Schriftfeld (unten rechts), von eng nach
        // weit. Anteile der Seitenbreite/-hoehe. Erste Region, die ein '+' UND
        // '#' liefert, gewinnt. Bei abweichenden EPLAN-Vorlagen hier justieren.
        private static readonly double[][] Regions =
        {
            //  ab X-Anteil (rechts),  bis Y-Anteil (unten)
            new[] { 0.45, 0.18 },
            new[] { 0.35, 0.28 },
            new[] { 0.25, 0.35 }
        };

        // Ortskennzeichen: '+' optional gefolgt von Leerzeichen, dann der Wert.
        private static readonly Regex OrtRegex =
            new Regex(@"\+\s*([A-Za-z0-9][A-Za-z0-9_.\-]*)", RegexOptions.Compiled);

        // Lfd.-Nr. / Lage: '#' optional gefolgt von Leerzeichen, dann der Wert.
        private static readonly Regex LageRegex =
            new Regex(@"#\s*([0-9][0-9A-Za-z_./\-]*)", RegexOptions.Compiled);

        public TitleBlockResult Extract(Page page)
        {
            TitleBlockResult best = null;

            foreach (var region in Regions)
            {
                string text = GetRegionText(page, region[0], region[1]);
                var result = Parse(text);

                // Erste vollstaendige Region sofort verwenden.
                if (result.IsComplete)
                {
                    return result;
                }

                // Sonst die "beste" Teil-Region merken (fuer Log/Diagnose).
                if (best == null || (!best.HasCandidates && result.HasCandidates))
                {
                    best = result;
                }
            }

            return best ?? new TitleBlockResult { RawText = string.Empty };
        }

        private TitleBlockResult Parse(string text)
        {
            var result = new TitleBlockResult { RawText = text };

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            Match ortMatch = OrtRegex.Match(text);
            if (ortMatch.Success)
            {
                result.Ort = "+" + ortMatch.Groups[1].Value;
            }

            Match lageMatch = LageRegex.Match(text);
            if (lageMatch.Success)
            {
                result.Lage = "#" + lageMatch.Groups[1].Value;
            }

            return result;
        }

        /// <summary>
        /// Sammelt die Woerter im angegebenen Schriftfeld-Bereich und gibt sie
        /// in Lesereihenfolge (oben nach unten, links nach rechts) zurueck.
        /// </summary>
        private string GetRegionText(Page page, double rightFraction, double bottomFraction)
        {
            double xMin = page.Width * rightFraction;
            double yMax = page.Height * bottomFraction;

            // Koordinatenursprung in PDF ist unten links: kleines Bottom = unten.
            var words = page.GetWords()
                .Where(w => w.BoundingBox.Left >= xMin && w.BoundingBox.Bottom <= yMax)
                .OrderByDescending(w => w.BoundingBox.Bottom)
                .ThenBy(w => w.BoundingBox.Left);

            var sb = new StringBuilder();
            foreach (Word word in words)
            {
                sb.Append(word.Text);
                sb.Append(' ');
            }

            return sb.ToString().Trim();
        }
    }
}
