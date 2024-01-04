
namespace XCM_Remote_Client
{
    partial class FormLicenza
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCodiceLicenzaCry = new System.Windows.Forms.TextBox();
            this.simpleButtonVerifica = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxQuestoPc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCodiceCliente = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Codice Licenza";
            // 
            // textBoxCodiceLicenzaCry
            // 
            this.textBoxCodiceLicenzaCry.Location = new System.Drawing.Point(12, 67);
            this.textBoxCodiceLicenzaCry.Multiline = true;
            this.textBoxCodiceLicenzaCry.Name = "textBoxCodiceLicenzaCry";
            this.textBoxCodiceLicenzaCry.Size = new System.Drawing.Size(242, 105);
            this.textBoxCodiceLicenzaCry.TabIndex = 1;
            // 
            // simpleButtonVerifica
            // 
            this.simpleButtonVerifica.Location = new System.Drawing.Point(179, 219);
            this.simpleButtonVerifica.Name = "simpleButtonVerifica";
            this.simpleButtonVerifica.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonVerifica.TabIndex = 2;
            this.simpleButtonVerifica.Text = "Verifica";
            this.simpleButtonVerifica.Click += new System.EventHandler(this.simpleButtonVerifica_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Questo PC";
            // 
            // textBoxQuestoPc
            // 
            this.textBoxQuestoPc.Location = new System.Drawing.Point(12, 25);
            this.textBoxQuestoPc.Name = "textBoxQuestoPc";
            this.textBoxQuestoPc.Size = new System.Drawing.Size(242, 21);
            this.textBoxQuestoPc.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Codice Cliente";
            // 
            // textBoxCodiceCliente
            // 
            this.textBoxCodiceCliente.Location = new System.Drawing.Point(12, 192);
            this.textBoxCodiceCliente.Name = "textBoxCodiceCliente";
            this.textBoxCodiceCliente.Size = new System.Drawing.Size(242, 21);
            this.textBoxCodiceCliente.TabIndex = 1;
            // 
            // FormLicenza
            // 
            this.ActiveGlowColor = System.Drawing.Color.Blue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 262);
            this.Controls.Add(this.simpleButtonVerifica);
            this.Controls.Add(this.textBoxCodiceCliente);
            this.Controls.Add(this.textBoxQuestoPc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCodiceLicenzaCry);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.InactiveGlowColor = System.Drawing.Color.Aqua;
            this.Name = "FormLicenza";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormLicenza";
            this.Load += new System.EventHandler(this.FormLicenza_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCodiceLicenzaCry;
        private DevExpress.XtraEditors.SimpleButton simpleButtonVerifica;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxQuestoPc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCodiceCliente;
    }
}