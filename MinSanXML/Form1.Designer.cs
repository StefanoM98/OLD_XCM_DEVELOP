
namespace MinSanXML
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.simpleButtonProduciXML = new DevExpress.XtraEditors.SimpleButton();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.datiBodyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBoxEditTipoTrasmissione = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEditTipoMittente = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEditTipoDestinatario = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.numericUpDownAnno = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMese = new System.Windows.Forms.NumericUpDown();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datiBodyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoTrasmissione.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoMittente.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoDestinatario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMese)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonProduciXML
            // 
            this.simpleButtonProduciXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonProduciXML.Location = new System.Drawing.Point(879, 453);
            this.simpleButtonProduciXML.Name = "simpleButtonProduciXML";
            this.simpleButtonProduciXML.Size = new System.Drawing.Size(75, 50);
            this.simpleButtonProduciXML.TabIndex = 0;
            this.simpleButtonProduciXML.Text = "Produci XML";
            this.simpleButtonProduciXML.Click += new System.EventHandler(this.simpleButtonProduciXML_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 453);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(114, 50);
            toolTipItem1.Text = "Trascina qui il template compilato per caricare i dati";
            superToolTip1.Items.Add(toolTipItem1);
            this.pictureEdit1.SuperTip = superToolTip1;
            this.pictureEdit1.TabIndex = 1;
            this.pictureEdit1.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureEdit1_DragDrop);
            this.pictureEdit1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureEdit1_DragEnter);
            // 
            // datiBodyBindingSource
            // 
            this.datiBodyBindingSource.DataSource = typeof(MinSanXML.DatiBody);
            // 
            // comboBoxEditTipoTrasmissione
            // 
            this.comboBoxEditTipoTrasmissione.Location = new System.Drawing.Point(12, 31);
            this.comboBoxEditTipoTrasmissione.Name = "comboBoxEditTipoTrasmissione";
            this.comboBoxEditTipoTrasmissione.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditTipoTrasmissione.Properties.Items.AddRange(new object[] {
            "T",
            "R",
            "E"});
            this.comboBoxEditTipoTrasmissione.Size = new System.Drawing.Size(100, 20);
            this.comboBoxEditTipoTrasmissione.TabIndex = 4;
            // 
            // comboBoxEditTipoMittente
            // 
            this.comboBoxEditTipoMittente.Location = new System.Drawing.Point(118, 31);
            this.comboBoxEditTipoMittente.Name = "comboBoxEditTipoMittente";
            this.comboBoxEditTipoMittente.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditTipoMittente.Properties.Items.AddRange(new object[] {
            "D"});
            this.comboBoxEditTipoMittente.Size = new System.Drawing.Size(100, 20);
            this.comboBoxEditTipoMittente.TabIndex = 4;
            // 
            // comboBoxEditTipoDestinatario
            // 
            this.comboBoxEditTipoDestinatario.Location = new System.Drawing.Point(224, 31);
            this.comboBoxEditTipoDestinatario.Name = "comboBoxEditTipoDestinatario";
            this.comboBoxEditTipoDestinatario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditTipoDestinatario.Properties.Items.AddRange(new object[] {
            "R"});
            this.comboBoxEditTipoDestinatario.Size = new System.Drawing.Size(100, 20);
            this.comboBoxEditTipoDestinatario.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Tipo Trasmissione";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(118, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(63, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Tipo Mittente";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(224, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(81, 13);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "Tipo Destinatario";
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl5.Location = new System.Drawing.Point(903, 12);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(25, 13);
            this.labelControl5.TabIndex = 5;
            this.labelControl5.Text = "Anno";
            // 
            // labelControl6
            // 
            this.labelControl6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl6.Location = new System.Drawing.Point(836, 13);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(25, 13);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "Mese";
            // 
            // numericUpDownAnno
            // 
            this.numericUpDownAnno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownAnno.Location = new System.Drawing.Point(892, 32);
            this.numericUpDownAnno.Maximum = new decimal(new int[] {
            2030,
            0,
            0,
            0});
            this.numericUpDownAnno.Minimum = new decimal(new int[] {
            2023,
            0,
            0,
            0});
            this.numericUpDownAnno.Name = "numericUpDownAnno";
            this.numericUpDownAnno.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownAnno.TabIndex = 6;
            this.numericUpDownAnno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownAnno.Value = new decimal(new int[] {
            2023,
            0,
            0,
            0});
            // 
            // numericUpDownMese
            // 
            this.numericUpDownMese.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMese.Location = new System.Drawing.Point(824, 32);
            this.numericUpDownMese.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownMese.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMese.Name = "numericUpDownMese";
            this.numericUpDownMese.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownMese.TabIndex = 6;
            this.numericUpDownMese.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMese.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(12, 57);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(942, 390);
            this.gridControl1.TabIndex = 7;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 515);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.numericUpDownMese);
            this.Controls.Add(this.numericUpDownAnno);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.comboBoxEditTipoDestinatario);
            this.Controls.Add(this.comboBoxEditTipoMittente);
            this.Controls.Add(this.comboBoxEditTipoTrasmissione);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.simpleButtonProduciXML);
            this.Name = "Form1";
            this.Text = "MinSan XML";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datiBodyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoTrasmissione.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoMittente.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditTipoDestinatario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAnno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMese)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonProduciXML;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditTipoTrasmissione;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditTipoMittente;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditTipoDestinatario;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.NumericUpDown numericUpDownAnno;
        private System.Windows.Forms.NumericUpDown numericUpDownMese;
        private System.Windows.Forms.BindingSource datiBodyBindingSource;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}

