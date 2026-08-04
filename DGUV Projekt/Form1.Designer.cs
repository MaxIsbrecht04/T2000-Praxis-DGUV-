namespace DGUV_Projekt
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblPortalTitle = new System.Windows.Forms.Label();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnNavModul1 = new System.Windows.Forms.Button();
            this.btnNavModul2 = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlModul1 = new System.Windows.Forms.Panel();
            this.lblM1Title = new System.Windows.Forms.Label();
            this.lblM1Sub = new System.Windows.Forms.Label();
            this.grpTemplate = new System.Windows.Forms.GroupBox();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.grpSources = new System.Windows.Forms.GroupBox();
            this.lblKabel = new System.Windows.Forms.Label();
            this.txtKabel = new System.Windows.Forms.TextBox();
            this.btnKabel = new System.Windows.Forms.Button();
            this.lblLoopl = new System.Windows.Forms.Label();
            this.txtLoopl = new System.Windows.Forms.TextBox();
            this.btnLoopl = new System.Windows.Forms.Button();
            this.lblGround = new System.Windows.Forms.Label();
            this.txtGround = new System.Windows.Forms.TextBox();
            this.btnGround = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlModul2 = new System.Windows.Forms.Panel();
            this.lblM2Title = new System.Windows.Forms.Label();
            this.lblM2Text = new System.Windows.Forms.Label();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlNav.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlModul1.SuspendLayout();
            this.grpTemplate.SuspendLayout();
            this.grpSources.SuspendLayout();
            this.pnlModul2.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.pnlHeader.Controls.Add(this.lblVersion);
            this.pnlHeader.Controls.Add(this.lblPortalTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 64);
            this.pnlHeader.TabIndex = 0;
            //
            // lblVersion
            //
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(940, 24);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(41, 16);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "v1.0.0";
            //
            // lblPortalTitle
            //
            this.lblPortalTitle.AutoSize = true;
            this.lblPortalTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblPortalTitle.ForeColor = System.Drawing.Color.White;
            this.lblPortalTitle.Location = new System.Drawing.Point(20, 14);
            this.lblPortalTitle.Name = "lblPortalTitle";
            this.lblPortalTitle.Size = new System.Drawing.Size(151, 32);
            this.lblPortalTitle.TabIndex = 0;
            this.lblPortalTitle.Text = "TOOL Portal";
            //
            // pnlNav
            //
            this.pnlNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlNav.Controls.Add(this.btnNavModul1);
            this.pnlNav.Controls.Add(this.btnNavModul2);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNav.Location = new System.Drawing.Point(0, 64);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Size = new System.Drawing.Size(1000, 50);
            this.pnlNav.TabIndex = 1;
            //
            // btnNavModul1
            //
            this.btnNavModul1.FlatAppearance.BorderSize = 0;
            this.btnNavModul1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavModul1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavModul1.Location = new System.Drawing.Point(12, 8);
            this.btnNavModul1.Name = "btnNavModul1";
            this.btnNavModul1.Size = new System.Drawing.Size(230, 34);
            this.btnNavModul1.TabIndex = 0;
            this.btnNavModul1.Text = "Modul 1 · Datenübertragung";
            this.btnNavModul1.UseVisualStyleBackColor = false;
            this.btnNavModul1.Click += new System.EventHandler(this.btnNavModul1_Click);
            //
            // btnNavModul2
            //
            this.btnNavModul2.FlatAppearance.BorderSize = 0;
            this.btnNavModul2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavModul2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavModul2.Location = new System.Drawing.Point(248, 8);
            this.btnNavModul2.Name = "btnNavModul2";
            this.btnNavModul2.Size = new System.Drawing.Size(200, 34);
            this.btnNavModul2.TabIndex = 1;
            this.btnNavModul2.Text = "Modul 2 · Messwerte";
            this.btnNavModul2.UseVisualStyleBackColor = false;
            this.btnNavModul2.Click += new System.EventHandler(this.btnNavModul2_Click);
            //
            // pnlContent
            //
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.pnlModul1);
            this.pnlContent.Controls.Add(this.pnlModul2);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 114);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1000, 456);
            this.pnlContent.TabIndex = 2;
            //
            // pnlModul1
            //
            this.pnlModul1.AutoScroll = true;
            this.pnlModul1.BackColor = System.Drawing.Color.White;
            this.pnlModul1.Controls.Add(this.lblM1Title);
            this.pnlModul1.Controls.Add(this.lblM1Sub);
            this.pnlModul1.Controls.Add(this.grpTemplate);
            this.pnlModul1.Controls.Add(this.grpSources);
            this.pnlModul1.Controls.Add(this.btnFill);
            this.pnlModul1.Controls.Add(this.progressBar);
            this.pnlModul1.Controls.Add(this.lblStatus);
            this.pnlModul1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModul1.Location = new System.Drawing.Point(0, 0);
            this.pnlModul1.Name = "pnlModul1";
            this.pnlModul1.Padding = new System.Windows.Forms.Padding(16);
            this.pnlModul1.Size = new System.Drawing.Size(1000, 456);
            this.pnlModul1.TabIndex = 0;
            //
            // lblM1Title
            //
            this.lblM1Title.AutoSize = true;
            this.lblM1Title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblM1Title.Location = new System.Drawing.Point(16, 12);
            this.lblM1Title.Name = "lblM1Title";
            this.lblM1Title.Size = new System.Drawing.Size(268, 25);
            this.lblM1Title.TabIndex = 0;
            this.lblM1Title.Text = "Modul 1 – Datenübertragung";
            //
            // lblM1Sub
            //
            this.lblM1Sub.AutoSize = true;
            this.lblM1Sub.ForeColor = System.Drawing.Color.Gray;
            this.lblM1Sub.Location = new System.Drawing.Point(16, 42);
            this.lblM1Sub.Name = "lblM1Sub";
            this.lblM1Sub.Size = new System.Drawing.Size(430, 16);
            this.lblM1Sub.TabIndex = 1;
            this.lblM1Sub.Text = "Füllt die Blätter „ZLPE IK RISO\" und „RPE\" aus den EPLAN-Exporten.";
            //
            // grpTemplate
            //
            this.grpTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTemplate.Controls.Add(this.btnTemplate);
            this.grpTemplate.Controls.Add(this.txtTemplate);
            this.grpTemplate.Location = new System.Drawing.Point(16, 72);
            this.grpTemplate.Name = "grpTemplate";
            this.grpTemplate.Size = new System.Drawing.Size(952, 66);
            this.grpTemplate.TabIndex = 2;
            this.grpTemplate.TabStop = false;
            this.grpTemplate.Text = "1) Prüfprotokoll-Vorlage (Ziel-Datei .xlsx)";
            //
            // btnTemplate
            //
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTemplate.Location = new System.Drawing.Point(813, 25);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(127, 27);
            this.btnTemplate.TabIndex = 1;
            this.btnTemplate.Text = "Durchsuchen...";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            //
            // txtTemplate
            //
            this.txtTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplate.Location = new System.Drawing.Point(16, 27);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.ReadOnly = true;
            this.txtTemplate.Size = new System.Drawing.Size(785, 22);
            this.txtTemplate.TabIndex = 0;
            //
            // grpSources
            //
            this.grpSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSources.Controls.Add(this.lblKabel);
            this.grpSources.Controls.Add(this.txtKabel);
            this.grpSources.Controls.Add(this.btnKabel);
            this.grpSources.Controls.Add(this.lblLoopl);
            this.grpSources.Controls.Add(this.txtLoopl);
            this.grpSources.Controls.Add(this.btnLoopl);
            this.grpSources.Controls.Add(this.lblGround);
            this.grpSources.Controls.Add(this.txtGround);
            this.grpSources.Controls.Add(this.btnGround);
            this.grpSources.Location = new System.Drawing.Point(16, 148);
            this.grpSources.Name = "grpSources";
            this.grpSources.Size = new System.Drawing.Size(952, 214);
            this.grpSources.TabIndex = 3;
            this.grpSources.TabStop = false;
            this.grpSources.Text = "2) Quelldateien";
            //
            // lblKabel
            //
            this.lblKabel.AutoSize = true;
            this.lblKabel.Location = new System.Drawing.Point(16, 26);
            this.lblKabel.Name = "lblKabel";
            this.lblKabel.Size = new System.Drawing.Size(520, 16);
            this.lblKabel.TabIndex = 0;
            this.lblKabel.Text = "Kabelübersicht   →   füllt „Messdatenblatt ZLPE IK RISO\" (Leistungskabel -WD*)";
            //
            // txtKabel
            //
            this.txtKabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKabel.Location = new System.Drawing.Point(16, 45);
            this.txtKabel.Name = "txtKabel";
            this.txtKabel.ReadOnly = true;
            this.txtKabel.Size = new System.Drawing.Size(785, 22);
            this.txtKabel.TabIndex = 1;
            //
            // btnKabel
            //
            this.btnKabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKabel.Location = new System.Drawing.Point(813, 43);
            this.btnKabel.Name = "btnKabel";
            this.btnKabel.Size = new System.Drawing.Size(127, 27);
            this.btnKabel.TabIndex = 2;
            this.btnKabel.Text = "Kabelübersicht...";
            this.btnKabel.UseVisualStyleBackColor = true;
            this.btnKabel.Click += new System.EventHandler(this.btnKabel_Click);
            //
            // lblLoopl
            //
            this.lblLoopl.AutoSize = true;
            this.lblLoopl.Location = new System.Drawing.Point(16, 88);
            this.lblLoopl.Name = "lblLoopl";
            this.lblLoopl.Size = new System.Drawing.Size(540, 16);
            this.lblLoopl.TabIndex = 3;
            this.lblLoopl.Text = "Loopliste   →   ergänzt ZLPE um Loop-Nr. + speisendes Schutzorgan";
            //
            // txtLoopl
            //
            this.txtLoopl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoopl.Location = new System.Drawing.Point(16, 107);
            this.txtLoopl.Name = "txtLoopl";
            this.txtLoopl.ReadOnly = true;
            this.txtLoopl.Size = new System.Drawing.Size(785, 22);
            this.txtLoopl.TabIndex = 4;
            //
            // btnLoopl
            //
            this.btnLoopl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoopl.Location = new System.Drawing.Point(813, 105);
            this.btnLoopl.Name = "btnLoopl";
            this.btnLoopl.Size = new System.Drawing.Size(127, 27);
            this.btnLoopl.TabIndex = 5;
            this.btnLoopl.Text = "Loopliste...";
            this.btnLoopl.UseVisualStyleBackColor = true;
            this.btnLoopl.Click += new System.EventHandler(this.btnLoopl_Click);
            //
            // lblGround
            //
            this.lblGround.AutoSize = true;
            this.lblGround.Location = new System.Drawing.Point(16, 150);
            this.lblGround.Name = "lblGround";
            this.lblGround.Size = new System.Drawing.Size(404, 16);
            this.lblGround.TabIndex = 6;
            this.lblGround.Text = "Erdungsverbindungen   →   füllt Blatt „Messdatenblatt RPE\"";
            //
            // txtGround
            //
            this.txtGround.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGround.Location = new System.Drawing.Point(16, 169);
            this.txtGround.Name = "txtGround";
            this.txtGround.ReadOnly = true;
            this.txtGround.Size = new System.Drawing.Size(785, 22);
            this.txtGround.TabIndex = 7;
            //
            // btnGround
            //
            this.btnGround.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGround.Location = new System.Drawing.Point(813, 167);
            this.btnGround.Name = "btnGround";
            this.btnGround.Size = new System.Drawing.Size(127, 27);
            this.btnGround.TabIndex = 8;
            this.btnGround.Text = "Erdungsverb...";
            this.btnGround.UseVisualStyleBackColor = true;
            this.btnGround.Click += new System.EventHandler(this.btnGround_Click);
            //
            // btnFill
            //
            this.btnFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.btnFill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFill.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFill.ForeColor = System.Drawing.Color.White;
            this.btnFill.Location = new System.Drawing.Point(16, 376);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(240, 42);
            this.btnFill.TabIndex = 4;
            this.btnFill.Text = "3) Ausfüllen && speichern...";
            this.btnFill.UseVisualStyleBackColor = false;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            //
            // progressBar
            //
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(272, 378);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(696, 23);
            this.progressBar.TabIndex = 5;
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(272, 404);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 16);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Bereit.";
            //
            // pnlModul2
            //
            this.pnlModul2.BackColor = System.Drawing.Color.White;
            this.pnlModul2.Controls.Add(this.lblM2Title);
            this.pnlModul2.Controls.Add(this.lblM2Text);
            this.pnlModul2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModul2.Location = new System.Drawing.Point(0, 0);
            this.pnlModul2.Name = "pnlModul2";
            this.pnlModul2.Padding = new System.Windows.Forms.Padding(16);
            this.pnlModul2.Size = new System.Drawing.Size(1000, 456);
            this.pnlModul2.TabIndex = 1;
            this.pnlModul2.Visible = false;
            //
            // lblM2Title
            //
            this.lblM2Title.AutoSize = true;
            this.lblM2Title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblM2Title.Location = new System.Drawing.Point(16, 12);
            this.lblM2Title.Name = "lblM2Title";
            this.lblM2Title.Size = new System.Drawing.Size(430, 25);
            this.lblM2Title.TabIndex = 0;
            this.lblM2Title.Text = "Modul 2 – Messwert-Erfassung (nicht implementiert)";
            //
            // lblM2Text
            //
            this.lblM2Text.Location = new System.Drawing.Point(16, 52);
            this.lblM2Text.Name = "lblM2Text";
            this.lblM2Text.Size = new System.Drawing.Size(940, 240);
            this.lblM2Text.TabIndex = 1;
            this.lblM2Text.Text = "Modul 2 sollte die Messwerte des Prüfgeräts automatisch in die richtigen Spalten d" +
    "es Prüfprotokolls schreiben. Dies wird im Rahmen dieser Arbeit nur konzeptionell " +
    "behandelt und nicht praktisch umgesetzt.\r\n\r\nBegründung:\r\n• Es gibt kein standardi" +
    "siertes DGUV-V3-Messgerät – je nach Hersteller/Modell unterschiedliche, meist prop" +
    "rietäre Datenprotokolle.\r\n• Eine allgemeingültige Lösung müsste sehr viele Geräte-" +
    "Schnittstellen abdecken.\r\n• Manche Geräte besitzen gar keine Computer-Schnittstell" +
    "e.\r\n\r\nDetails und der konzeptionelle Lösungsweg (Push-Print / Zustandsautomat) sin" +
    "d in der Dokumentation beschrieben.";
            //
            // pnlLog
            //
            this.pnlLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.pnlLog.Controls.Add(this.txtLog);
            this.pnlLog.Controls.Add(this.lblLog);
            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLog.Location = new System.Drawing.Point(0, 570);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Padding = new System.Windows.Forms.Padding(8);
            this.pnlLog.Size = new System.Drawing.Size(1000, 170);
            this.pnlLog.TabIndex = 3;
            //
            // txtLog
            //
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(8, 30);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(984, 132);
            this.txtLog.TabIndex = 1;
            this.txtLog.Text = "";
            //
            // lblLog
            //
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLog.Location = new System.Drawing.Point(8, 8);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(984, 22);
            this.lblLog.TabIndex = 0;
            this.lblLog.Text = "Live-Protokoll:";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 740);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlLog);
            this.Controls.Add(this.pnlNav);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(820, 620);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIFI Portal – DGUV V3 Prüfprotokoll-Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlNav.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlModul1.ResumeLayout(false);
            this.pnlModul1.PerformLayout();
            this.grpTemplate.ResumeLayout(false);
            this.grpTemplate.PerformLayout();
            this.grpSources.ResumeLayout(false);
            this.grpSources.PerformLayout();
            this.pnlModul2.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblPortalTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Button btnNavModul1;
        private System.Windows.Forms.Button btnNavModul2;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlModul1;
        private System.Windows.Forms.Label lblM1Title;
        private System.Windows.Forms.Label lblM1Sub;
        private System.Windows.Forms.GroupBox grpTemplate;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.GroupBox grpSources;
        private System.Windows.Forms.Label lblKabel;
        private System.Windows.Forms.TextBox txtKabel;
        private System.Windows.Forms.Button btnKabel;
        private System.Windows.Forms.Label lblLoopl;
        private System.Windows.Forms.TextBox txtLoopl;
        private System.Windows.Forms.Button btnLoopl;
        private System.Windows.Forms.Label lblGround;
        private System.Windows.Forms.TextBox txtGround;
        private System.Windows.Forms.Button btnGround;
        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlModul2;
        private System.Windows.Forms.Label lblM2Title;
        private System.Windows.Forms.Label lblM2Text;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Label lblLog;
    }
}
