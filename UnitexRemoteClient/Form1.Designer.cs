
namespace UnitexRemoteClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureEditLogo = new DevExpress.XtraEditors.PictureEdit();
            this.simpleButtonSpedizioni = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonPrenotaRitiro = new DevExpress.XtraEditors.SimpleButton();
            this.labelControlStatoConnessione = new DevExpress.XtraEditors.LabelControl();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::UnitexRemoteClient.WaitForm1), true, true);
            this.simpleButtonLogin = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditLogo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEditLogo
            // 
            this.pictureEditLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEditLogo.EditValue = ((object)(resources.GetObject("pictureEditLogo.EditValue")));
            this.pictureEditLogo.Location = new System.Drawing.Point(214, 12);
            this.pictureEditLogo.Name = "pictureEditLogo";
            this.pictureEditLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEditLogo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEditLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEditLogo.Size = new System.Drawing.Size(188, 196);
            this.pictureEditLogo.TabIndex = 0;
            // 
            // simpleButtonSpedizioni
            // 
            this.simpleButtonSpedizioni.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButtonSpedizioni.ImageOptions.SvgImage")));
            this.simpleButtonSpedizioni.Location = new System.Drawing.Point(12, 12);
            this.simpleButtonSpedizioni.Name = "simpleButtonSpedizioni";
            this.simpleButtonSpedizioni.Size = new System.Drawing.Size(174, 46);
            this.simpleButtonSpedizioni.TabIndex = 1;
            this.simpleButtonSpedizioni.Text = "Spedizioni";
            this.simpleButtonSpedizioni.Click += new System.EventHandler(this.simpleButtonSpedizioni_Click);
            // 
            // simpleButtonPrenotaRitiro
            // 
            this.simpleButtonPrenotaRitiro.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButtonPrenotaRitiro.ImageOptions.SvgImage")));
            this.simpleButtonPrenotaRitiro.Location = new System.Drawing.Point(12, 64);
            this.simpleButtonPrenotaRitiro.Name = "simpleButtonPrenotaRitiro";
            this.simpleButtonPrenotaRitiro.Size = new System.Drawing.Size(174, 46);
            this.simpleButtonPrenotaRitiro.TabIndex = 1;
            this.simpleButtonPrenotaRitiro.Text = "Prenota Ritiro";
            this.simpleButtonPrenotaRitiro.Click += new System.EventHandler(this.simpleButtonPrenotaRitiro_Click);
            // 
            // labelControlStatoConnessione
            // 
            this.labelControlStatoConnessione.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControlStatoConnessione.Appearance.BackColor = System.Drawing.Color.Tomato;
            this.labelControlStatoConnessione.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControlStatoConnessione.Appearance.Options.UseBackColor = true;
            this.labelControlStatoConnessione.Appearance.Options.UseForeColor = true;
            this.labelControlStatoConnessione.Appearance.Options.UseTextOptions = true;
            this.labelControlStatoConnessione.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControlStatoConnessione.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlStatoConnessione.Location = new System.Drawing.Point(89, 186);
            this.labelControlStatoConnessione.Name = "labelControlStatoConnessione";
            this.labelControlStatoConnessione.Size = new System.Drawing.Size(110, 28);
            this.labelControlStatoConnessione.TabIndex = 2;
            this.labelControlStatoConnessione.Text = "Non Connesso";
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // simpleButtonLogin
            // 
            this.simpleButtonLogin.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.simpleButtonLogin.ImageOptions.SvgImageSize = new System.Drawing.Size(23, 23);
            this.simpleButtonLogin.Location = new System.Drawing.Point(12, 186);
            this.simpleButtonLogin.Name = "simpleButtonLogin";
            this.simpleButtonLogin.Size = new System.Drawing.Size(71, 28);
            this.simpleButtonLogin.TabIndex = 1;
            this.simpleButtonLogin.Text = "Login";
            this.simpleButtonLogin.Click += new System.EventHandler(this.simpleButtonLogin_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(414, 220);
            this.Controls.Add(this.labelControlStatoConnessione);
            this.Controls.Add(this.simpleButtonLogin);
            this.Controls.Add(this.simpleButtonPrenotaRitiro);
            this.Controls.Add(this.simpleButtonSpedizioni);
            this.Controls.Add(this.pictureEditLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unitex Remote Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditLogo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEditLogo;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSpedizioni;
        private DevExpress.XtraEditors.SimpleButton simpleButtonPrenotaRitiro;
        private DevExpress.XtraEditors.LabelControl labelControlStatoConnessione;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonLogin;
    }
}

