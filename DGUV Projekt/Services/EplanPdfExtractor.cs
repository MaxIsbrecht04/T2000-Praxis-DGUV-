using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace DGUV_Projekt.Services
{
    /// <summary>
    /// Fortschrittsinformation fuer eine laufende Extraktion.
    /// Wird ueber IProgress an die UI zurueckgemeldet.
    /// </summary>
    public class ExtractionProgress
    {
        public ExtractionProgress(int currentPage, int totalPages)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
    }

    /// <summary>
    /// Liest ein EPLAN-PDF seitenweise mit UglyToad.PdfPig ein, filtert die
    /// relevanten Seiten (enthalten '+' und '#') und laesst deren Text von der
    /// KI in ein Lage-zu-Ort-Mapping ueberfuehren.
    /// Die Rate-Limit-Bremse (Task.Delay 2s) bleibt zwingend erhalten.
    /// </summary>
    public class EplanPdfExtractor
    {
        // Rate-Limit-Schutz: Der Siemens-Key erlaubt nur 30 Anfragen pro Minute.
        // Diese Pause pro KI-Aufruf ist der Tempomat und darf nicht entfernt werden.
        private const int RateLimitDelayMs = 2000;

        private readonly SiemensAiClient _aiClient;
        private readonly Action<string> _log;

        public EplanPdfExtractor(SiemensAiClient aiClient, Action<string> log)
        {
            _aiClient = aiClient ?? throw new ArgumentNullException(nameof(aiClient));
            _log = log ?? (_ => { });
        }

        /// <summary>
        /// Fuehrt die vollstaendige Extraktion durch und liefert das
        /// aggregierte Mapping (Lage -> Ortskennzeichen).
        /// Sollte auf einem Hintergrund-Thread ausgefuehrt werden, damit die
        /// UI waehrend des PDF-Parsens nicht einfriert.
        /// </summary>
        public async Task<Dictionary<string, string>> ExtractAsync(
            string pdfPath,
            IProgress<ExtractionProgress> progress,
            CancellationToken cancellationToken)
        {
            var finalMapping = new Dictionary<string, string>();

            _log("Starte PDF-Extraktion...");

            using (PdfDocument document = PdfDocument.Open(pdfPath))
            {
                int totalPages = document.NumberOfPages;

                for (int i = 1; i <= totalPages; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    Page page = document.GetPage(i);
                    string pageText = page.Text;

                    progress?.Report(new ExtractionProgress(i, totalPages));

                    // Nur Seiten mit Ortskennzeichen ('+') und Lage ('#') sind relevant.
                    if (pageText.Contains("+") && pageText.Contains("#"))
                    {
                        _log($"Verarbeite Seite {i} von {totalPages} via KI...");

                        string aiResponse = await _aiClient.AskAsync(pageText, cancellationToken).ConfigureAwait(false);

                        MergeMapping(finalMapping, aiResponse, i);

                        _log("Warte 2 Sekunden (Rate-Limit-Schutz fuer Siemens-API)...");
                        await Task.Delay(RateLimitDelayMs, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            return finalMapping;
        }

        /// <summary>
        /// Deserialisiert die KI-Antwort und uebernimmt die gefundenen
        /// Zuordnungen in das Gesamt-Mapping.
        /// </summary>
        private void MergeMapping(Dictionary<string, string> target, string aiResponse, int pageNumber)
        {
            if (string.IsNullOrEmpty(aiResponse))
            {
                return;
            }

            try
            {
                var pageMapping = JsonSerializer.Deserialize<Dictionary<string, string>>(aiResponse);
                if (pageMapping != null)
                {
                    foreach (var kvp in pageMapping)
                    {
                        target[kvp.Key] = kvp.Value;
                    }
                }
            }
            catch (JsonException)
            {
                _log($"Warnung: KI hat auf Seite {pageNumber} kein sauberes JSON zurueckgegeben.");
            }
        }
    }
}
