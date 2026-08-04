using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Baut aus Kabeluebersicht (+ Loopliste) die Eintraege fuer das Blatt
    /// "Messdatenblatt ZLPE IK RISO".
    ///
    /// Modell (aus dem Abgleich EPLAN-Stromlaufplan / Referenzprotokoll):
    /// Ein ZLPE-Eintrag ist ein Leistungskabel-Segment (-WD*) eines
    /// Stromkreises - bzw. ein Abgang ohne eigenes Kabel. In der zweiten
    /// Zeile steht das SPEISENDE Schutzorgan (im Schrank), dessen Kenndaten
    /// (Bauform/Nennstrom/Charakteristik) und Loop-Nr. aus der Loopliste
    /// kommen. Da Feldkabel oft in Ketten haengen (-WD2.1 am -WD2, nicht am
    /// Schrank), wird die Quelle/Ziel-Kette bis zum Schrank zurueckverfolgt.
    /// </summary>
    public class ZlpeBuilder
    {
        // Geraete-Knoten in Quelle/Ziel: "=000EEA011+H011-XD0" -> FG + Ort (+BMK).
        private static readonly Regex NodeRegex = new Regex(
            @"(=[0-9A-Za-z._]+)\+([A-Za-z0-9_]+)(-[A-Za-z0-9_.]+)?", RegexOptions.Compiled);

        // Schrank-Orte (Banner-Gruppen). Default: Ortskennzeichen beginnt mit 'H'
        // (+H011, +H122, ...). Bei anderen Anlagen-Konventionen hier anpassen.
        private static readonly Regex SchrankOrtRegex = new Regex(@"^H", RegexOptions.Compiled);

        // Querschnitt nur aus eindeutigen Typmustern: "3G2,5", "4x4mm²", "4x50/25".
        private static readonly Regex QuerschnittRegex = new Regex(
            @"\d+\s*[Gx]\s*(\d+(?:[.,]\d+)?)(/\d+)?", RegexOptions.Compiled);

        // Sicherungs-BMK "-FC<Zahl>": es wird nur "-FC1" uebernommen.
        private static readonly Regex FcRegex = new Regex(
            @"^-FC(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private class Node
        {
            public string Fg;
            public string Ort;
        }

        public IList<ZlpeEintrag> Build(IList<KabelRow> kabel, IList<LoopRow> loops)
        {
            loops = loops ?? new List<LoopRow>();

            // Loopliste: erster Eintrag je Funktionsgruppe gewinnt.
            var loopByFg = new Dictionary<string, LoopRow>(StringComparer.OrdinalIgnoreCase);
            foreach (LoopRow l in loops)
            {
                if (!loopByFg.ContainsKey(l.Funktion)) loopByFg[l.Funktion] = l;
            }

            // Index: Knoten-Bezeichnung -> Kabel, die dort haengen (fuer Ketten).
            var nodeIndex = new Dictionary<string, List<KabelRow>>(StringComparer.OrdinalIgnoreCase);
            foreach (KabelRow k in kabel)
            {
                foreach (string n in NodeKeys(k))
                {
                    List<KabelRow> list;
                    if (!nodeIndex.TryGetValue(n, out list)) nodeIndex[n] = list = new List<KabelRow>();
                    list.Add(k);
                }
            }

            var result = new List<ZlpeEintrag>();

            // 1) Jedes Leistungskabel wird ein Eintrag.
            foreach (KabelRow k in kabel)
            {
                Node feed = ResolveFeed(k, nodeIndex, loopByFg, new HashSet<KabelRow>());

                LoopRow loop = null;
                if (feed != null && feed.Fg != null) loopByFg.TryGetValue(feed.Fg, out loop);
                if (loop == null) loopByFg.TryGetValue(k.Funktion, out loop);

                result.Add(new ZlpeEintrag
                {
                    Funktion = k.Funktion,
                    Kabel = k.Kabel,
                    Ort = feed != null ? "+" + feed.Ort : null,
                    Loop = loop != null ? loop.Loop : null,
                    SchutzBmk = loop != null ? loop.Bmk : null,
                    Bauform = loop != null ? StripAmpere(loop.Bauform) : null,
                    Nennstrom = loop != null ? StripAmpere(loop.Nennstrom) : null,
                    Charakteristik = loop != null ? loop.Charakteristik : null,
                    Querschnitt = ParseQuerschnitt(k.Typ),
                    Kommentar = loop != null ? loop.Kommentar : null
                });
            }

            // 2) Loopliste-Abgaenge, deren Stromkreis kein eigenes -WD-Kabel hat,
            //    werden Eintraege ohne Kabel (z.B. Reserve-Abgaenge).
            var fgMitKabel = new HashSet<string>(kabel.Select(k => k.Funktion), StringComparer.OrdinalIgnoreCase);
            foreach (LoopRow l in loops)
            {
                if (fgMitKabel.Contains(l.Funktion)) continue;
                result.Add(new ZlpeEintrag
                {
                    Funktion = l.Funktion,
                    Kabel = null,
                    Ort = l.Ort,
                    Loop = l.Loop,
                    SchutzBmk = l.Bmk,
                    Bauform = StripAmpere(l.Bauform),
                    Nennstrom = StripAmpere(l.Nennstrom),
                    Charakteristik = l.Charakteristik,
                    Querschnitt = null,
                    Kommentar = l.Kommentar
                });
            }

            // Filtern und nach Ort, Funktion, Kabel sortieren.
            return result
                .Where(KeepEntry)
                .OrderBy(e => e.Ort, StringComparer.OrdinalIgnoreCase)
                .ThenBy(e => e.Funktion, StringComparer.OrdinalIgnoreCase)
                .ThenBy(e => e.Kabel ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// Filterregeln fuer das ZLPE-Blatt:
        ///  - keine Eintraege ohne speisendes (-) Schutzorgan oder ohne (+) Ort,
        ///  - Sicherungen (-FC): nur "-FC1", keine weiteren FC-Nummern.
        /// </summary>
        private static bool KeepEntry(ZlpeEintrag e)
        {
            if (string.IsNullOrEmpty(e.SchutzBmk)) return false;                 // kein (-) BMK
            if (string.IsNullOrEmpty(e.Ort) || !e.Ort.StartsWith("+")) return false; // kein (+) Ort

            Match fc = FcRegex.Match(e.SchutzBmk);
            if (fc.Success && fc.Groups[1].Value != "1") return false;           // nur -FC1

            return true;
        }

        /// <summary>
        /// Verfolgt die Quelle/Ziel-Kette eines Kabels bis zu einem
        /// Schrank-Knoten (Ort +H*, oder Funktionsgruppe in der Loopliste).
        /// </summary>
        private Node ResolveFeed(KabelRow k, Dictionary<string, List<KabelRow>> nodeIndex,
            Dictionary<string, LoopRow> loopByFg, HashSet<KabelRow> visited)
        {
            if (!visited.Add(k) || visited.Count > 8) return null;

            // Direkt: haengt ein Ende an einem Schrank / Looplist-Abgang?
            foreach (Match m in AllNodes(k))
            {
                string fg = m.Groups[1].Value;
                string ort = m.Groups[2].Value;
                if (SchrankOrtRegex.IsMatch(ort) || loopByFg.ContainsKey(fg))
                {
                    return new Node { Fg = fg, Ort = ort };
                }
            }

            // Sonst: ueber gemeinsame Knoten zum Vorgaenger-Kabel.
            foreach (string n in NodeKeys(k))
            {
                List<KabelRow> peers;
                if (!nodeIndex.TryGetValue(n, out peers)) continue;
                foreach (KabelRow peer in peers)
                {
                    if (peer == k) continue;
                    Node r = ResolveFeed(peer, nodeIndex, loopByFg, visited);
                    if (r != null) return r;
                }
            }
            return null;
        }

        private static IEnumerable<Match> AllNodes(KabelRow k)
        {
            foreach (Match m in NodeRegex.Matches(k.Quelle ?? string.Empty)) yield return m;
            foreach (Match m in NodeRegex.Matches(k.Ziel ?? string.Empty)) yield return m;
        }

        private static IEnumerable<string> NodeKeys(KabelRow k)
        {
            foreach (Match m in AllNodes(k)) yield return m.Value;
        }

        private static string ParseQuerschnitt(string typ)
        {
            if (string.IsNullOrEmpty(typ)) return null;
            Match m = QuerschnittRegex.Match(typ);
            if (!m.Success) return null;
            string q = m.Groups[1].Value.Replace(',', '.');
            return m.Groups[2].Success ? q + m.Groups[2].Value : q;
        }

        // "15-36A" -> "15-36", "15A" -> "15"; die Spalte traegt die Einheit [A] bereits.
        private static string StripAmpere(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            string v = value.Trim();
            if (v.EndsWith("A", StringComparison.OrdinalIgnoreCase))
            {
                v = v.Substring(0, v.Length - 1).Trim();
            }
            return v;
        }
    }
}
