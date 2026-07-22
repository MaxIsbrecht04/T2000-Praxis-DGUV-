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
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.grpTemplate.SuspendLayout();
            this.grpSources.SuspendLayout();
            this.SuspendLayout();
            //
            // grpTemplate
            //
            this.grpTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTemplate.Controls.Add(this.btnTemplate);
            this.grpTemplate.Controls.Add(this.txtTemplate);
            this.grpTemplate.Location = new System.Drawing.Point(12, 12);
            this.grpTemplate.Name = "grpTemplate";
            this.grpTemplate.Size = new System.Drawing.Size(816, 66);
            this.grpTemplate.TabIndex = 0;
            this.grpTemplate.TabStop = false;
            this.grpTemplate.Text = "1) Prüfprotokoll-Vorlage (Ziel-Datei .xlsx)";
            //
            // btnTemplate
            //
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTemplate.Location = new System.Drawing.Point(677, 25);
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
            this.txtTemplate.Size = new System.Drawing.Size(649, 22);
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
            this.grpSources.Location = new System.Drawing.Point(12, 84);
            this.grpSources.Name = "grpSources";
            this.grpSources.Size = new System.Drawing.Size(816, 214);
            this.grpSources.TabIndex = 1;
            this.grpSources.TabStop = false;
            this.grpSources.Text = "2) Quelldateien";
            //
            // lblKabel
            //
            this.lblKabel.AutoSize = true;
            this.lblKabel.Location = new System.Drawing.Point(16, 26);
            this.lblKabel.Name = "lblKabel";
            this.lblKabel.Size = new System.Drawing.Size(540, 16);
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
            this.txtKabel.Size = new System.Drawing.Size(649, 22);
            this.txtKabel.TabIndex = 1;
            //
            // btnKabel
            //
            this.btnKabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKabel.Location = new System.Drawing.Point(677, 43);
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
            this.lblLoopl.Size = new System.Drawing.Size(560, 16);
            this.lblLoopl.TabIndex = 3;
            this.lblLoopl.Text = "Loopliste   →   ergänzt ZLPE um Loop-Nr. + speisendes Schutzorgan (Kenndaten)";
            //
            // txtLoopl
            //
            this.txtLoopl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoopl.Location = new System.Drawing.Point(16, 107);
            this.txtLoopl.Name = "txtLoopl";
            this.txtLoopl.ReadOnly = true;
            this.txtLoopl.Size = new System.Drawing.Size(649, 22);
            this.txtLoopl.TabIndex = 4;
            //
            // btnLoopl
            //
            this.btnLoopl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoopl.Location = new System.Drawing.Point(677, 105);
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
            this.txtGround.Size = new System.Drawing.Size(649, 22);
            this.txtGround.TabIndex = 7;
            //
            // btnGround
            //
            this.btnGround.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGround.Location = new System.Drawing.Point(677, 167);
            this.btnGround.Name = "btnGround";
            this.btnGround.Size = new System.Drawing.Size(127, 27);
            this.btnGround.TabIndex = 8;
            this.btnGround.Text = "Erdungsverb...";
            this.btnGround.UseVisualStyleBackColor = true;
            this.btnGround.Click += new System.EventHandler(this.btnGround_Click);
            //
            // btnFill
            //
            this.btnFill.Location = new System.Drawing.Point(12, 306);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(240, 42);
            this.btnFill.TabIndex = 2;
            this.btnFill.Text = "3) Ausfüllen && speichern...";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            //
            // progressBar
            //
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(268, 306);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(560, 23);
            this.progressBar.TabIndex = 3;
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(268, 332);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 16);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Bereit.";
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 359);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(83, 16);
            this.lblLog.TabIndex = 5;
            this.lblLog.Text = "Live-Protokoll:";
            //
            // txtLog
            //
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 378);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(816, 200);
            this.txtLog.TabIndex = 6;
            this.txtLog.Text = "";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 590);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnFill);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.grpSources);
            this.Controls.Add(this.grpTemplate);
            this.MinimumSize = new System.Drawing.Size(700, 560);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DGUV V3 - Prüfprotokoll-Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpTemplate.ResumeLayout(false);
            this.grpTemplate.PerformLayout();
            this.grpSources.ResumeLayout(false);
            this.grpSources.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}
