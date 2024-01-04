
namespace EzOrdiniRemoti
{
    partial class FormOrdini
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOrdini));
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            this.gridControlProdotti = new DevExpress.XtraGrid.GridControl();
            this.vociOrdineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewProdotti = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDataOrdine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNomeDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndirizzoDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCAPDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCittaDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProvDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRagioneSocialeFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPIVAFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndirizzoFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCAPFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCittaFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProvFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumeroOrdine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescrizioneProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantitaProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSconto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImportoUnitario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImportoTotale = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataConsegnaRichiesta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNote = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBarcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRegione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.defaultToolTipController1 = new DevExpress.Utils.DefaultToolTipController(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.impostazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.storicoOrdiniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anagraficaArticoliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anagraficaDestinatariToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anagraficaFatturazioneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personalizzazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aiutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connessioneRemotaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
            this.checkEditSalvaNuoviProdotti = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditSalvaNuoviDatiFatturazione = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditSalvaNuoviDatiDestinazione = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButtonInviaOrdine = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonAnnullaOrdine = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.simpleButtonConvertiInCSV = new DevExpress.XtraEditors.SimpleButton();
            this.comboBoxEditVettore = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEditMandante = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.gridControlExcelIngresso = new DevExpress.XtraGrid.GridControl();
            this.gridViewExcelIngresso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.labelControlTrascina = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProdotti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vociOrdineBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProdotti)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviProdotti.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviDatiFatturazione.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviDatiDestinazione.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditVettore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditMandante.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlExcelIngresso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewExcelIngresso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlProdotti
            // 
            this.gridControlProdotti.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlProdotti.DataSource = this.vociOrdineBindingSource;
            this.gridControlProdotti.Location = new System.Drawing.Point(0, 32);
            this.gridControlProdotti.MainView = this.gridViewProdotti;
            this.gridControlProdotti.Name = "gridControlProdotti";
            this.gridControlProdotti.Size = new System.Drawing.Size(1593, 326);
            this.gridControlProdotti.TabIndex = 1;
            this.gridControlProdotti.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewProdotti});
            // 
            // vociOrdineBindingSource
            // 
            this.vociOrdineBindingSource.DataSource = typeof(PoolingFileDaElaborare.VociOrdine);
            // 
            // gridViewProdotti
            // 
            this.gridViewProdotti.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDataOrdine,
            this.colNomeDestinazione,
            this.colIndirizzoDestinazione,
            this.colCAPDestinazione,
            this.colCittaDestinazione,
            this.colProvDestinazione,
            this.colRagioneSocialeFatturazione,
            this.colPIVAFatturazione,
            this.colIndirizzoFatturazione,
            this.colCAPFatturazione,
            this.colCittaFatturazione,
            this.colProvFatturazione,
            this.colNumeroOrdine,
            this.colCodProdotto,
            this.colDescrizioneProdotto,
            this.colQuantitaProdotto,
            this.colSconto,
            this.colImportoUnitario,
            this.colImportoTotale,
            this.colDataConsegnaRichiesta,
            this.colNote,
            this.colLotto,
            this.colIVA,
            this.colBarcode,
            this.gridColumnRegione});
            this.gridViewProdotti.GridControl = this.gridControlProdotti;
            this.gridViewProdotti.Name = "gridViewProdotti";
            this.gridViewProdotti.OptionsCustomization.AllowGroup = false;
            this.gridViewProdotti.OptionsView.ColumnAutoWidth = false;
            this.gridViewProdotti.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridViewProdotti.OptionsView.ShowGroupPanel = false;
            // 
            // colDataOrdine
            // 
            this.colDataOrdine.Caption = "Data Ordine";
            this.colDataOrdine.FieldName = "DataOrdine";
            this.colDataOrdine.Name = "colDataOrdine";
            this.colDataOrdine.Visible = true;
            this.colDataOrdine.VisibleIndex = 1;
            // 
            // colNomeDestinazione
            // 
            this.colNomeDestinazione.Caption = "Nome Dest.";
            this.colNomeDestinazione.FieldName = "NomeDestinazione";
            this.colNomeDestinazione.Name = "colNomeDestinazione";
            this.colNomeDestinazione.Visible = true;
            this.colNomeDestinazione.VisibleIndex = 17;
            // 
            // colIndirizzoDestinazione
            // 
            this.colIndirizzoDestinazione.Caption = "Ind. Dest.";
            this.colIndirizzoDestinazione.FieldName = "IndirizzoDestinazione";
            this.colIndirizzoDestinazione.Name = "colIndirizzoDestinazione";
            this.colIndirizzoDestinazione.Visible = true;
            this.colIndirizzoDestinazione.VisibleIndex = 18;
            // 
            // colCAPDestinazione
            // 
            this.colCAPDestinazione.Caption = "CAP Dest.";
            this.colCAPDestinazione.FieldName = "CAPDestinazione";
            this.colCAPDestinazione.Name = "colCAPDestinazione";
            this.colCAPDestinazione.Visible = true;
            this.colCAPDestinazione.VisibleIndex = 19;
            // 
            // colCittaDestinazione
            // 
            this.colCittaDestinazione.Caption = "Città Dest.";
            this.colCittaDestinazione.FieldName = "CittaDestinazione";
            this.colCittaDestinazione.Name = "colCittaDestinazione";
            this.colCittaDestinazione.Visible = true;
            this.colCittaDestinazione.VisibleIndex = 20;
            // 
            // colProvDestinazione
            // 
            this.colProvDestinazione.Caption = "Prov. Dest.";
            this.colProvDestinazione.FieldName = "ProvDestinazione";
            this.colProvDestinazione.Name = "colProvDestinazione";
            this.colProvDestinazione.Visible = true;
            this.colProvDestinazione.VisibleIndex = 21;
            // 
            // colRagioneSocialeFatturazione
            // 
            this.colRagioneSocialeFatturazione.Caption = "Nome Fatt.";
            this.colRagioneSocialeFatturazione.FieldName = "RagioneSocialeFatturazione";
            this.colRagioneSocialeFatturazione.Name = "colRagioneSocialeFatturazione";
            this.colRagioneSocialeFatturazione.Visible = true;
            this.colRagioneSocialeFatturazione.VisibleIndex = 11;
            // 
            // colPIVAFatturazione
            // 
            this.colPIVAFatturazione.Caption = "P.IVA Fatt.";
            this.colPIVAFatturazione.FieldName = "PIVAFatturazione";
            this.colPIVAFatturazione.Name = "colPIVAFatturazione";
            this.colPIVAFatturazione.Visible = true;
            this.colPIVAFatturazione.VisibleIndex = 12;
            // 
            // colIndirizzoFatturazione
            // 
            this.colIndirizzoFatturazione.Caption = "Ind. Fatt.";
            this.colIndirizzoFatturazione.FieldName = "IndirizzoFatturazione";
            this.colIndirizzoFatturazione.Name = "colIndirizzoFatturazione";
            this.colIndirizzoFatturazione.Visible = true;
            this.colIndirizzoFatturazione.VisibleIndex = 13;
            // 
            // colCAPFatturazione
            // 
            this.colCAPFatturazione.Caption = "CAP Fatt.";
            this.colCAPFatturazione.FieldName = "CAPFatturazione";
            this.colCAPFatturazione.Name = "colCAPFatturazione";
            this.colCAPFatturazione.Visible = true;
            this.colCAPFatturazione.VisibleIndex = 14;
            // 
            // colCittaFatturazione
            // 
            this.colCittaFatturazione.Caption = "Città Fatt.";
            this.colCittaFatturazione.FieldName = "CittaFatturazione";
            this.colCittaFatturazione.Name = "colCittaFatturazione";
            this.colCittaFatturazione.Visible = true;
            this.colCittaFatturazione.VisibleIndex = 15;
            // 
            // colProvFatturazione
            // 
            this.colProvFatturazione.Caption = "Prov. Fatt.";
            this.colProvFatturazione.FieldName = "ProvFatturazione";
            this.colProvFatturazione.Name = "colProvFatturazione";
            this.colProvFatturazione.Visible = true;
            this.colProvFatturazione.VisibleIndex = 16;
            // 
            // colNumeroOrdine
            // 
            this.colNumeroOrdine.Caption = "Num. Ordine";
            this.colNumeroOrdine.FieldName = "NumeroOrdine";
            this.colNumeroOrdine.Name = "colNumeroOrdine";
            this.colNumeroOrdine.Visible = true;
            this.colNumeroOrdine.VisibleIndex = 0;
            // 
            // colCodProdotto
            // 
            this.colCodProdotto.Caption = "Cod. Prod.";
            this.colCodProdotto.FieldName = "CodProdotto";
            this.colCodProdotto.Name = "colCodProdotto";
            this.colCodProdotto.Visible = true;
            this.colCodProdotto.VisibleIndex = 2;
            // 
            // colDescrizioneProdotto
            // 
            this.colDescrizioneProdotto.Caption = "Desc. Prod";
            this.colDescrizioneProdotto.FieldName = "DescrizioneProdotto";
            this.colDescrizioneProdotto.Name = "colDescrizioneProdotto";
            this.colDescrizioneProdotto.Visible = true;
            this.colDescrizioneProdotto.VisibleIndex = 3;
            // 
            // colQuantitaProdotto
            // 
            this.colQuantitaProdotto.Caption = "Qtà Prod.";
            this.colQuantitaProdotto.FieldName = "QuantitaProdotto";
            this.colQuantitaProdotto.Name = "colQuantitaProdotto";
            this.colQuantitaProdotto.Visible = true;
            this.colQuantitaProdotto.VisibleIndex = 4;
            // 
            // colSconto
            // 
            this.colSconto.Caption = "Sconto";
            this.colSconto.FieldName = "Sconto";
            this.colSconto.Name = "colSconto";
            this.colSconto.Visible = true;
            this.colSconto.VisibleIndex = 7;
            // 
            // colImportoUnitario
            // 
            this.colImportoUnitario.Caption = "Imp. Unit.";
            this.colImportoUnitario.FieldName = "ImportoUnitario";
            this.colImportoUnitario.Name = "colImportoUnitario";
            this.colImportoUnitario.Visible = true;
            this.colImportoUnitario.VisibleIndex = 5;
            // 
            // colImportoTotale
            // 
            this.colImportoTotale.Caption = "Imp. Tot.";
            this.colImportoTotale.FieldName = "ImportoTotale";
            this.colImportoTotale.Name = "colImportoTotale";
            this.colImportoTotale.Visible = true;
            this.colImportoTotale.VisibleIndex = 6;
            // 
            // colDataConsegnaRichiesta
            // 
            this.colDataConsegnaRichiesta.Caption = "Data Cons.";
            this.colDataConsegnaRichiesta.FieldName = "DataConsegnaRichiesta";
            this.colDataConsegnaRichiesta.Name = "colDataConsegnaRichiesta";
            // 
            // colNote
            // 
            this.colNote.Caption = "Note";
            this.colNote.FieldName = "Note";
            this.colNote.Name = "colNote";
            this.colNote.Visible = true;
            this.colNote.VisibleIndex = 22;
            // 
            // colLotto
            // 
            this.colLotto.Caption = "Lotto";
            this.colLotto.FieldName = "Lotto";
            this.colLotto.Name = "colLotto";
            this.colLotto.Visible = true;
            this.colLotto.VisibleIndex = 10;
            // 
            // colIVA
            // 
            this.colIVA.Caption = "IVA";
            this.colIVA.FieldName = "IVA";
            this.colIVA.Name = "colIVA";
            this.colIVA.Visible = true;
            this.colIVA.VisibleIndex = 8;
            // 
            // colBarcode
            // 
            this.colBarcode.Caption = "Barcode";
            this.colBarcode.FieldName = "Barcode";
            this.colBarcode.Name = "colBarcode";
            this.colBarcode.Visible = true;
            this.colBarcode.VisibleIndex = 9;
            // 
            // gridColumnRegione
            // 
            this.gridColumnRegione.Caption = "Regione";
            this.gridColumnRegione.FieldName = "Regione";
            this.gridColumnRegione.Name = "gridColumnRegione";
            this.gridColumnRegione.Visible = true;
            this.gridColumnRegione.VisibleIndex = 23;
            // 
            // defaultToolTipController1
            // 
            // 
            // 
            // 
            this.defaultToolTipController1.DefaultController.KeepWhileHovered = true;
            // 
            // menuStrip1
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this.menuStrip1, DevExpress.Utils.DefaultBoolean.Default);
            this.menuStrip1.Enabled = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.impostazioniToolStripMenuItem,
            this.personalizzazioniToolStripMenuItem,
            this.aiutoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1593, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // impostazioniToolStripMenuItem
            // 
            this.impostazioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.storicoOrdiniToolStripMenuItem,
            this.anagraficaArticoliToolStripMenuItem,
            this.anagraficaDestinatariToolStripMenuItem,
            this.anagraficaFatturazioneToolStripMenuItem});
            this.impostazioniToolStripMenuItem.Name = "impostazioniToolStripMenuItem";
            this.impostazioniToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.impostazioniToolStripMenuItem.Text = "Archivio";
            // 
            // storicoOrdiniToolStripMenuItem
            // 
            this.storicoOrdiniToolStripMenuItem.Name = "storicoOrdiniToolStripMenuItem";
            this.storicoOrdiniToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.storicoOrdiniToolStripMenuItem.Text = "Storico Ordini";
            this.storicoOrdiniToolStripMenuItem.Click += new System.EventHandler(this.storicoOrdiniToolStripMenuItem_Click);
            // 
            // anagraficaArticoliToolStripMenuItem
            // 
            this.anagraficaArticoliToolStripMenuItem.Name = "anagraficaArticoliToolStripMenuItem";
            this.anagraficaArticoliToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.anagraficaArticoliToolStripMenuItem.Text = "Anagrafica Articoli";
            this.anagraficaArticoliToolStripMenuItem.Click += new System.EventHandler(this.anagraficaArticoliToolStripMenuItem_Click);
            // 
            // anagraficaDestinatariToolStripMenuItem
            // 
            this.anagraficaDestinatariToolStripMenuItem.Name = "anagraficaDestinatariToolStripMenuItem";
            this.anagraficaDestinatariToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.anagraficaDestinatariToolStripMenuItem.Text = "Anagrafica Destinatari";
            this.anagraficaDestinatariToolStripMenuItem.Click += new System.EventHandler(this.anagraficaDestinatariToolStripMenuItem_Click);
            // 
            // anagraficaFatturazioneToolStripMenuItem
            // 
            this.anagraficaFatturazioneToolStripMenuItem.Name = "anagraficaFatturazioneToolStripMenuItem";
            this.anagraficaFatturazioneToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.anagraficaFatturazioneToolStripMenuItem.Text = "Anagrafica Fatturazione";
            this.anagraficaFatturazioneToolStripMenuItem.Click += new System.EventHandler(this.anagraficaFatturazioneToolStripMenuItem_Click);
            // 
            // personalizzazioniToolStripMenuItem
            // 
            this.personalizzazioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eMailToolStripMenuItem});
            this.personalizzazioniToolStripMenuItem.Name = "personalizzazioniToolStripMenuItem";
            this.personalizzazioniToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.personalizzazioniToolStripMenuItem.Text = "Impostazioni";
            this.personalizzazioniToolStripMenuItem.Click += new System.EventHandler(this.personalizzazioniToolStripMenuItem_Click);
            // 
            // eMailToolStripMenuItem
            // 
            this.eMailToolStripMenuItem.Name = "eMailToolStripMenuItem";
            this.eMailToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.eMailToolStripMenuItem.Text = "e-Mail";
            this.eMailToolStripMenuItem.Click += new System.EventHandler(this.eMailToolStripMenuItem_Click);
            // 
            // aiutoToolStripMenuItem
            // 
            this.aiutoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualeToolStripMenuItem,
            this.connessioneRemotaToolStripMenuItem,
            this.informazioniToolStripMenuItem});
            this.aiutoToolStripMenuItem.Name = "aiutoToolStripMenuItem";
            this.aiutoToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aiutoToolStripMenuItem.Text = "Aiuto";
            // 
            // manualeToolStripMenuItem
            // 
            this.manualeToolStripMenuItem.Name = "manualeToolStripMenuItem";
            this.manualeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.manualeToolStripMenuItem.Text = "Manuale D\'Uso";
            this.manualeToolStripMenuItem.Click += new System.EventHandler(this.manualeToolStripMenuItem_Click);
            // 
            // connessioneRemotaToolStripMenuItem
            // 
            this.connessioneRemotaToolStripMenuItem.Name = "connessioneRemotaToolStripMenuItem";
            this.connessioneRemotaToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.connessioneRemotaToolStripMenuItem.Text = "Asistenza Remota";
            this.connessioneRemotaToolStripMenuItem.Click += new System.EventHandler(this.assistenzaeRemotaToolStripMenuItem_Click);
            // 
            // informazioniToolStripMenuItem
            // 
            this.informazioniToolStripMenuItem.Name = "informazioniToolStripMenuItem";
            this.informazioniToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.informazioniToolStripMenuItem.Text = "Informazioni";
            this.informazioniToolStripMenuItem.Click += new System.EventHandler(this.informazioniToolStripMenuItem_Click);
            // 
            // pdfViewer1
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this.pdfViewer1, DevExpress.Utils.DefaultBoolean.Default);
            this.pdfViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pdfViewer1.Location = new System.Drawing.Point(7, 5);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.Size = new System.Drawing.Size(1569, 496);
            this.pdfViewer1.TabIndex = 0;
            this.pdfViewer1.Visible = false;
            // 
            // checkEditSalvaNuoviProdotti
            // 
            this.checkEditSalvaNuoviProdotti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditSalvaNuoviProdotti.Enabled = false;
            this.checkEditSalvaNuoviProdotti.Location = new System.Drawing.Point(1075, 7);
            this.checkEditSalvaNuoviProdotti.Name = "checkEditSalvaNuoviProdotti";
            this.checkEditSalvaNuoviProdotti.Properties.Caption = "Salva nuovi prodotti inseriti";
            this.checkEditSalvaNuoviProdotti.Size = new System.Drawing.Size(168, 19);
            toolTipItem1.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipItem1.Text = "<b>Salva nuovi prodotti</b>\r\n\r\nSe attivata il programma salverà i prodotti inseri" +
    "ti nella griglia in modo che successivamente inserendo solo il codice prodotto r" +
    "ichiamerà i dati accessori";
            superToolTip1.Items.Add(toolTipItem1);
            this.checkEditSalvaNuoviProdotti.SuperTip = superToolTip1;
            this.checkEditSalvaNuoviProdotti.TabIndex = 7;
            // 
            // checkEditSalvaNuoviDatiFatturazione
            // 
            this.checkEditSalvaNuoviDatiFatturazione.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditSalvaNuoviDatiFatturazione.Enabled = false;
            this.checkEditSalvaNuoviDatiFatturazione.Location = new System.Drawing.Point(1249, 7);
            this.checkEditSalvaNuoviDatiFatturazione.Name = "checkEditSalvaNuoviDatiFatturazione";
            this.checkEditSalvaNuoviDatiFatturazione.Properties.Caption = "Salva nuovi dati fatturazione";
            this.checkEditSalvaNuoviDatiFatturazione.Size = new System.Drawing.Size(168, 19);
            toolTipItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipItem2.Text = "<b>Sakva dati Fatturazione</b>\r\n\r\nSe abilitata i dati di fatturazione inseriti in" +
    " modo che la prossima volta inserendo solo la partita iva vengano richiamati tut" +
    "ti i dati salvati nei campi restanti";
            superToolTip2.Items.Add(toolTipItem2);
            this.checkEditSalvaNuoviDatiFatturazione.SuperTip = superToolTip2;
            this.checkEditSalvaNuoviDatiFatturazione.TabIndex = 7;
            // 
            // checkEditSalvaNuoviDatiDestinazione
            // 
            this.checkEditSalvaNuoviDatiDestinazione.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditSalvaNuoviDatiDestinazione.Enabled = false;
            this.checkEditSalvaNuoviDatiDestinazione.Location = new System.Drawing.Point(1413, 7);
            this.checkEditSalvaNuoviDatiDestinazione.Name = "checkEditSalvaNuoviDatiDestinazione";
            this.checkEditSalvaNuoviDatiDestinazione.Properties.Caption = "Salva nuovi dati destinatario";
            this.checkEditSalvaNuoviDatiDestinazione.Size = new System.Drawing.Size(168, 19);
            this.checkEditSalvaNuoviDatiDestinazione.TabIndex = 7;
            // 
            // simpleButtonInviaOrdine
            // 
            this.simpleButtonInviaOrdine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonInviaOrdine.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonInviaOrdine.ImageOptions.Image")));
            this.simpleButtonInviaOrdine.Location = new System.Drawing.Point(1465, 735);
            this.simpleButtonInviaOrdine.Name = "simpleButtonInviaOrdine";
            this.simpleButtonInviaOrdine.Size = new System.Drawing.Size(128, 38);
            toolTipItem3.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipItem3.Text = "<b>Invia Ordine</b>\r\n\r\nInvia L\'ordine a video ad XCM Healthcare";
            superToolTip3.Items.Add(toolTipItem3);
            this.simpleButtonInviaOrdine.SuperTip = superToolTip3;
            this.simpleButtonInviaOrdine.TabIndex = 8;
            this.simpleButtonInviaOrdine.Text = "Invia Ordine";
            this.simpleButtonInviaOrdine.Click += new System.EventHandler(this.simpleButtonInviaOrdine_Click);
            // 
            // simpleButtonAnnullaOrdine
            // 
            this.simpleButtonAnnullaOrdine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonAnnullaOrdine.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAnnullaOrdine.ImageOptions.Image")));
            this.simpleButtonAnnullaOrdine.Location = new System.Drawing.Point(3, 735);
            this.simpleButtonAnnullaOrdine.Name = "simpleButtonAnnullaOrdine";
            this.simpleButtonAnnullaOrdine.Size = new System.Drawing.Size(128, 38);
            toolTipItem4.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipItem4.Text = "<b>Annulla Ordine</b>\r\n\r\nPulisci tutti i campi a video senza salvare nulla";
            superToolTip4.Items.Add(toolTipItem4);
            this.simpleButtonAnnullaOrdine.SuperTip = superToolTip4;
            this.simpleButtonAnnullaOrdine.TabIndex = 8;
            this.simpleButtonAnnullaOrdine.Text = "Annulla Ordine";
            this.simpleButtonAnnullaOrdine.Click += new System.EventHandler(this.simpleButtonAnnullaOrdine_Click);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 24);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButtonConvertiInCSV);
            this.splitContainerControl1.Panel1.Controls.Add(this.comboBoxEditVettore);
            this.splitContainerControl1.Panel1.Controls.Add(this.comboBoxEditMandante);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl13);
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControlProdotti);
            this.splitContainerControl1.Panel1.Controls.Add(this.checkEditSalvaNuoviDatiDestinazione);
            this.splitContainerControl1.Panel1.Controls.Add(this.checkEditSalvaNuoviProdotti);
            this.splitContainerControl1.Panel1.Controls.Add(this.checkEditSalvaNuoviDatiFatturazione);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControlExcelIngresso);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControlTrascina);
            this.splitContainerControl1.Panel2.Controls.Add(this.memoEdit1);
            this.splitContainerControl1.Panel2.Controls.Add(this.pdfViewer1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Panel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.splitContainerControl1_Panel2_DragDrop);
            this.splitContainerControl1.Panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.splitContainerControl1_Panel2_DragEnter);
            this.splitContainerControl1.Size = new System.Drawing.Size(1593, 705);
            this.splitContainerControl1.SplitterPosition = 366;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.ToolTipController = this.defaultToolTipController1.DefaultController;
            // 
            // simpleButtonConvertiInCSV
            // 
            this.simpleButtonConvertiInCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonConvertiInCSV.Location = new System.Drawing.Point(920, 4);
            this.simpleButtonConvertiInCSV.Name = "simpleButtonConvertiInCSV";
            this.simpleButtonConvertiInCSV.Size = new System.Drawing.Size(133, 23);
            this.simpleButtonConvertiInCSV.TabIndex = 8;
            this.simpleButtonConvertiInCSV.Text = "Converti in CSV";
            this.simpleButtonConvertiInCSV.Click += new System.EventHandler(this.simpleButtonConvertiInCSV_Click);
            // 
            // comboBoxEditVettore
            // 
            this.comboBoxEditVettore.Location = new System.Drawing.Point(400, 6);
            this.comboBoxEditVettore.Name = "comboBoxEditVettore";
            this.comboBoxEditVettore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditVettore.Properties.Items.AddRange(new object[] {
            "UNITEX",
            "CASILLI",
            "CDL EXPRESS",
            "FORTINO",
            "GLS",
            "IMPROTA",
            "TLI"});
            this.comboBoxEditVettore.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditVettore.Size = new System.Drawing.Size(220, 20);
            this.comboBoxEditVettore.TabIndex = 5;
            // 
            // comboBoxEditMandante
            // 
            this.comboBoxEditMandante.Location = new System.Drawing.Point(70, 6);
            this.comboBoxEditMandante.Name = "comboBoxEditMandante";
            this.comboBoxEditMandante.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditMandante.Properties.Items.AddRange(new object[] {
            "FARMAIMPRESA",
            "PMS",
            "POLARIS",
            "FALQUI",
            "KPS",
            "APS"});
            this.comboBoxEditMandante.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditMandante.Properties.EditValueChanged += new System.EventHandler(this.comboBoxEdit1_Properties_EditValueChanged);
            this.comboBoxEditMandante.Size = new System.Drawing.Size(277, 20);
            this.comboBoxEditMandante.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(355, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Corriere";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(16, 9);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(48, 13);
            this.labelControl13.TabIndex = 0;
            this.labelControl13.Text = "Mandante";
            // 
            // gridControlExcelIngresso
            // 
            this.gridControlExcelIngresso.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlExcelIngresso.Location = new System.Drawing.Point(0, 1);
            this.gridControlExcelIngresso.MainView = this.gridViewExcelIngresso;
            this.gridControlExcelIngresso.Name = "gridControlExcelIngresso";
            this.gridControlExcelIngresso.Size = new System.Drawing.Size(1593, 331);
            this.gridControlExcelIngresso.TabIndex = 10;
            this.gridControlExcelIngresso.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewExcelIngresso});
            this.gridControlExcelIngresso.Visible = false;
            // 
            // gridViewExcelIngresso
            // 
            this.gridViewExcelIngresso.GridControl = this.gridControlExcelIngresso;
            this.gridViewExcelIngresso.Name = "gridViewExcelIngresso";
            this.gridViewExcelIngresso.OptionsBehavior.Editable = false;
            this.gridViewExcelIngresso.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewExcelIngresso.OptionsView.ShowGroupPanel = false;
            // 
            // labelControlTrascina
            // 
            this.labelControlTrascina.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelControlTrascina.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            this.labelControlTrascina.Appearance.Options.UseFont = true;
            this.labelControlTrascina.Location = new System.Drawing.Point(712, 289);
            this.labelControlTrascina.Name = "labelControlTrascina";
            this.labelControlTrascina.Size = new System.Drawing.Size(166, 25);
            this.labelControlTrascina.TabIndex = 9;
            this.labelControlTrascina.Text = "Trascina qui il file";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.Location = new System.Drawing.Point(0, 0);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(1593, 327);
            this.memoEdit1.TabIndex = 8;
            this.memoEdit1.Visible = false;
            // 
            // FormOrdini
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1593, 775);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.simpleButtonAnnullaOrdine);
            this.Controls.Add(this.simpleButtonInviaOrdine);
            this.Controls.Add(this.menuStrip1);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FormOrdini.IconOptions.Icon")));
            this.LookAndFeel.SkinName = "DevExpress Style";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.Name = "FormOrdini";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XCM - Comunicazione Ordini";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormOrdini_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlProdotti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vociOrdineBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProdotti)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviProdotti.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviDatiFatturazione.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSalvaNuoviDatiDestinazione.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            this.splitContainerControl1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            this.splitContainerControl1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditVettore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditMandante.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlExcelIngresso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewExcelIngresso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControlProdotti;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProdotti;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem impostazioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem storicoOrdiniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anagraficaArticoliToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anagraficaDestinatariToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anagraficaFatturazioneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personalizzazioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aiutoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connessioneRemotaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informazioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eMailToolStripMenuItem;
        private DevExpress.Utils.DefaultToolTipController defaultToolTipController1;
        private DevExpress.XtraEditors.CheckEdit checkEditSalvaNuoviProdotti;
        private DevExpress.XtraEditors.CheckEdit checkEditSalvaNuoviDatiFatturazione;
        private DevExpress.XtraEditors.CheckEdit checkEditSalvaNuoviDatiDestinazione;
        private DevExpress.XtraEditors.SimpleButton simpleButtonInviaOrdine;
        private DevExpress.XtraEditors.SimpleButton simpleButtonAnnullaOrdine;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraPdfViewer.PdfViewer pdfViewer1;
        private DevExpress.XtraEditors.LabelControl labelControlTrascina;
        private DevExpress.XtraGrid.GridControl gridControlExcelIngresso;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewExcelIngresso;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditMandante;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private System.Windows.Forms.BindingSource vociOrdineBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colDataOrdine;
        private DevExpress.XtraGrid.Columns.GridColumn colNomeDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colIndirizzoDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colCAPDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colCittaDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colProvDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colRagioneSocialeFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colPIVAFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colIndirizzoFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colCAPFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colCittaFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colProvFatturazione;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroOrdine;
        private DevExpress.XtraGrid.Columns.GridColumn colCodProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colDescrizioneProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantitaProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colSconto;
        private DevExpress.XtraGrid.Columns.GridColumn colImportoUnitario;
        private DevExpress.XtraGrid.Columns.GridColumn colImportoTotale;
        private DevExpress.XtraGrid.Columns.GridColumn colDataConsegnaRichiesta;
        private DevExpress.XtraGrid.Columns.GridColumn colNote;
        private DevExpress.XtraGrid.Columns.GridColumn colLotto;
        private DevExpress.XtraGrid.Columns.GridColumn colIVA;
        private DevExpress.XtraGrid.Columns.GridColumn colBarcode;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditVettore;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRegione;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConvertiInCSV;
    }
}

