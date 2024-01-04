
namespace MassiveMailSender
{
    partial class ConfiguratoreMail
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfiguratoreMail));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEditSMTPHost = new DevExpress.XtraEditors.TextEdit();
            this.textEditSMTPport = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditSenderMail = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textEditSenderMailPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textEditSenderMailName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkButtonMostraPassword = new DevExpress.XtraEditors.CheckButton();
            this.styleControllerFlatAppearance = new DevExpress.XtraEditors.StyleController(this.components);
            this.simpleButtonTestSettings = new DevExpress.XtraEditors.SimpleButton();
            this.dxErrorProviderConfiguratoreEmail = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSMTPHost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSMTPport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMail.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMailPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMailName.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerFlatAppearance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProviderConfiguratoreEmail)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(28, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(25, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Host";
            // 
            // textEditSMTPHost
            // 
            this.textEditSMTPHost.Location = new System.Drawing.Point(81, 31);
            this.textEditSMTPHost.Name = "textEditSMTPHost";
            this.textEditSMTPHost.Size = new System.Drawing.Size(223, 20);
            this.textEditSMTPHost.TabIndex = 0;
            this.textEditSMTPHost.Enter += new System.EventHandler(this.textEdit_Enter);
            this.textEditSMTPHost.Leave += new System.EventHandler(this.textEdit_Leave);
            this.textEditSMTPHost.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit_Validating);
            // 
            // textEditSMTPport
            // 
            this.textEditSMTPport.Location = new System.Drawing.Point(81, 57);
            this.textEditSMTPport.Name = "textEditSMTPport";
            this.textEditSMTPport.Size = new System.Drawing.Size(223, 20);
            this.textEditSMTPport.TabIndex = 1;
            this.textEditSMTPport.Enter += new System.EventHandler(this.textEdit_Enter);
            this.textEditSMTPport.Leave += new System.EventHandler(this.textEdit_Leave);
            this.textEditSMTPport.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit_Validating);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(30, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(23, 16);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Port";
            // 
            // textEditSenderMail
            // 
            this.textEditSenderMail.Location = new System.Drawing.Point(81, 38);
            this.textEditSenderMail.Name = "textEditSenderMail";
            this.textEditSenderMail.Size = new System.Drawing.Size(223, 20);
            this.textEditSenderMail.TabIndex = 0;
            this.textEditSenderMail.Enter += new System.EventHandler(this.textEdit_Enter);
            this.textEditSenderMail.Leave += new System.EventHandler(this.textEdit_Leave);
            this.textEditSenderMail.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit_Validating);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(28, 41);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(25, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Email";
            // 
            // textEditSenderMailPassword
            // 
            this.textEditSenderMailPassword.Location = new System.Drawing.Point(81, 64);
            this.textEditSenderMailPassword.Name = "textEditSenderMailPassword";
            this.textEditSenderMailPassword.Properties.PasswordChar = '*';
            this.textEditSenderMailPassword.Size = new System.Drawing.Size(191, 20);
            this.textEditSenderMailPassword.TabIndex = 1;
            this.textEditSenderMailPassword.Enter += new System.EventHandler(this.textEdit_Enter);
            this.textEditSenderMailPassword.Leave += new System.EventHandler(this.textEdit_Leave);
            this.textEditSenderMailPassword.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit_Validating);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(7, 67);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(46, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Password";
            // 
            // textEditSenderMailName
            // 
            this.textEditSenderMailName.Location = new System.Drawing.Point(81, 90);
            this.textEditSenderMailName.Name = "textEditSenderMailName";
            this.textEditSenderMailName.Size = new System.Drawing.Size(223, 20);
            this.textEditSenderMailName.TabIndex = 2;
            this.textEditSenderMailName.Enter += new System.EventHandler(this.textEdit_Enter);
            this.textEditSenderMailName.Leave += new System.EventHandler(this.textEdit_Leave);
            this.textEditSenderMailName.Validating += new System.ComponentModel.CancelEventHandler(this.textEdit_Validating);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(25, 93);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(28, 13);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textEditSMTPport);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.textEditSMTPHost);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SMTP Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkButtonMostraPassword);
            this.groupBox2.Controls.Add(this.textEditSenderMail);
            this.groupBox2.Controls.Add(this.labelControl3);
            this.groupBox2.Controls.Add(this.textEditSenderMailName);
            this.groupBox2.Controls.Add(this.labelControl4);
            this.groupBox2.Controls.Add(this.labelControl5);
            this.groupBox2.Controls.Add(this.textEditSenderMailPassword);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 140);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sender Settings";
            // 
            // checkButtonMostraPassword
            // 
            this.checkButtonMostraPassword.AppearancePressed.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.checkButtonMostraPassword.AppearancePressed.Options.UseBackColor = true;
            this.checkButtonMostraPassword.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.checkButtonMostraPassword.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.checkButtonMostraPassword.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("checkButtonMostraPassword.ImageOptions.SvgImage")));
            this.checkButtonMostraPassword.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.checkButtonMostraPassword.Location = new System.Drawing.Point(278, 64);
            this.checkButtonMostraPassword.Name = "checkButtonMostraPassword";
            this.checkButtonMostraPassword.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.checkButtonMostraPassword.Size = new System.Drawing.Size(26, 20);
            this.checkButtonMostraPassword.StyleController = this.styleControllerFlatAppearance;
            this.checkButtonMostraPassword.TabIndex = 3;
            this.checkButtonMostraPassword.CheckedChanged += new System.EventHandler(this.checkButtonMostraPassword_CheckedChanged);
            // 
            // styleControllerFlatAppearance
            // 
            this.styleControllerFlatAppearance.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.styleControllerFlatAppearance.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.styleControllerFlatAppearance.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            // 
            // simpleButtonTestSettings
            // 
            this.simpleButtonTestSettings.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonTestSettings.Appearance.Options.UseFont = true;
            this.simpleButtonTestSettings.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButtonTestSettings.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonTestSettings.ImageOptions.Image")));
            this.simpleButtonTestSettings.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.simpleButtonTestSettings.Location = new System.Drawing.Point(290, 285);
            this.simpleButtonTestSettings.Name = "simpleButtonTestSettings";
            this.simpleButtonTestSettings.Size = new System.Drawing.Size(41, 33);
            this.simpleButtonTestSettings.StyleController = this.styleControllerFlatAppearance;
            toolTipItem1.Text = "Test impostazioni";
            superToolTip1.Items.Add(toolTipItem1);
            this.simpleButtonTestSettings.SuperTip = superToolTip1;
            this.simpleButtonTestSettings.TabIndex = 2;
            this.simpleButtonTestSettings.Click += new System.EventHandler(this.simpleButtonTestSettings_Click);
            // 
            // dxErrorProviderConfiguratoreEmail
            // 
            this.dxErrorProviderConfiguratoreEmail.ContainerControl = this;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.simpleButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.simpleButton1.Location = new System.Drawing.Point(12, 285);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(41, 33);
            toolTipItem2.Text = "Test impostazioni";
            superToolTip2.Items.Add(toolTipItem2);
            this.simpleButton1.SuperTip = superToolTip2;
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // ConfiguratoreMail
            // 
            this.AcceptButton = this.simpleButtonTestSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 330);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButtonTestSettings);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ConfiguratoreMail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Impostazioni Mail";
            this.Load += new System.EventHandler(this.ConfiguratoreMail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditSMTPHost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSMTPport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMail.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMailPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSenderMailName.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.styleControllerFlatAppearance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProviderConfiguratoreEmail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditSMTPHost;
        private DevExpress.XtraEditors.TextEdit textEditSMTPport;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditSenderMail;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textEditSenderMailPassword;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEditSenderMailName;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.CheckButton checkButtonMostraPassword;
        private DevExpress.XtraEditors.StyleController styleControllerFlatAppearance;
        private DevExpress.XtraEditors.SimpleButton simpleButtonTestSettings;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProviderConfiguratoreEmail;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}