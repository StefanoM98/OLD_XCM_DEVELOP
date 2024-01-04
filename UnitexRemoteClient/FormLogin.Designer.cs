
namespace UnitexRemoteClient
{
    partial class FormLogin
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.checkEditRicordami = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButtonVerifica = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonAnnulla = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditRicordami.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Utenza";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 55);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(46, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Password";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(64, 23);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(223, 21);
            this.textBoxUser.TabIndex = 0;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(64, 52);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(223, 21);
            this.textBoxPassword.TabIndex = 1;
            // 
            // checkEditRicordami
            // 
            this.checkEditRicordami.Location = new System.Drawing.Point(64, 83);
            this.checkEditRicordami.Name = "checkEditRicordami";
            this.checkEditRicordami.Properties.Caption = "Ricordami";
            this.checkEditRicordami.Size = new System.Drawing.Size(75, 20);
            this.checkEditRicordami.TabIndex = 4;
            // 
            // simpleButtonVerifica
            // 
            this.simpleButtonVerifica.Location = new System.Drawing.Point(212, 81);
            this.simpleButtonVerifica.Name = "simpleButtonVerifica";
            this.simpleButtonVerifica.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonVerifica.TabIndex = 2;
            this.simpleButtonVerifica.Text = "Autorizza";
            this.simpleButtonVerifica.Click += new System.EventHandler(this.simpleButtonVerifica_Click);
            // 
            // simpleButtonAnnulla
            // 
            this.simpleButtonAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonAnnulla.Location = new System.Drawing.Point(212, 110);
            this.simpleButtonAnnulla.Name = "simpleButtonAnnulla";
            this.simpleButtonAnnulla.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonAnnulla.TabIndex = 3;
            this.simpleButtonAnnulla.Text = "Annulla";
            this.simpleButtonAnnulla.Click += new System.EventHandler(this.simpleButtonAnnulla_Click);
            // 
            // FormLogin
            // 
            this.AcceptButton = this.simpleButtonVerifica;
            this.ActiveGlowColor = System.Drawing.Color.Blue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.simpleButtonAnnulla;
            this.ClientSize = new System.Drawing.Size(310, 149);
            this.Controls.Add(this.simpleButtonAnnulla);
            this.Controls.Add(this.simpleButtonVerifica);
            this.Controls.Add(this.checkEditRicordami);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.InactiveGlowColor = System.Drawing.Color.Aqua;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Autenticazione XCM";
            ((System.ComponentModel.ISupportInitialize)(this.checkEditRicordami.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private DevExpress.XtraEditors.CheckEdit checkEditRicordami;
        private DevExpress.XtraEditors.SimpleButton simpleButtonVerifica;
        private DevExpress.XtraEditors.SimpleButton simpleButtonAnnulla;
    }
}