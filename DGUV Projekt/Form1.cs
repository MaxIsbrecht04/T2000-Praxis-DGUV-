using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGUV_Projekt.Services;

namespace DGUV_Projekt
{
    public partial class Form1 : Form
    {
        // Steuert den Abbruch eines laufenden Extraktionslaufs.
        private CancellationTokenSource _cts;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log("Modul 0 (PDF-Extraktion) bereit. Bitte PDF auswaehlen und API-Key eingeben.");
        }

        // ------------------------------------------------------------------
        // Thread-sicheres Live-Logging in die RichTextBox.
        // Aufrufe aus Hintergrund-Threads werden per Invoke auf den UI-Thread
        // marshalled, sodass die Oberflaeche fluessig aktualisiert wird.
        // ------------------------------------------------------------------
        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }

            txtLog.AppendText($"{DateTime.Now:HH:mm:ss}: {message}\r\n");
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }

        private void btnSelectPdf_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF Dateien (*.pdf)|*.pdf";
                openFileDialog.Title = "EPLAN PDF auswaehlen";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPdfPath.Text = openFileDialog.FileName;
                    Log($"PDF ausgewaehlt: {openFileDialog.FileName}");
                }
            }
        }

        private async void btnStartExtraction_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPdfPath.Text) || !File.Exists(txtPdfPath.Text))
            {
                MessageBox.Show("Bitte waehle zuerst eine gueltige PDF-Datei aus.", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtApiKey.Text))
            {
                MessageBox.Show("Bitte gib deinen Siemens API-Key ein.", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetRunningState(true);
            _cts = new CancellationTokenSource();

            // Progress wird auf dem UI-Thread erzeugt und marshallt Report()
            // dadurch automatisch zurueck auf die Oberflaeche.
            var progress = new Progress<ExtractionProgress>(OnExtractionProgress);

            try
            {
                var aiClient = new SiemensAiClient(txtApiKey.Text, Log);
                var extractor = new EplanPdfExtractor(aiClient, Log);

                string pdfPath = txtPdfPath.Text;
                CancellationToken token = _cts.Token;

                // Die gesamte (teils synchrone) PDF-Verarbeitung laeuft auf einem
                // Hintergrund-Thread -> die UI bleibt jederzeit reaktionsfaehig.
                Dictionary<string, string> finalMapping =
                    await Task.Run(() => extractor.ExtractAsync(pdfPath, progress, token));

                SaveMappingToJson(finalMapping);
            }
            catch (OperationCanceledException)
            {
                Log("Vorgang wurde abgebrochen.");
            }
            catch (Exception ex)
            {
                Log($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                SetRunningState(false);
                Log("Vorgang abgeschlossen.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                Log("Abbruch angefordert...");
                btnCancel.Enabled = false;
                _cts.Cancel();
            }
        }

        // Aktualisiert Fortschrittsbalken und Statuszeile (laeuft via Progress
        // bereits auf dem UI-Thread).
        private void OnExtractionProgress(ExtractionProgress p)
        {
            if (p.TotalPages > 0)
            {
                progressBar.Maximum = p.TotalPages;
                progressBar.Value = Math.Min(p.CurrentPage, p.TotalPages);
            }

            lblStatus.Text = $"Seite {p.CurrentPage} von {p.TotalPages}";
        }

        // Schaltet die Bedienelemente zwischen "bereit" und "laeuft" um.
        private void SetRunningState(bool running)
        {
            btnStartExtraction.Enabled = !running;
            btnSelectPdf.Enabled = !running;
            txtApiKey.Enabled = !running;
            btnCancel.Enabled = running;

            if (running)
            {
                progressBar.Value = 0;
                lblStatus.Text = "Verarbeitung laeuft...";
            }
            else
            {
                lblStatus.Text = "Bereit.";
            }
        }

        private void SaveMappingToJson(Dictionary<string, string> mapping)
        {
            if (mapping.Count == 0)
            {
                Log("Keine Mappings gefunden. Es wird keine Datei gespeichert.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON Dateien (*.json)|*.json";
                saveFileDialog.Title = "Mapping-Ergebnis speichern";
                saveFileDialog.FileName = "Lage_zu_Ort_Mapping.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(mapping, options);
                    File.WriteAllText(saveFileDialog.FileName, jsonString);
                    Log($"Erfolgreich gespeichert unter: {saveFileDialog.FileName}");
                }
            }
        }

        // Laufenden Vorgang beim Schliessen des Fensters sauber abbrechen.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts?.Cancel();
            base.OnFormClosing(e);
        }
    }
}
