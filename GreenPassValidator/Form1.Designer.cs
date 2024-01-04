
namespace GreenPassValidator
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timerRefreshCamera = new System.Windows.Forms.Timer(this.components);
            this.TimerBlink = new System.Windows.Forms.Timer(this.components);
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::GreenPassValidator.WaitForm1), false, true);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelControlOrarioPC = new DevExpress.XtraEditors.LabelControl();
            this.timerOraRilevamento = new System.Windows.Forms.Timer(this.components);
            this.labelRisultatoScansione = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.timerFullScreen = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.timerCheckPresenzeDaily = new System.Windows.Forms.Timer(this.components);
            this.timerRedLine = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerRefreshCamera
            // 
            this.timerRefreshCamera.Interval = 1000;
            this.timerRefreshCamera.Tick += new System.EventHandler(this.timerRefreshCamer_Tick);
            // 
            // TimerBlink
            // 
            this.TimerBlink.Interval = 1000;
            this.TimerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(887, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(370, 193);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // labelControlOrarioPC
            // 
            this.labelControlOrarioPC.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControlOrarioPC.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlOrarioPC.Appearance.Options.UseFont = true;
            this.labelControlOrarioPC.Location = new System.Drawing.Point(593, 3);
            this.labelControlOrarioPC.Name = "labelControlOrarioPC";
            this.labelControlOrarioPC.Size = new System.Drawing.Size(116, 35);
            this.labelControlOrarioPC.TabIndex = 6;
            this.labelControlOrarioPC.Text = "00:00:00";
            // 
            // timerOraRilevamento
            // 
            this.timerOraRilevamento.Enabled = true;
            this.timerOraRilevamento.Interval = 250;
            this.timerOraRilevamento.Tick += new System.EventHandler(this.timerOraRilevamento_Tick);
            // 
            // labelRisultatoScansione
            // 
            this.labelRisultatoScansione.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRisultatoScansione.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRisultatoScansione.Location = new System.Drawing.Point(12, 652);
            this.labelRisultatoScansione.Name = "labelRisultatoScansione";
            this.labelRisultatoScansione.Size = new System.Drawing.Size(1263, 49);
            this.labelRisultatoScansione.TabIndex = 7;
            this.labelRisultatoScansione.Text = "RISULTATO SCANSIONE";
            this.labelRisultatoScansione.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelRisultatoScansione.Visible = false;
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest.Location = new System.Drawing.Point(1141, 9);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(124, 23);
            this.buttonTest.TabIndex = 8;
            this.buttonTest.Text = "TEST";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Visible = false;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // timerFullScreen
            // 
            this.timerFullScreen.Enabled = true;
            this.timerFullScreen.Interval = 1000;
            this.timerFullScreen.Tick += new System.EventHandler(this.timerFullScreen_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(887, 321);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(370, 277);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel6.Location = new System.Drawing.Point(15, 13);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(848, 585);
            this.panel6.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.label1.Location = new System.Drawing.Point(42, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(711, 112);
            this.label1.TabIndex = 0;
            this.label1.Text = "Per l\'accesso utilizzare il QR Code aziendale e non il GreenPass";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(218, 31);
            this.label4.TabIndex = 0;
            this.label4.Text = "Bacheca Digitale";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Red;
            this.panel5.Location = new System.Drawing.Point(887, 456);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(374, 5);
            this.panel5.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Location = new System.Drawing.Point(15, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1260, 610);
            this.panel1.TabIndex = 9;
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new System.Drawing.Point(15, 11);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new System.Drawing.Size(457, 21);
            this.comboBoxDevice.TabIndex = 10;
            this.comboBoxDevice.Visible = false;
            this.comboBoxDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxDevice_SelectedIndexChanged);
            // 
            // timerCheckPresenzeDaily
            // 
            this.timerCheckPresenzeDaily.Enabled = true;
            this.timerCheckPresenzeDaily.Interval = 10000;
            this.timerCheckPresenzeDaily.Tick += new System.EventHandler(this.timerCheckPresenzeDaily_Tick);
            // 
            // timerRedLine
            // 
            this.timerRedLine.Enabled = true;
            this.timerRedLine.Interval = 250;
            this.timerRedLine.Tick += new System.EventHandler(this.timerRedLine_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 707);
            this.Controls.Add(this.comboBoxDevice);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.labelRisultatoScansione);
            this.Controls.Add(this.labelControlOrarioPC);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controllo Accessi XCM";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerRefreshCamera;
        private System.Windows.Forms.Timer TimerBlink;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DevExpress.XtraEditors.LabelControl labelControlOrarioPC;
        private System.Windows.Forms.Timer timerOraRilevamento;
        private System.Windows.Forms.Label labelRisultatoScansione;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Timer timerFullScreen;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxDevice;
        private System.Windows.Forms.Timer timerCheckPresenzeDaily;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerRedLine;
    }
}

