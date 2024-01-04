
namespace MovimentiMagazzinoFromGespe
{
    partial class FormDettaglioSpedizioni
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDettaglioSpedizioni));
            this.simpleButtonDati = new DevExpress.XtraEditors.SimpleButton();
            this.dateEditDataDa = new DevExpress.XtraEditors.DateEdit();
            this.dateEditDataA = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.gridControlDettaglioSpedizioni = new DevExpress.XtraGrid.GridControl();
            this.dettaglioSpedizioniXCMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewDettaglioSpedizioni = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDataDoc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumDoc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVettore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadZIPCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadCountry = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnloadRegione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRifXCM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPallet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColli = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPesoReale = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPesoVolumetrico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVolume = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotaleAttivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalePassivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMargine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSpondaIdraulica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInformatoreScentifico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAppuntamentoTelefonico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageDettaglioSpedizioni = new DevExpress.XtraTab.XtraTabPage();
            this.simpleButtonEsportaGriglia = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonTuttiTraLeDate = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPageMagazzino = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataDa.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataDa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataA.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDettaglioSpedizioni)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dettaglioSpedizioniXCMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDettaglioSpedizioni)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPageDettaglioSpedizioni.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButtonDati
            // 
            this.simpleButtonDati.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonDati.Location = new System.Drawing.Point(1113, 12);
            this.simpleButtonDati.Name = "simpleButtonDati";
            this.simpleButtonDati.Size = new System.Drawing.Size(109, 23);
            this.simpleButtonDati.TabIndex = 1;
            this.simpleButtonDati.Text = "Seleziona Dati";
            this.simpleButtonDati.Click += new System.EventHandler(this.simpleButtonDati_Click);
            // 
            // dateEditDataDa
            // 
            this.dateEditDataDa.EditValue = null;
            this.dateEditDataDa.Location = new System.Drawing.Point(370, 14);
            this.dateEditDataDa.Name = "dateEditDataDa";
            this.dateEditDataDa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDataDa.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDataDa.Size = new System.Drawing.Size(171, 20);
            this.dateEditDataDa.TabIndex = 2;
            this.dateEditDataDa.EditValueChanged += new System.EventHandler(this.dateEditDataDa_EditValueChanged);
            // 
            // dateEditDataA
            // 
            this.dateEditDataA.EditValue = null;
            this.dateEditDataA.Location = new System.Drawing.Point(576, 12);
            this.dateEditDataA.Name = "dateEditDataA";
            this.dateEditDataA.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDataA.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDataA.Size = new System.Drawing.Size(171, 20);
            this.dateEditDataA.TabIndex = 2;
            this.dateEditDataA.EditValueChanged += new System.EventHandler(this.dateEditDataA_EditValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(343, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Da";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(556, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "A";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 17);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 13);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "Mandante";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(59, 14);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(259, 20);
            this.comboBoxEdit1.TabIndex = 13;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // gridControlDettaglioSpedizioni
            // 
            this.gridControlDettaglioSpedizioni.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDettaglioSpedizioni.DataSource = this.dettaglioSpedizioniXCMBindingSource;
            this.gridControlDettaglioSpedizioni.Location = new System.Drawing.Point(5, 50);
            this.gridControlDettaglioSpedizioni.MainView = this.gridViewDettaglioSpedizioni;
            this.gridControlDettaglioSpedizioni.Name = "gridControlDettaglioSpedizioni";
            this.gridControlDettaglioSpedizioni.Size = new System.Drawing.Size(1217, 540);
            this.gridControlDettaglioSpedizioni.TabIndex = 15;
            this.gridControlDettaglioSpedizioni.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDettaglioSpedizioni});
            // 
            // dettaglioSpedizioniXCMBindingSource
            // 
            this.dettaglioSpedizioniXCMBindingSource.DataSource = typeof(MovimentiMagazzinoFromGespe.DettaglioSpedizioniXCM);
            // 
            // gridViewDettaglioSpedizioni
            // 
            this.gridViewDettaglioSpedizioni.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDataDoc,
            this.colNumDoc,
            this.colShipNum,
            this.colVettore,
            this.colUnloadName,
            this.colUnloadAddress,
            this.colUnloadZIPCode,
            this.colUnloadLocation,
            this.colUnloadCountry,
            this.colUnloadRegione,
            this.colRifXCM,
            this.colPallet,
            this.colColli,
            this.colPesoReale,
            this.colPesoVolumetrico,
            this.colVolume,
            this.colTotaleAttivo,
            this.colTotalePassivo,
            this.colMargine,
            this.colSpondaIdraulica,
            this.colInformatoreScentifico,
            this.colAppuntamentoTelefonico});
            this.gridViewDettaglioSpedizioni.GridControl = this.gridControlDettaglioSpedizioni;
            this.gridViewDettaglioSpedizioni.Name = "gridViewDettaglioSpedizioni";
            this.gridViewDettaglioSpedizioni.OptionsView.ShowAutoFilterRow = true;
            this.gridViewDettaglioSpedizioni.OptionsView.ShowFooter = true;
            this.gridViewDettaglioSpedizioni.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewDettaglioSpedizioni_CustomUnboundColumnData);
            // 
            // colDataDoc
            // 
            this.colDataDoc.FieldName = "DataDoc";
            this.colDataDoc.Name = "colDataDoc";
            this.colDataDoc.Visible = true;
            this.colDataDoc.VisibleIndex = 0;
            this.colDataDoc.Width = 73;
            // 
            // colNumDoc
            // 
            this.colNumDoc.FieldName = "NumDoc";
            this.colNumDoc.Name = "colNumDoc";
            this.colNumDoc.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "NumDoc", "N° Righe {0}")});
            this.colNumDoc.Visible = true;
            this.colNumDoc.VisibleIndex = 1;
            this.colNumDoc.Width = 96;
            // 
            // colShipNum
            // 
            this.colShipNum.FieldName = "ShipNum";
            this.colShipNum.Name = "colShipNum";
            this.colShipNum.Visible = true;
            this.colShipNum.VisibleIndex = 2;
            this.colShipNum.Width = 70;
            // 
            // colVettore
            // 
            this.colVettore.FieldName = "Vettore";
            this.colVettore.Name = "colVettore";
            this.colVettore.Visible = true;
            this.colVettore.VisibleIndex = 3;
            this.colVettore.Width = 70;
            // 
            // colUnloadName
            // 
            this.colUnloadName.FieldName = "UnloadName";
            this.colUnloadName.Name = "colUnloadName";
            this.colUnloadName.Visible = true;
            this.colUnloadName.VisibleIndex = 4;
            this.colUnloadName.Width = 70;
            // 
            // colUnloadAddress
            // 
            this.colUnloadAddress.FieldName = "UnloadAddress";
            this.colUnloadAddress.Name = "colUnloadAddress";
            this.colUnloadAddress.Visible = true;
            this.colUnloadAddress.VisibleIndex = 5;
            this.colUnloadAddress.Width = 70;
            // 
            // colUnloadZIPCode
            // 
            this.colUnloadZIPCode.FieldName = "UnloadZIPCode";
            this.colUnloadZIPCode.Name = "colUnloadZIPCode";
            this.colUnloadZIPCode.Visible = true;
            this.colUnloadZIPCode.VisibleIndex = 6;
            this.colUnloadZIPCode.Width = 70;
            // 
            // colUnloadLocation
            // 
            this.colUnloadLocation.FieldName = "UnloadLocation";
            this.colUnloadLocation.Name = "colUnloadLocation";
            this.colUnloadLocation.Visible = true;
            this.colUnloadLocation.VisibleIndex = 7;
            this.colUnloadLocation.Width = 70;
            // 
            // colUnloadCountry
            // 
            this.colUnloadCountry.FieldName = "UnloadCountry";
            this.colUnloadCountry.Name = "colUnloadCountry";
            this.colUnloadCountry.Visible = true;
            this.colUnloadCountry.VisibleIndex = 8;
            this.colUnloadCountry.Width = 70;
            // 
            // colUnloadRegione
            // 
            this.colUnloadRegione.FieldName = "UnloadRegione";
            this.colUnloadRegione.Name = "colUnloadRegione";
            this.colUnloadRegione.Visible = true;
            this.colUnloadRegione.VisibleIndex = 9;
            this.colUnloadRegione.Width = 70;
            // 
            // colRifXCM
            // 
            this.colRifXCM.FieldName = "RifXCM";
            this.colRifXCM.Name = "colRifXCM";
            this.colRifXCM.Visible = true;
            this.colRifXCM.VisibleIndex = 10;
            this.colRifXCM.Width = 70;
            // 
            // colPallet
            // 
            this.colPallet.FieldName = "Pallet";
            this.colPallet.Name = "colPallet";
            this.colPallet.Visible = true;
            this.colPallet.VisibleIndex = 11;
            this.colPallet.Width = 70;
            // 
            // colColli
            // 
            this.colColli.FieldName = "Colli";
            this.colColli.Name = "colColli";
            this.colColli.Visible = true;
            this.colColli.VisibleIndex = 12;
            this.colColli.Width = 70;
            // 
            // colPesoReale
            // 
            this.colPesoReale.FieldName = "PesoReale";
            this.colPesoReale.Name = "colPesoReale";
            this.colPesoReale.Visible = true;
            this.colPesoReale.VisibleIndex = 13;
            this.colPesoReale.Width = 70;
            // 
            // colPesoVolumetrico
            // 
            this.colPesoVolumetrico.FieldName = "PesoVolumetrico";
            this.colPesoVolumetrico.Name = "colPesoVolumetrico";
            this.colPesoVolumetrico.Visible = true;
            this.colPesoVolumetrico.VisibleIndex = 14;
            this.colPesoVolumetrico.Width = 70;
            // 
            // colVolume
            // 
            this.colVolume.FieldName = "Volume";
            this.colVolume.Name = "colVolume";
            this.colVolume.Visible = true;
            this.colVolume.VisibleIndex = 15;
            this.colVolume.Width = 70;
            // 
            // colTotaleAttivo
            // 
            this.colTotaleAttivo.FieldName = "TotaleAttivo";
            this.colTotaleAttivo.Name = "colTotaleAttivo";
            this.colTotaleAttivo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotaleAttivo", "{0:c2}")});
            this.colTotaleAttivo.Visible = true;
            this.colTotaleAttivo.VisibleIndex = 16;
            this.colTotaleAttivo.Width = 98;
            // 
            // colTotalePassivo
            // 
            this.colTotalePassivo.DisplayFormat.FormatString = "c2";
            this.colTotalePassivo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotalePassivo.FieldName = "TotalePassivo";
            this.colTotalePassivo.Name = "colTotalePassivo";
            this.colTotalePassivo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalePassivo", "{0:c2}")});
            this.colTotalePassivo.Visible = true;
            this.colTotalePassivo.VisibleIndex = 17;
            this.colTotalePassivo.Width = 106;
            // 
            // colMargine
            // 
            this.colMargine.FieldName = "Margine";
            this.colMargine.Name = "colMargine";
            this.colMargine.OptionsColumn.ReadOnly = true;
            this.colMargine.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Margine", "{0:c2}")});
            this.colMargine.Visible = true;
            this.colMargine.VisibleIndex = 18;
            this.colMargine.Width = 82;
            // 
            // colSpondaIdraulica
            // 
            this.colSpondaIdraulica.FieldName = "SpondaIdraulica";
            this.colSpondaIdraulica.Name = "colSpondaIdraulica";
            this.colSpondaIdraulica.Visible = true;
            this.colSpondaIdraulica.VisibleIndex = 19;
            this.colSpondaIdraulica.Width = 67;
            // 
            // colInformatoreScentifico
            // 
            this.colInformatoreScentifico.FieldName = "InformatoreScentifico";
            this.colInformatoreScentifico.Name = "colInformatoreScentifico";
            this.colInformatoreScentifico.Visible = true;
            this.colInformatoreScentifico.VisibleIndex = 20;
            this.colInformatoreScentifico.Width = 47;
            // 
            // colAppuntamentoTelefonico
            // 
            this.colAppuntamentoTelefonico.FieldName = "AppuntamentoTelefonico";
            this.colAppuntamentoTelefonico.Name = "colAppuntamentoTelefonico";
            this.colAppuntamentoTelefonico.Visible = true;
            this.colAppuntamentoTelefonico.VisibleIndex = 21;
            this.colAppuntamentoTelefonico.Width = 66;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPageDettaglioSpedizioni;
            this.xtraTabControl1.Size = new System.Drawing.Size(1227, 614);
            this.xtraTabControl1.TabIndex = 16;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageDettaglioSpedizioni,
            this.xtraTabPageMagazzino});
            // 
            // xtraTabPageDettaglioSpedizioni
            // 
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.simpleButtonEsportaGriglia);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.simpleButtonTuttiTraLeDate);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.simpleButtonDati);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.gridControlDettaglioSpedizioni);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.dateEditDataDa);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.labelControl5);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.dateEditDataA);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.comboBoxEdit1);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.label1);
            this.xtraTabPageDettaglioSpedizioni.Controls.Add(this.label2);
            this.xtraTabPageDettaglioSpedizioni.Name = "xtraTabPageDettaglioSpedizioni";
            this.xtraTabPageDettaglioSpedizioni.Size = new System.Drawing.Size(1225, 589);
            this.xtraTabPageDettaglioSpedizioni.Text = "Dettaglio Spedizioni";
            // 
            // simpleButtonEsportaGriglia
            // 
            this.simpleButtonEsportaGriglia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonEsportaGriglia.Location = new System.Drawing.Point(998, 12);
            this.simpleButtonEsportaGriglia.Name = "simpleButtonEsportaGriglia";
            this.simpleButtonEsportaGriglia.Size = new System.Drawing.Size(109, 23);
            this.simpleButtonEsportaGriglia.TabIndex = 1;
            this.simpleButtonEsportaGriglia.Text = "Esporta Griglia";
            this.simpleButtonEsportaGriglia.Click += new System.EventHandler(this.simpleButtonEsportaGriglia_Click);
            // 
            // simpleButtonTuttiTraLeDate
            // 
            this.simpleButtonTuttiTraLeDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonTuttiTraLeDate.Location = new System.Drawing.Point(753, 9);
            this.simpleButtonTuttiTraLeDate.Name = "simpleButtonTuttiTraLeDate";
            this.simpleButtonTuttiTraLeDate.Size = new System.Drawing.Size(109, 23);
            this.simpleButtonTuttiTraLeDate.TabIndex = 1;
            this.simpleButtonTuttiTraLeDate.Text = "Tutti tra le date";
            this.simpleButtonTuttiTraLeDate.Click += new System.EventHandler(this.simpleButtonTuttiTraLeDate_Click);
            // 
            // xtraTabPageMagazzino
            // 
            this.xtraTabPageMagazzino.Name = "xtraTabPageMagazzino";
            this.xtraTabPageMagazzino.Size = new System.Drawing.Size(1225, 589);
            this.xtraTabPageMagazzino.Text = "Magazzino";
            // 
            // FormDettaglioSpedizioni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 614);
            this.Controls.Add(this.xtraTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDettaglioSpedizioni";
            this.Text = "Amministrazione XCM";
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataDa.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataDa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataA.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDataA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDettaglioSpedizioni)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dettaglioSpedizioniXCMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDettaglioSpedizioni)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPageDettaglioSpedizioni.ResumeLayout(false);
            this.xtraTabPageDettaglioSpedizioni.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButtonDati;
        private DevExpress.XtraEditors.DateEdit dateEditDataDa;
        private DevExpress.XtraEditors.DateEdit dateEditDataA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraGrid.GridControl gridControlDettaglioSpedizioni;
        private System.Windows.Forms.BindingSource dettaglioSpedizioniXCMBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDettaglioSpedizioni;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageDettaglioSpedizioni;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageMagazzino;
        private DevExpress.XtraGrid.Columns.GridColumn colDataDoc;
        private DevExpress.XtraGrid.Columns.GridColumn colNumDoc;
        private DevExpress.XtraGrid.Columns.GridColumn colShipNum;
        private DevExpress.XtraGrid.Columns.GridColumn colVettore;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadName;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadZIPCode;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadLocation;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadCountry;
        private DevExpress.XtraGrid.Columns.GridColumn colUnloadRegione;
        private DevExpress.XtraGrid.Columns.GridColumn colRifXCM;
        private DevExpress.XtraGrid.Columns.GridColumn colPallet;
        private DevExpress.XtraGrid.Columns.GridColumn colColli;
        private DevExpress.XtraGrid.Columns.GridColumn colPesoReale;
        private DevExpress.XtraGrid.Columns.GridColumn colPesoVolumetrico;
        private DevExpress.XtraGrid.Columns.GridColumn colVolume;
        private DevExpress.XtraGrid.Columns.GridColumn colTotaleAttivo;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalePassivo;
        private DevExpress.XtraGrid.Columns.GridColumn colMargine;
        private DevExpress.XtraGrid.Columns.GridColumn colSpondaIdraulica;
        private DevExpress.XtraGrid.Columns.GridColumn colInformatoreScentifico;
        private DevExpress.XtraGrid.Columns.GridColumn colAppuntamentoTelefonico;
        private DevExpress.XtraEditors.SimpleButton simpleButtonTuttiTraLeDate;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaGriglia;
    }
}