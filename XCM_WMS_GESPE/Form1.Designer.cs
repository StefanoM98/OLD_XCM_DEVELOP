
namespace XCM_WMS_GESPE
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
            this.gnXcmDataSet = new XCM_WMS_GESPE.GnXcmDataSet();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.giacenzaProdottoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDataRiferimento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRiferimento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodiceProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescrizioneProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantitaTotale = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnReparto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMapID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataScadenza = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShelfLifeFromExpire = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShelflifePrd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGiorniRimanentiAllaVendita = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMagazzinoLogico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDataAllaVendita = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnAllaScadenzaEffettiva = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButtonEsportaXslx = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonInviaReportATutti = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gnXcmDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.giacenzaProdottoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gnXcmDataSet
            // 
            this.gnXcmDataSet.DataSetName = "GnXcmDataSet";
            this.gnXcmDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(12, 26);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxEdit1.Properties.Appearance.Options.UseFont = true;
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(290, 22);
            this.comboBoxEdit1.TabIndex = 1;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.DataSource = this.giacenzaProdottoBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(12, 54);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1239, 517);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDataRiferimento,
            this.colRiferimento,
            this.colCodiceProdotto,
            this.colDescrizioneProdotto,
            this.colQuantitaTotale,
            this.gridColumnReparto,
            this.colMapID,
            this.colLotto,
            this.colDataScadenza,
            this.colShelfLifeFromExpire,
            this.colShelflifePrd,
            this.colGiorniRimanentiAllaVendita,
            this.colMagazzinoLogico,
            this.gridColumnDataAllaVendita,
            this.gridColumnAllaScadenzaEffettiva});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "QuantitaTotale", this.colQuantitaTotale, "", 0D)});
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            // 
            // colDataRiferimento
            // 
            this.colDataRiferimento.FieldName = "DataRiferimento";
            this.colDataRiferimento.Name = "colDataRiferimento";
            this.colDataRiferimento.Width = 104;
            // 
            // colRiferimento
            // 
            this.colRiferimento.FieldName = "Riferimento";
            this.colRiferimento.Name = "colRiferimento";
            this.colRiferimento.Width = 99;
            // 
            // colCodiceProdotto
            // 
            this.colCodiceProdotto.AppearanceCell.Options.UseTextOptions = true;
            this.colCodiceProdotto.Caption = "Codice Articolo";
            this.colCodiceProdotto.FieldName = "CodiceProdotto";
            this.colCodiceProdotto.Name = "colCodiceProdotto";
            this.colCodiceProdotto.Visible = true;
            this.colCodiceProdotto.VisibleIndex = 1;
            this.colCodiceProdotto.Width = 124;
            // 
            // colDescrizioneProdotto
            // 
            this.colDescrizioneProdotto.AppearanceCell.Options.UseTextOptions = true;
            this.colDescrizioneProdotto.FieldName = "DescrizioneProdotto";
            this.colDescrizioneProdotto.Name = "colDescrizioneProdotto";
            this.colDescrizioneProdotto.Visible = true;
            this.colDescrizioneProdotto.VisibleIndex = 2;
            this.colDescrizioneProdotto.Width = 258;
            // 
            // colQuantitaTotale
            // 
            this.colQuantitaTotale.AppearanceCell.Options.UseTextOptions = true;
            this.colQuantitaTotale.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colQuantitaTotale.DisplayFormat.FormatString = "N0";
            this.colQuantitaTotale.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantitaTotale.FieldName = "QuantitaTotale";
            this.colQuantitaTotale.Name = "colQuantitaTotale";
            this.colQuantitaTotale.Visible = true;
            this.colQuantitaTotale.VisibleIndex = 3;
            this.colQuantitaTotale.Width = 113;
            // 
            // gridColumnReparto
            // 
            this.gridColumnReparto.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnReparto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnReparto.Caption = "Reparto";
            this.gridColumnReparto.FieldName = "gridColumnReparto";
            this.gridColumnReparto.Name = "gridColumnReparto";
            this.gridColumnReparto.UnboundDataType = typeof(string);
            this.gridColumnReparto.Visible = true;
            this.gridColumnReparto.VisibleIndex = 0;
            this.gridColumnReparto.Width = 138;
            // 
            // colMapID
            // 
            this.colMapID.AppearanceCell.Options.UseTextOptions = true;
            this.colMapID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMapID.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colMapID.FieldName = "MapID";
            this.colMapID.Name = "colMapID";
            this.colMapID.Width = 131;
            // 
            // colLotto
            // 
            this.colLotto.AppearanceCell.Options.UseTextOptions = true;
            this.colLotto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLotto.FieldName = "Lotto";
            this.colLotto.Name = "colLotto";
            this.colLotto.Visible = true;
            this.colLotto.VisibleIndex = 4;
            this.colLotto.Width = 130;
            // 
            // colDataScadenza
            // 
            this.colDataScadenza.AppearanceCell.Options.UseTextOptions = true;
            this.colDataScadenza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDataScadenza.FieldName = "DataScadenza";
            this.colDataScadenza.Name = "colDataScadenza";
            this.colDataScadenza.Visible = true;
            this.colDataScadenza.VisibleIndex = 5;
            this.colDataScadenza.Width = 116;
            // 
            // colShelfLifeFromExpire
            // 
            this.colShelfLifeFromExpire.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colShelfLifeFromExpire.FieldName = "ShelfLifeFromExpire";
            this.colShelfLifeFromExpire.Name = "colShelfLifeFromExpire";
            // 
            // colShelflifePrd
            // 
            this.colShelflifePrd.AppearanceCell.Options.UseTextOptions = true;
            this.colShelflifePrd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colShelflifePrd.Caption = "Shelf Life Articolo";
            this.colShelflifePrd.FieldName = "ShelflifePrd";
            this.colShelflifePrd.Name = "colShelflifePrd";
            this.colShelflifePrd.Visible = true;
            this.colShelflifePrd.VisibleIndex = 6;
            this.colShelflifePrd.Width = 94;
            // 
            // colGiorniRimanentiAllaVendita
            // 
            this.colGiorniRimanentiAllaVendita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colGiorniRimanentiAllaVendita.Name = "colGiorniRimanentiAllaVendita";
            this.colGiorniRimanentiAllaVendita.OptionsColumn.ReadOnly = true;
            this.colGiorniRimanentiAllaVendita.UnboundDataType = typeof(System.TimeSpan);
            // 
            // colMagazzinoLogico
            // 
            this.colMagazzinoLogico.AppearanceCell.Options.UseTextOptions = true;
            this.colMagazzinoLogico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMagazzinoLogico.Caption = "Magazzino Logico";
            this.colMagazzinoLogico.FieldName = "MagazzinoLogico";
            this.colMagazzinoLogico.Name = "colMagazzinoLogico";
            this.colMagazzinoLogico.Visible = true;
            this.colMagazzinoLogico.VisibleIndex = 9;
            this.colMagazzinoLogico.Width = 164;
            // 
            // gridColumnDataAllaVendita
            // 
            this.gridColumnDataAllaVendita.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnDataAllaVendita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDataAllaVendita.Caption = "Scadenza alla Vendita";
            this.gridColumnDataAllaVendita.FieldName = "gridColumnDataAllaVendita";
            this.gridColumnDataAllaVendita.Name = "gridColumnDataAllaVendita";
            this.gridColumnDataAllaVendita.UnboundDataType = typeof(int);
            this.gridColumnDataAllaVendita.Visible = true;
            this.gridColumnDataAllaVendita.VisibleIndex = 7;
            this.gridColumnDataAllaVendita.Width = 127;
            // 
            // gridColumnAllaScadenzaEffettiva
            // 
            this.gridColumnAllaScadenzaEffettiva.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumnAllaScadenzaEffettiva.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnAllaScadenzaEffettiva.Caption = "Scadenza Effettiva";
            this.gridColumnAllaScadenzaEffettiva.FieldName = "gridColumnAllaScadenzaEffettiva";
            this.gridColumnAllaScadenzaEffettiva.Name = "gridColumnAllaScadenzaEffettiva";
            this.gridColumnAllaScadenzaEffettiva.UnboundDataType = typeof(int);
            this.gridColumnAllaScadenzaEffettiva.Visible = true;
            this.gridColumnAllaScadenzaEffettiva.VisibleIndex = 8;
            this.gridColumnAllaScadenzaEffettiva.Width = 114;
            // 
            // simpleButtonEsportaXslx
            // 
            this.simpleButtonEsportaXslx.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonEsportaXslx.Appearance.Options.UseFont = true;
            this.simpleButtonEsportaXslx.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEsportaXslx.ImageOptions.Image")));
            this.simpleButtonEsportaXslx.Location = new System.Drawing.Point(308, 12);
            this.simpleButtonEsportaXslx.Name = "simpleButtonEsportaXslx";
            this.simpleButtonEsportaXslx.Size = new System.Drawing.Size(164, 35);
            this.simpleButtonEsportaXslx.TabIndex = 3;
            this.simpleButtonEsportaXslx.Text = "Esporta Singolo";
            this.simpleButtonEsportaXslx.Click += new System.EventHandler(this.simpleButtonEsportaXslx_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 16);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Mandante";
            // 
            // simpleButtonInviaReportATutti
            // 
            this.simpleButtonInviaReportATutti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonInviaReportATutti.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonInviaReportATutti.Appearance.Options.UseFont = true;
            this.simpleButtonInviaReportATutti.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonInviaReportATutti.ImageOptions.Image")));
            this.simpleButtonInviaReportATutti.Location = new System.Drawing.Point(1020, 12);
            this.simpleButtonInviaReportATutti.Name = "simpleButtonInviaReportATutti";
            this.simpleButtonInviaReportATutti.Size = new System.Drawing.Size(231, 36);
            this.simpleButtonInviaReportATutti.TabIndex = 3;
            this.simpleButtonInviaReportATutti.Text = "Invia report a tutti i mandanti";
            this.simpleButtonInviaReportATutti.Click += new System.EventHandler(this.simpleButtonInviaReportATutti_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 583);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButtonInviaReportATutti);
            this.Controls.Add(this.simpleButtonEsportaXslx);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.comboBoxEdit1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Export Giacenze";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;          
            ((System.ComponentModel.ISupportInitialize)(this.gnXcmDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.giacenzaProdottoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private GnXcmDataSet gnXcmDataSet;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource giacenzaProdottoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colDataRiferimento;
        private DevExpress.XtraGrid.Columns.GridColumn colRiferimento;
        private DevExpress.XtraGrid.Columns.GridColumn colCodiceProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colDescrizioneProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantitaTotale;
        private DevExpress.XtraGrid.Columns.GridColumn colMapID;
        private DevExpress.XtraGrid.Columns.GridColumn colLotto;
        private DevExpress.XtraGrid.Columns.GridColumn colDataScadenza;
        private DevExpress.XtraGrid.Columns.GridColumn colShelfLifeFromExpire;
        private DevExpress.XtraGrid.Columns.GridColumn colShelflifePrd;
        private DevExpress.XtraGrid.Columns.GridColumn colGiorniRimanentiAllaVendita;
        private DevExpress.XtraGrid.Columns.GridColumn colMagazzinoLogico;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDataAllaVendita;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAllaScadenzaEffettiva;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaXslx;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInviaReportATutti;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnReparto;
    }
}

