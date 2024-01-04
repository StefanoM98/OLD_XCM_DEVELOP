
namespace GreenPassValidator
{
    partial class GeneratoreQRCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneratoreQRCode));
            this.textBoxQRText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonGeneraQR = new System.Windows.Forms.Button();
            this.buttonSalvaQR = new System.Windows.Forms.Button();
            this.pictureBoxLogoQR = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMultiline = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogoQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxQRText
            // 
            this.textBoxQRText.Location = new System.Drawing.Point(12, 41);
            this.textBoxQRText.Name = "textBoxQRText";
            this.textBoxQRText.Size = new System.Drawing.Size(422, 20);
            this.textBoxQRText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Contenuto QR";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(12, 67);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(601, 498);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // buttonGeneraQR
            // 
            this.buttonGeneraQR.Location = new System.Drawing.Point(440, 39);
            this.buttonGeneraQR.Name = "buttonGeneraQR";
            this.buttonGeneraQR.Size = new System.Drawing.Size(75, 23);
            this.buttonGeneraQR.TabIndex = 3;
            this.buttonGeneraQR.Text = "Genera QR";
            this.buttonGeneraQR.UseVisualStyleBackColor = true;
            this.buttonGeneraQR.Click += new System.EventHandler(this.buttonGeneraQR_Click);
            // 
            // buttonSalvaQR
            // 
            this.buttonSalvaQR.Location = new System.Drawing.Point(521, 39);
            this.buttonSalvaQR.Name = "buttonSalvaQR";
            this.buttonSalvaQR.Size = new System.Drawing.Size(75, 23);
            this.buttonSalvaQR.TabIndex = 3;
            this.buttonSalvaQR.Text = "Salva QR";
            this.buttonSalvaQR.UseVisualStyleBackColor = true;
            this.buttonSalvaQR.Click += new System.EventHandler(this.buttonSalvaQR_Click);
            // 
            // pictureBoxLogoQR
            // 
            this.pictureBoxLogoQR.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxLogoQR.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogoQR.Image")));
            this.pictureBoxLogoQR.Location = new System.Drawing.Point(619, 67);
            this.pictureBoxLogoQR.Name = "pictureBoxLogoQR";
            this.pictureBoxLogoQR.Size = new System.Drawing.Size(169, 85);
            this.pictureBoxLogoQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogoQR.TabIndex = 4;
            this.pictureBoxLogoQR.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(616, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Logo QR";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(619, 180);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(177, 20);
            this.comboBoxEdit1.TabIndex = 5;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(616, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Dipendenti";
            // 
            // textBoxMultiline
            // 
            this.textBoxMultiline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMultiline.Location = new System.Drawing.Point(619, 215);
            this.textBoxMultiline.Multiline = true;
            this.textBoxMultiline.Name = "textBoxMultiline";
            this.textBoxMultiline.Size = new System.Drawing.Size(177, 172);
            this.textBoxMultiline.TabIndex = 6;
            // 
            // GeneratoreQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 577);
            this.Controls.Add(this.textBoxMultiline);
            this.Controls.Add(this.comboBoxEdit1);
            this.Controls.Add(this.pictureBoxLogoQR);
            this.Controls.Add(this.buttonSalvaQR);
            this.Controls.Add(this.buttonGeneraQR);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxQRText);
            this.Name = "GeneratoreQRCode";
            this.Text = "GeneratoreQRCode";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogoQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxQRText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonGeneraQR;
        private System.Windows.Forms.Button buttonSalvaQR;
        private System.Windows.Forms.PictureBox pictureBoxLogoQR;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMultiline;
    }
}