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
            this.btnGetEplan = new System.Windows.Forms.Button();
            this.btnStartExtract = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnGetEplan
            // 
            this.btnGetEplan.Location = new System.Drawing.Point(27, 23);
            this.btnGetEplan.Name = "btnGetEplan";
            this.btnGetEplan.Size = new System.Drawing.Size(134, 50);
            this.btnGetEplan.TabIndex = 0;
            this.btnGetEplan.Text = "Eplan Pdf";
            this.btnGetEplan.UseVisualStyleBackColor = true;
            this.btnGetEplan.Click += new System.EventHandler(this.btnGetEplan_Click);
            // 
            // btnStartExtract
            // 
            this.btnStartExtract.Location = new System.Drawing.Point(27, 79);
            this.btnStartExtract.Name = "btnStartExtract";
            this.btnStartExtract.Size = new System.Drawing.Size(134, 50);
            this.btnStartExtract.TabIndex = 1;
            this.btnStartExtract.Text = "Start Extract";
            this.btnStartExtract.UseVisualStyleBackColor = true;
            this.btnStartExtract.Click += new System.EventHandler(this.btnStartExtract_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(167, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(605, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(30, 145);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(741, 290);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnStartExtract);
            this.Controls.Add(this.btnGetEplan);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetEplan;
        private System.Windows.Forms.Button btnStartExtract;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

