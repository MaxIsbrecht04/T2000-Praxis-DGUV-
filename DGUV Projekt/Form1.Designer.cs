namespace DGUV_Projekt
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpInputs = new System.Windows.Forms.GroupBox();
            this.chkUseAi = new System.Windows.Forms.CheckBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblPdf = new System.Windows.Forms.Label();
            this.btnSelectPdf = new System.Windows.Forms.Button();
            this.txtPdfPath = new System.Windows.Forms.TextBox();
            this.btnStartExtraction = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.grpInputs.SuspendLayout();
            this.SuspendLayout();
            //
            // grpInputs
            //
            this.grpInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInputs.Controls.Add(this.chkUseAi);
            this.grpInputs.Controls.Add(this.lblApiKey);
            this.grpInputs.Controls.Add(this.txtApiKey);
            this.grpInputs.Controls.Add(this.lblPdf);
            this.grpInputs.Controls.Add(this.btnSelectPdf);
            this.grpInputs.Controls.Add(this.txtPdfPath);
            this.grpInputs.Location = new System.Drawing.Point(12, 12);
            this.grpInputs.Name = "grpInputs";
            this.grpInputs.Size = new System.Drawing.Size(776, 142);
            this.grpInputs.TabIndex = 0;
            this.grpInputs.TabStop = false;
            this.grpInputs.Text = "Eingaben";
            //
            // chkUseAi
            //
            this.chkUseAi.AutoSize = true;
            this.chkUseAi.Location = new System.Drawing.Point(150, 106);
            this.chkUseAi.Name = "chkUseAi";
            this.chkUseAi.Size = new System.Drawing.Size(442, 20);
            this.chkUseAi.TabIndex = 5;
            this.chkUseAi.Text = "KI-Extraktion statt deterministischer Auswertung verwenden (Vergleich)";
            this.chkUseAi.UseVisualStyleBackColor = true;
            this.chkUseAi.CheckedChanged += new System.EventHandler(this.chkUseAi_CheckedChanged);
            //
            // lblApiKey
            //
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Location = new System.Drawing.Point(16, 72);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(115, 16);
            this.lblApiKey.TabIndex = 3;
            this.lblApiKey.Text = "Siemens API-Key:";
            //
            // txtApiKey
            //
            this.txtApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApiKey.Location = new System.Drawing.Point(150, 69);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.PasswordChar = '*';
            this.txtApiKey.Size = new System.Drawing.Size(608, 22);
            this.txtApiKey.TabIndex = 4;
            //
            // lblPdf
            //
            this.lblPdf.AutoSize = true;
            this.lblPdf.Location = new System.Drawing.Point(16, 33);
            this.lblPdf.Name = "lblPdf";
            this.lblPdf.Size = new System.Drawing.Size(76, 16);
            this.lblPdf.TabIndex = 0;
            this.lblPdf.Text = "EPLAN-PDF:";
            //
            // btnSelectPdf
            //
            this.btnSelectPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectPdf.Location = new System.Drawing.Point(637, 28);
            this.btnSelectPdf.Name = "btnSelectPdf";
            this.btnSelectPdf.Size = new System.Drawing.Size(121, 27);
            this.btnSelectPdf.TabIndex = 2;
            this.btnSelectPdf.Text = "Durchsuchen...";
            this.btnSelectPdf.UseVisualStyleBackColor = true;
            this.btnSelectPdf.Click += new System.EventHandler(this.btnSelectPdf_Click);
            //
            // txtPdfPath
            //
            this.txtPdfPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPdfPath.Location = new System.Drawing.Point(150, 30);
            this.txtPdfPath.Name = "txtPdfPath";
            this.txtPdfPath.ReadOnly = true;
            this.txtPdfPath.Size = new System.Drawing.Size(481, 22);
            this.txtPdfPath.TabIndex = 1;
            //
            // btnStartExtraction
            //
            this.btnStartExtraction.Location = new System.Drawing.Point(12, 166);
            this.btnStartExtraction.Name = "btnStartExtraction";
            this.btnStartExtraction.Size = new System.Drawing.Size(160, 40);
            this.btnStartExtraction.TabIndex = 1;
            this.btnStartExtraction.Text = "Extraktion starten";
            this.btnStartExtraction.UseVisualStyleBackColor = true;
            this.btnStartExtraction.Click += new System.EventHandler(this.btnStartExtraction_Click);
            //
            // btnCancel
            //
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(178, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // progressBar
            //
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(304, 166);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(484, 23);
            this.progressBar.TabIndex = 3;
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(304, 192);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 16);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Bereit.";
            //
            // lblLog
            //
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 219);
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
            this.txtLog.Location = new System.Drawing.Point(12, 238);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(776, 262);
            this.txtLog.TabIndex = 6;
            this.txtLog.Text = "";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 512);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStartExtraction);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.grpInputs);
            this.MinimumSize = new System.Drawing.Size(620, 470);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DGUV V3 - Modul 0: EPLAN PDF-Extraktion";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpInputs.ResumeLayout(false);
            this.grpInputs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpInputs;
        private System.Windows.Forms.CheckBox chkUseAi;
        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label lblPdf;
        private System.Windows.Forms.Button btnSelectPdf;
        private System.Windows.Forms.TextBox txtPdfPath;
        private System.Windows.Forms.Button btnStartExtraction;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}
