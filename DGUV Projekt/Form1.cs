using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DGUV_Projekt.Services.Excel;

namespace DGUV_Projekt
{
    /// <summary>
    /// Prüfprotokoll-Generator: Füllt die beiden Untertabellen des
    /// DGUV-V3-Prüfprotokolls.
    ///   - Kabelübersicht + Loopliste -> Blatt "Messdatenblatt ZLPE IK RISO"
    ///     (Leistungskabel -WD* mit speisendem Schutzorgan)
    ///   - Erdungsverbindungen        -> Blatt "Messdatenblatt RPE"
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log("Bereit. 1) Vorlage waehlen, 2) Quelldateien waehlen, 3) Ausfuellen.");
            Log("Kabeluebersicht (+ Loopliste)  -> Blatt \"Messdatenblatt ZLPE IK RISO\"");
            Log("Erdungsverbindungen            -> Blatt \"Messdatenblatt RPE\"");
        }

        // Thread-sicheres Live-Logging (Aufrufe aus Hintergrund-Threads via Invoke).
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

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            string path = PickExcel("Pruefprotokoll-Vorlage auswaehlen",
                "Excel-Vorlage (*.xlsx;*.xlsm)|*.xlsx;*.xlsm");
            if (path != null)
            {
                txtTemplate.Text = path;
                Log($"Vorlage: {path}");
            }
        }

        private void btnKabel_Click(object sender, EventArgs e)
        {
            string path = PickExcel("Kabeluebersicht auswaehlen (Leistungskabel fuer ZLPE)",
                "Excel (*.xlsx;*.xlsm;*.xls)|*.xlsx;*.xlsm;*.xls");
            if (path != null)
            {
                txtKabel.Text = path;
                Log($"Kabeluebersicht: {path}");
            }
        }

        private void btnLoopl_Click(object sender, EventArgs e)
        {
            string path = PickExcel("Loopliste auswaehlen (Schutzorgane/Loop-Nr. fuer ZLPE)",
                "Excel (*.xlsx;*.xlsm;*.xls)|*.xlsx;*.xlsm;*.xls");
            if (path != null)
            {
                txtLoopl.Text = path;
                Log($"Loopliste: {path}");
            }
        }

        private void btnGround_Click(object sender, EventArgs e)
        {
            string path = PickExcel("Erdungsverbindungen auswaehlen (fuellt RPE)",
                "Excel (*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm");
            if (path != null)
            {
                txtGround.Text = path;
                Log($"Erdungsverbindungen: {path}");
            }
        }

        private async void btnFill_Click(object sender, EventArgs e)
        {
            string template = txtTemplate.Text;
            string kabel = txtKabel.Text;
            string loopl = txtLoopl.Text;
            string ground = txtGround.Text;

            if (string.IsNullOrWhiteSpace(template) || !File.Exists(template))
            {
                MessageBox.Show("Bitte zuerst die Pruefprotokoll-Vorlage auswaehlen.", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool hasKabel = !string.IsNullOrWhiteSpace(kabel) && File.Exists(kabel);
            bool hasLoopl = !string.IsNullOrWhiteSpace(loopl) && File.Exists(loopl);
            bool hasGround = !string.IsNullOrWhiteSpace(ground) && File.Exists(ground);

            if (!hasKabel && !hasGround)
            {
                MessageBox.Show("Bitte mindestens die Kabeluebersicht (fuer ZLPE) oder die " +
                    "Erdungsverbindungen (fuer RPE) auswaehlen.",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hasKabel && !hasLoopl)
            {
                Log("Hinweis: Ohne Loopliste fehlen Loop-Nr. und Schutzorgan-Kenndaten im ZLPE-Blatt.");
            }

            // Zielpfad abfragen (Vorlage selbst nicht ueberschreiben).
            string outputPath;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.Title = "Ausgefuelltes Pruefprotokoll speichern unter";
                sfd.FileName = Path.GetFileNameWithoutExtension(template) + "_ausgefuellt.xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;
                outputPath = sfd.FileName;
            }

            SetBusy(true);
            try
            {
                FillResult result = await Task.Run(() => RunFill(template,
                    hasKabel ? kabel : null, hasLoopl ? loopl : null,
                    hasGround ? ground : null, outputPath));

                if (result.ZlpeRows >= 0)
                    Log($"ZLPE IK RISO: {result.ZlpeRows} Eintraege (Leistungskabel + Abgaenge) geschrieben.");
                if (result.RpeRows >= 0)
                    Log($"RPE: {result.RpeRows} Eintraege aus den Erdungsverbindungen geschrieben.");
                Log($"Gespeichert unter: {outputPath}");
                Log("Fertig.");

                MessageBox.Show("Pruefprotokoll erfolgreich ausgefuellt.", "Fertig",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log($"Fehler: {ex.Message}");
                MessageBox.Show($"Beim Ausfuellen ist ein Fehler aufgetreten:\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        // Laeuft auf dem Hintergrund-Thread.
        private FillResult RunFill(string template, string kabelPath, string looplPath,
            string groundPath, string outputPath)
        {
            var result = new FillResult();
            using (var filler = new ProtokollFiller(template))
            {
                if (kabelPath != null)
                {
                    Log("Lese Kabeluebersicht...");
                    IList<KabelRow> kabelRows = KabelUebersichtReader.Read(kabelPath);
                    Log($"Kabeluebersicht: {kabelRows.Count} Leistungskabel (-WD*) gefunden.");

                    IList<LoopRow> loopRows = new List<LoopRow>();
                    if (looplPath != null)
                    {
                        Log("Lese Loopliste...");
                        loopRows = LooplistReader.Read(looplPath);
                        Log($"Loopliste: {loopRows.Count} Abgaenge gelesen.");
                    }

                    Log("Verkette Kabel mit speisenden Schutzorganen...");
                    IList<ZlpeEintrag> eintraege = new ZlpeBuilder().Build(kabelRows, loopRows);
                    Log($"{eintraege.Count} ZLPE-Eintraege erzeugt. Schreibe in ZLPE IK RISO...");
                    result.ZlpeRows = filler.FillZlpe(eintraege);
                }

                if (groundPath != null)
                {
                    Log("Lese Erdungsverbindungen...");
                    IList<GroundRow> groundRows = GroundConnectionsReader.Read(groundPath);
                    Log($"Erdungsverbindungen: {groundRows.Count} Datenzeilen gelesen. Schreibe in RPE...");
                    result.RpeRows = filler.FillRpe(groundRows);
                }

                Log("Speichere Arbeitsmappe...");
                filler.Save(outputPath);
            }
            return result;
        }

        private string PickExcel(string title, string filter)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = title;
                ofd.Filter = filter;
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            }
        }

        private void SetBusy(bool busy)
        {
            btnFill.Enabled = !busy;
            btnTemplate.Enabled = !busy;
            btnKabel.Enabled = !busy;
            btnLoopl.Enabled = !busy;
            btnGround.Enabled = !busy;
            progressBar.Style = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            lblStatus.Text = busy ? "Verarbeitung laeuft..." : "Bereit.";
        }
    }
}
