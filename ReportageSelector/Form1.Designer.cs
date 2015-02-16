namespace ReportageSelector
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
            this.ReportagesListBox = new System.Windows.Forms.ListBox();
            this.ReportagesListBoxLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportagesListBox
            // 
            this.ReportagesListBox.FormattingEnabled = true;
            this.ReportagesListBox.Location = new System.Drawing.Point(10, 27);
            this.ReportagesListBox.Name = "ReportagesListBox";
            this.ReportagesListBox.Size = new System.Drawing.Size(493, 251);
            this.ReportagesListBox.TabIndex = 0;
            // 
            // ReportagesListBoxLabel
            // 
            this.ReportagesListBoxLabel.AutoSize = true;
            this.ReportagesListBoxLabel.Location = new System.Drawing.Point(10, 10);
            this.ReportagesListBoxLabel.Name = "ReportagesListBoxLabel";
            this.ReportagesListBoxLabel.Size = new System.Drawing.Size(112, 13);
            this.ReportagesListBoxLabel.TabIndex = 0;
            this.ReportagesListBoxLabel.Text = "Выберите репортаж:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(510, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 250);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(510, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 42);
            this.button1.TabIndex = 1;
            this.button1.Text = "В этот репортаж";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 290);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Не выпускать";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 332);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReportagesListBoxLabel);
            this.Controls.Add(this.ReportagesListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ReportagesListBox;
        private System.Windows.Forms.Label ReportagesListBoxLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

