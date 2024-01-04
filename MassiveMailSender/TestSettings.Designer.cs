
namespace MassiveMailSender
{
    partial class TestSettings
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEditMailTestSettings = new DevExpress.XtraEditors.TextEdit();
            this.simpleButtonInviaMailTest = new DevExpress.XtraEditors.SimpleButton();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.simpleButtonChiudi = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEditMailTestSettings.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(23, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(31, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Email";
            // 
            // textEditMailTestSettings
            // 
            this.textEditMailTestSettings.Location = new System.Drawing.Point(60, 14);
            this.textEditMailTestSettings.Name = "textEditMailTestSettings";
            this.textEditMailTestSettings.Size = new System.Drawing.Size(229, 20);
            this.textEditMailTestSettings.TabIndex = 1;
            this.textEditMailTestSettings.Validated += new System.EventHandler(this.textEditMailTestSettings_Validated);
            // 
            // simpleButtonInviaMailTest
            // 
            this.simpleButtonInviaMailTest.Location = new System.Drawing.Point(214, 40);
            this.simpleButtonInviaMailTest.Name = "simpleButtonInviaMailTest";
            this.simpleButtonInviaMailTest.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonInviaMailTest.TabIndex = 2;
            this.simpleButtonInviaMailTest.Text = "Invia";
            this.simpleButtonInviaMailTest.Click += new System.EventHandler(this.simpleButtonInviaMailTest_Click);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // simpleButtonChiudi
            // 
            this.simpleButtonChiudi.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonChiudi.Location = new System.Drawing.Point(133, 40);
            this.simpleButtonChiudi.Name = "simpleButtonChiudi";
            this.simpleButtonChiudi.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonChiudi.TabIndex = 2;
            this.simpleButtonChiudi.Text = "Chiudi";
            this.simpleButtonChiudi.Click += new System.EventHandler(this.simpleButtonChiudi_Click);
            // 
            // TestSettings
            // 
            this.AcceptButton = this.simpleButtonInviaMailTest;
            this.ActiveGlowColor = System.Drawing.Color.DeepSkyBlue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.simpleButtonChiudi;
            this.ClientSize = new System.Drawing.Size(318, 78);
            this.Controls.Add(this.simpleButtonChiudi);
            this.Controls.Add(this.simpleButtonInviaMailTest);
            this.Controls.Add(this.textEditMailTestSettings);
            this.Controls.Add(this.labelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "TestSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TestSettings";
            this.Shown += new System.EventHandler(this.TestSettings_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.textEditMailTestSettings.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditMailTestSettings;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInviaMailTest;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonChiudi;
    }
}