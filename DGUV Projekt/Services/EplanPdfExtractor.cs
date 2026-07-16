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
    /// Liest ein EPLAN-PDF seitenweise ein und ermittelt pro Seite die
    /// Zuordnung Lfd.-Nr. (#) -> Ortskennzeichen (+) aus dem Schriftfeld.
    ///
    /// Zwei Betriebsarten:
    ///  - Deterministisch (Standard): Auswertung ueber Koordinaten + Muster,
    ///    ohne API-Aufruf -> schnell und reproduzierbar.
    ///  - KI-Modus (optional): der reine Schriftfeld-Text wird an die
    ///    Siemens-KI geschickt. Dabei bleibt die Rate-Limit-Bremse (2s) erhalten.
    /// </summary>
    public class EplanPdfExtractor
    {
        // Rate-Limit-Schutz fuer den KI-Modus: Der Siemens-Key erlaubt nur
        // 30 Anfragen pro Minute. Diese Pause pro KI-Aufruf ist der Tempomat.
        private const int RateLimitDelayMs = 2000;

        private readonly TitleBlockExtractor _titleBlock;
        private readonly SiemensAiClient _aiClient; // nur im KI-Modus benoetigt
        private readonly Action<string> _log;

        public EplanPdfExtractor(TitleBlockExtractor titleBlock, SiemensAiClient aiClient, Action<string> log)
        {
            _titleBlock = titleBlock ?? throw new ArgumentNullException(nameof(titleBlock));
            _aiClient = aiClient;
            _log = log ?? (_ => { });
        }

        /// <summary>
        /// Fuehrt die vollstaendige Extraktion durch und liefert das
        /// aggregierte Mapping (Lfd.-Nr. -> Ortskennzeichen).
        /// Sollte auf einem Hintergrund-Thread laufen, damit die UI nicht einfriert.
        /// </summary>
        public async Task<Dictionary<string, string>> ExtractAsync(
            string pdfPath,
            bool useAi,
            IProgress<ExtractionProgress> progress,
            CancellationToken cancellationToken)
        {
            var finalMapping = new Dictionary<string, string>();

            _log(useAi
                ? "Starte Extraktion im KI-Modus (Schriftfeld -> Siemens-KI)..."
                : "Starte deterministische Extraktion (Schriftfeld ueber Koordinaten)...");

            using (PdfDocument document = PdfDocument.Open(pdfPath))
            {
                int totalPages = document.NumberOfPages;

                for (int i = 1; i <= totalPages; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    Page page = document.GetPage(i);
                    progress?.Report(new ExtractionProgress(i, totalPages));

                    TitleBlockResult titleBlock = _titleBlock.Extract(page);

                    if (useAi)
                    {
                        await ProcessPageWithAiAsync(finalMapping, titleBlock, i, totalPages, cancellationToken)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        ProcessPageDeterministic(finalMapping, titleBlock, i);
                    }
                }
            }

            _log($"Fertig. {finalMapping.Count} Zuordnung(en) gefunden.");
            return finalMapping;
        }

        // --- Deterministische Auswertung -----------------------------------

        private void ProcessPageDeterministic(Dictionary<string, string> mapping, TitleBlockResult titleBlock, int pageNumber)
        {
            if (titleBlock.IsComplete)
            {
                mapping[titleBlock.Lage] = titleBlock.Ort;
                _log($"Seite {pageNumber}: {titleBlock.Lage} -> {titleBlock.Ort}");
            }
            else if (titleBlock.HasCandidates)
            {
                // Schriftfeld erkannt, aber nicht beide Werte gefunden -> zur
                // Diagnose kurz protokollieren (hilft beim Justieren der Region).
                _log($"Seite {pageNumber}: unvollstaendig. Schriftfeld-Text: \"{Shorten(titleBlock.RawText)}\"");
            }
        }

        // --- KI-Auswertung --------------------------------------------------

        private async Task ProcessPageWithAiAsync(
            Dictionary<string, string> mapping,
            TitleBlockResult titleBlock,
            int pageNumber,
            int totalPages,
            CancellationToken cancellationToken)
        {
            if (_aiClient == null)
            {
                throw new InvalidOperationException("KI-Modus aktiv, aber kein API-Client vorhanden.");
            }

            // Nur Seiten mit Schriftfeld-Kandidaten (+ und #) an die KI schicken.
            if (!(titleBlock.RawText.Contains("+") && titleBlock.RawText.Contains("#")))
            {
                return;
            }

            _log($"Seite {pageNumber} von {totalPages}: Schriftfeld an KI...");

            string aiResponse = await _aiClient.AskAsync(titleBlock.RawText, cancellationToken).ConfigureAwait(false);
            MergeMapping(mapping, aiResponse, pageNumber);

            _log("Warte 2 Sekunden (Rate-Limit-Schutz fuer Siemens-API)...");
            await Task.Delay(RateLimitDelayMs, cancellationToken).ConfigureAwait(false);
        }

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

        private static string Shorten(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= 120)
            {
                return value;
            }

            return value.Substring(0, 120) + "...";
        }
    }
}
