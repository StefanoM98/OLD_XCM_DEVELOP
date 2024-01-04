
namespace AmministrazioneUtility
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.simpleButtonViaggi = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonContaSpedizioni = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // simpleButtonViaggi
            // 
            this.simpleButtonViaggi.Location = new System.Drawing.Point(12, 12);
            this.simpleButtonViaggi.Name = "simpleButtonViaggi";
            this.simpleButtonViaggi.Size = new System.Drawing.Size(168, 32);
            this.simpleButtonViaggi.TabIndex = 0;
            this.simpleButtonViaggi.Text = "Inserisci Lista Viaggi";
            this.simpleButtonViaggi.Click += new System.EventHandler(this.simpleButtonViaggi_Click);
            // 
            // simpleButtonContaSpedizioni
            // 
            this.simpleButtonContaSpedizioni.Location = new System.Drawing.Point(12, 50);
            this.simpleButtonContaSpedizioni.Name = "simpleButtonContaSpedizioni";
            this.simpleButtonContaSpedizioni.Size = new System.Drawing.Size(168, 32);
            this.simpleButtonContaSpedizioni.TabIndex = 0;
            this.simpleButtonContaSpedizioni.Text = "Conta spedizioni in Lista Viaggi";
            this.simpleButtonContaSpedizioni.Click += new System.EventHandler(this.simpleButtonContaSpedizioni_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 193);
            this.Controls.Add(this.simpleButtonContaSpedizioni);
            this.Controls.Add(this.simpleButtonViaggi);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonViaggi;
        private DevExpress.XtraEditors.SimpleButton simpleButtonContaSpedizioni;
    }
}

