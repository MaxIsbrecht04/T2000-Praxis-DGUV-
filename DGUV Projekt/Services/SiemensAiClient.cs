using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DGUV_Projekt.Services
{
    /// <summary>
    /// Kapselt die Kommunikation mit der Siemens-LLM-API.
    /// Zustaendig fuer den Request-Aufbau, das Senden und das Extrahieren
    /// des reinen JSON-Inhalts aus der KI-Antwort.
    /// </summary>
    public class SiemensAiClient
    {
        // Endpunkt und Modell laut Vorgabe fuer Modul 0.
        private const string Endpoint = "https://api.siemens.com/llm/v1/chat/completions";
        private const string Model = "qwen-3.6-27b";

        // Der Extraktions-Auftrag an die KI. Bewusst als Konstante gehalten,
        // damit der Prompt zentral und nachvollziehbar bleibt.
        private const string SystemPrompt =
            "Du bekommst den Text aus dem EPLAN-Schriftfeld (unten rechts auf dem Blatt). " +
            "Finde die Lfd.-Nr., die mit '#' beginnt (z.B. '#0310'), und das Ortskennzeichen, " +
            "das mit '+' beginnt (z.B. '+P141'). Weise die Lfd.-Nr. dem Ortskennzeichen zu. " +
            "Ignoriere das Anlagenkennzeichen, das mit '=' beginnt. Antworte AUSSCHLIESSLICH mit " +
            "einem validen, rohen JSON-Objekt im Format {\"#0310\": \"+P141\"}. Verwende keine " +
            "Markdown-Bloecke (wie ```json) und keinen zusaetzlichen Text.";

        // Ein einziger, wiederverwendeter HttpClient (Best Practice, verhindert
        // Socket-Erschoepfung). Der Bearer-Token wird pro Request gesetzt, damit
        // keine geteilten Default-Header thread-unsicher mutiert werden.
        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        private readonly string _apiKey;
        private readonly Action<string> _log;

        public SiemensAiClient(string apiKey, Action<string> log)
        {
            _apiKey = apiKey;
            _log = log ?? (_ => { });
        }

        /// <summary>
        /// Schickt den Seitentext an die KI und liefert den bereinigten
        /// JSON-String der Antwort zurueck (oder null bei einem Fehler).
        /// </summary>
        public async Task<string> AskAsync(string extractedText, CancellationToken cancellationToken)
        {
            string requestJson = BuildRequestJson(extractedText);

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, Endpoint))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    using (var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                    {
                        string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            // Fehlertext mitloggen (z.B. 429 Rate-Limit), aber nicht die
                            // gesamte Antwort ins Log fluten.
                            _log($"API-Fehler {(int)response.StatusCode} ({response.ReasonPhrase}): {Truncate(responseString, 300)}");
                            return null;
                        }

                        return ExtractContent(responseString);
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Echter Nutzer-Abbruch: nach oben durchreichen, damit die Schleife stoppt.
                throw;
            }
            catch (Exception ex)
            {
                // Alles andere (inkl. Timeout / Netzwerkfehler) protokollieren und
                // die Seite ueberspringen, statt den gesamten Lauf abzubrechen.
                _log($"API-Fehler: {ex.Message}");
                return null;
            }
        }

        private static string BuildRequestJson(string extractedText)
        {
            var requestBody = new
            {
                model = Model,
                max_tokens = 500,
                chat_template_kwargs = new { enable_thinking = false },
                messages = new[]
                {
                    new { role = "system", content = SystemPrompt },
                    new { role = "user", content = extractedText }
                }
            };

            return JsonSerializer.Serialize(requestBody);
        }

        /// <summary>
        /// Holt den Nachrichteninhalt aus der Chat-Completion-Antwort und
        /// entfernt eventuelle Markdown-Codefences.
        /// </summary>
        private string ExtractContent(string responseString)
        {
            using (JsonDocument doc = JsonDocument.Parse(responseString))
            {
                JsonElement root = doc.RootElement;
                string content = root
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (content == null)
                {
                    return null;
                }

                return content.Replace("```json", string.Empty).Replace("```", string.Empty).Trim();
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength) + "...";
        }
    }
}
