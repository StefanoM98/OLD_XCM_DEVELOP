
namespace MovimentiMagazzinoFromGespe
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.dDTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewMovMag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colrowIdLink = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPallet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodMandante = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMandante = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoMovimentazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coluniq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumDDT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataDDT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCommittente = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDestinatario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNomeDestinatazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndirizzoDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProvDestinazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCorriere = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRifOrdine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNoteDDT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodiceProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescrizioneProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGruppoProdotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantita = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLotto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colScadenza = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSconto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImportoUnitario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPesoUnitario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConfezioniPerCollo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumeroColli = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCausale = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrdineGespe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCitta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRegione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTripGespe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShipGespe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTemperaturaTrasporto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAliquotaIva = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocNumGespe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCalcoloFatturazione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dateEditAccessiAl = new DevExpress.XtraEditors.DateEdit();
            this.dateEditAccessiDal = new DevExpress.XtraEditors.DateEdit();
            this.buttonAggiorna = new System.Windows.Forms.Button();
            this.simpleButtonEsportaXslx = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonEsportaTutti = new DevExpress.XtraEditors.SimpleButton();
            this.buttonOggi = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonTuttiTraLeDate = new System.Windows.Forms.Button();
            this.buttonResetLayout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDTBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMovMag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.DataSource = this.dDTBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(12, 53);
            this.gridControl1.MainView = this.gridViewMovMag;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1451, 548);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMovMag});
            // 
            // dDTBindingSource
            // 
            this.dDTBindingSource.DataSource = typeof(MovimentiMagazzinoFromGespe.DDT);
            // 
            // gridViewMovMag
            // 
            this.gridViewMovMag.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colrowIdLink,
            this.colPallet,
            this.colCodMandante,
            this.colMandante,
            this.colTipoMovimentazione,
            this.coluniq,
            this.colNumDDT,
            this.colDataDDT,
            this.colCommittente,
            this.colDestinatario,
            this.colNomeDestinatazione,
            this.colIndirizzoDestinazione,
            this.colProvDestinazione,
            this.colCorriere,
            this.colRifOrdine,
            this.colNoteDDT,
            this.colCodiceProdotto,
            this.colDescrizioneProdotto,
            this.colGruppoProdotto,
            this.colQuantita,
            this.colLotto,
            this.colScadenza,
            this.colSconto,
            this.colImportoUnitario,
            this.colPesoUnitario,
            this.colConfezioniPerCollo,
            this.colNumeroColli,
            this.colCausale,
            this.colOrdineGespe,
            this.colCitta,
            this.colRegione,
            this.colTripGespe,
            this.colShipGespe,
            this.colTemperaturaTrasporto,
            this.colAliquotaIva,
            this.colDocNumGespe,
            this.colCalcoloFatturazione});
            this.gridViewMovMag.GridControl = this.gridControl1;
            this.gridViewMovMag.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Quantita", null, "Pezzi {0:0.##}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0})),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "NumeroColli", null, "Colli {0:0.##}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0})),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "CodiceProdotto", null, "Referenze {0:0.##}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0})),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PesoUnitario", null, "Peso {0:0.##}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0})),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ConfezioniPerCollo", null, "Confezioni {0:0.##}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0})),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CalcoloFatturazione", null, "Da fatturare {0:c2}", new decimal(new int[] {
                            0,
                            0,
                            0,
                            0}))});
            this.gridViewMovMag.Name = "gridViewMovMag";
            this.gridViewMovMag.OptionsBehavior.Editable = false;
            this.gridViewMovMag.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMovMag.OptionsView.ColumnAutoWidth = false;
            this.gridViewMovMag.OptionsView.ShowAutoFilterRow = true;
            this.gridViewMovMag.OptionsView.ShowFooter = true;
            this.gridViewMovMag.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewMovMag_CustomUnboundColumnData);
            // 
            // colrowIdLink
            // 
            this.colrowIdLink.FieldName = "rowIdLink";
            this.colrowIdLink.Name = "colrowIdLink";
            this.colrowIdLink.Visible = true;
            this.colrowIdLink.VisibleIndex = 1;
            // 
            // colPallet
            // 
            this.colPallet.FieldName = "Pallet";
            this.colPallet.Name = "colPallet";
            this.colPallet.Visible = true;
            this.colPallet.VisibleIndex = 29;
            // 
            // colCodMandante
            // 
            this.colCodMandante.FieldName = "CodMandante";
            this.colCodMandante.Name = "colCodMandante";
            this.colCodMandante.Visible = true;
            this.colCodMandante.VisibleIndex = 6;
            // 
            // colMandante
            // 
            this.colMandante.FieldName = "Mandante";
            this.colMandante.Name = "colMandante";
            this.colMandante.Visible = true;
            this.colMandante.VisibleIndex = 7;
            // 
            // colTipoMovimentazione
            // 
            this.colTipoMovimentazione.FieldName = "TipoMovimentazione";
            this.colTipoMovimentazione.Name = "colTipoMovimentazione";
            this.colTipoMovimentazione.Visible = true;
            this.colTipoMovimentazione.VisibleIndex = 8;
            // 
            // coluniq
            // 
            this.coluniq.FieldName = "uniq";
            this.coluniq.Name = "coluniq";
            this.coluniq.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "uniq", "{0}")});
            this.coluniq.Visible = true;
            this.coluniq.VisibleIndex = 0;
            // 
            // colNumDDT
            // 
            this.colNumDDT.FieldName = "NumDDT";
            this.colNumDDT.Name = "colNumDDT";
            this.colNumDDT.Visible = true;
            this.colNumDDT.VisibleIndex = 4;
            // 
            // colDataDDT
            // 
            this.colDataDDT.FieldName = "DataDDT";
            this.colDataDDT.Name = "colDataDDT";
            this.colDataDDT.Visible = true;
            this.colDataDDT.VisibleIndex = 2;
            // 
            // colCommittente
            // 
            this.colCommittente.FieldName = "Committente";
            this.colCommittente.Name = "colCommittente";
            this.colCommittente.Visible = true;
            this.colCommittente.VisibleIndex = 9;
            // 
            // colDestinatario
            // 
            this.colDestinatario.FieldName = "Destinatario";
            this.colDestinatario.Name = "colDestinatario";
            this.colDestinatario.Visible = true;
            this.colDestinatario.VisibleIndex = 10;
            // 
            // colNomeDestinatazione
            // 
            this.colNomeDestinatazione.FieldName = "NomeDestinatazione";
            this.colNomeDestinatazione.Name = "colNomeDestinatazione";
            this.colNomeDestinatazione.Visible = true;
            this.colNomeDestinatazione.VisibleIndex = 11;
            // 
            // colIndirizzoDestinazione
            // 
            this.colIndirizzoDestinazione.FieldName = "IndirizzoDestinazione";
            this.colIndirizzoDestinazione.Name = "colIndirizzoDestinazione";
            this.colIndirizzoDestinazione.Visible = true;
            this.colIndirizzoDestinazione.VisibleIndex = 12;
            // 
            // colProvDestinazione
            // 
            this.colProvDestinazione.FieldName = "ProvDestinazione";
            this.colProvDestinazione.Name = "colProvDestinazione";
            this.colProvDestinazione.Visible = true;
            this.colProvDestinazione.VisibleIndex = 14;
            // 
            // colCorriere
            // 
            this.colCorriere.FieldName = "Corriere";
            this.colCorriere.Name = "colCorriere";
            this.colCorriere.Visible = true;
            this.colCorriere.VisibleIndex = 16;
            // 
            // colRifOrdine
            // 
            this.colRifOrdine.FieldName = "RifOrdine";
            this.colRifOrdine.Name = "colRifOrdine";
            this.colRifOrdine.Visible = true;
            this.colRifOrdine.VisibleIndex = 5;
            // 
            // colNoteDDT
            // 
            this.colNoteDDT.FieldName = "NoteDDT";
            this.colNoteDDT.Name = "colNoteDDT";
            this.colNoteDDT.Visible = true;
            this.colNoteDDT.VisibleIndex = 17;
            // 
            // colCodiceProdotto
            // 
            this.colCodiceProdotto.FieldName = "CodiceProdotto";
            this.colCodiceProdotto.Name = "colCodiceProdotto";
            this.colCodiceProdotto.Visible = true;
            this.colCodiceProdotto.VisibleIndex = 18;
            // 
            // colDescrizioneProdotto
            // 
            this.colDescrizioneProdotto.FieldName = "DescrizioneProdotto";
            this.colDescrizioneProdotto.Name = "colDescrizioneProdotto";
            this.colDescrizioneProdotto.Visible = true;
            this.colDescrizioneProdotto.VisibleIndex = 19;
            // 
            // colGruppoProdotto
            // 
            this.colGruppoProdotto.FieldName = "GruppoProdotto";
            this.colGruppoProdotto.Name = "colGruppoProdotto";
            this.colGruppoProdotto.Visible = true;
            this.colGruppoProdotto.VisibleIndex = 20;
            // 
            // colQuantita
            // 
            this.colQuantita.FieldName = "Quantita";
            this.colQuantita.Name = "colQuantita";
            this.colQuantita.Visible = true;
            this.colQuantita.VisibleIndex = 21;
            // 
            // colLotto
            // 
            this.colLotto.FieldName = "Lotto";
            this.colLotto.Name = "colLotto";
            this.colLotto.Visible = true;
            this.colLotto.VisibleIndex = 22;
            // 
            // colScadenza
            // 
            this.colScadenza.FieldName = "Scadenza";
            this.colScadenza.Name = "colScadenza";
            this.colScadenza.Visible = true;
            this.colScadenza.VisibleIndex = 23;
            // 
            // colSconto
            // 
            this.colSconto.FieldName = "Sconto";
            this.colSconto.Name = "colSconto";
            this.colSconto.Visible = true;
            this.colSconto.VisibleIndex = 24;
            // 
            // colImportoUnitario
            // 
            this.colImportoUnitario.FieldName = "ImportoUnitario";
            this.colImportoUnitario.Name = "colImportoUnitario";
            this.colImportoUnitario.Visible = true;
            this.colImportoUnitario.VisibleIndex = 25;
            // 
            // colPesoUnitario
            // 
            this.colPesoUnitario.FieldName = "PesoUnitario";
            this.colPesoUnitario.Name = "colPesoUnitario";
            this.colPesoUnitario.Visible = true;
            this.colPesoUnitario.VisibleIndex = 26;
            // 
            // colConfezioniPerCollo
            // 
            this.colConfezioniPerCollo.FieldName = "ConfezioniPerCollo";
            this.colConfezioniPerCollo.Name = "colConfezioniPerCollo";
            this.colConfezioniPerCollo.Visible = true;
            this.colConfezioniPerCollo.VisibleIndex = 27;
            // 
            // colNumeroColli
            // 
            this.colNumeroColli.FieldName = "NumeroColli";
            this.colNumeroColli.Name = "colNumeroColli";
            this.colNumeroColli.Visible = true;
            this.colNumeroColli.VisibleIndex = 28;
            // 
            // colCausale
            // 
            this.colCausale.FieldName = "Causale";
            this.colCausale.Name = "colCausale";
            this.colCausale.Visible = true;
            this.colCausale.VisibleIndex = 30;
            // 
            // colOrdineGespe
            // 
            this.colOrdineGespe.FieldName = "OrdineGespe";
            this.colOrdineGespe.Name = "colOrdineGespe";
            this.colOrdineGespe.Visible = true;
            this.colOrdineGespe.VisibleIndex = 31;
            // 
            // colCitta
            // 
            this.colCitta.FieldName = "Citta";
            this.colCitta.Name = "colCitta";
            this.colCitta.Visible = true;
            this.colCitta.VisibleIndex = 15;
            // 
            // colRegione
            // 
            this.colRegione.FieldName = "Regione";
            this.colRegione.Name = "colRegione";
            this.colRegione.Visible = true;
            this.colRegione.VisibleIndex = 13;
            // 
            // colTripGespe
            // 
            this.colTripGespe.FieldName = "TripGespe";
            this.colTripGespe.Name = "colTripGespe";
            this.colTripGespe.Visible = true;
            this.colTripGespe.VisibleIndex = 32;
            // 
            // colShipGespe
            // 
            this.colShipGespe.FieldName = "ShipGespe";
            this.colShipGespe.Name = "colShipGespe";
            this.colShipGespe.Visible = true;
            this.colShipGespe.VisibleIndex = 33;
            // 
            // colTemperaturaTrasporto
            // 
            this.colTemperaturaTrasporto.FieldName = "TemperaturaTrasporto";
            this.colTemperaturaTrasporto.Name = "colTemperaturaTrasporto";
            this.colTemperaturaTrasporto.Visible = true;
            this.colTemperaturaTrasporto.VisibleIndex = 34;
            // 
            // colAliquotaIva
            // 
            this.colAliquotaIva.FieldName = "AliquotaIva";
            this.colAliquotaIva.Name = "colAliquotaIva";
            this.colAliquotaIva.Visible = true;
            this.colAliquotaIva.VisibleIndex = 35;
            // 
            // colDocNumGespe
            // 
            this.colDocNumGespe.FieldName = "DocNumGespe";
            this.colDocNumGespe.Name = "colDocNumGespe";
            this.colDocNumGespe.Visible = true;
            this.colDocNumGespe.VisibleIndex = 36;
            // 
            // colCalcoloFatturazione
            // 
            this.colCalcoloFatturazione.DisplayFormat.FormatString = "c2";
            this.colCalcoloFatturazione.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCalcoloFatturazione.FieldName = "CalcoloFatturazione";
            this.colCalcoloFatturazione.Name = "colCalcoloFatturazione";
            this.colCalcoloFatturazione.OptionsColumn.ReadOnly = true;
            this.colCalcoloFatturazione.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CalcoloFatturazione", "TOT={0:c2}")});
            this.colCalcoloFatturazione.Visible = true;
            this.colCalcoloFatturazione.VisibleIndex = 3;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(11, 24);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 13);
            this.labelControl5.TabIndex = 12;
            this.labelControl5.Text = "Mandante";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(512, 24);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(9, 13);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "Al";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(343, 24);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(15, 13);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "Dal";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(65, 21);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(259, 20);
            this.comboBoxEdit1.TabIndex = 11;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // dateEditAccessiAl
            // 
            this.dateEditAccessiAl.EditValue = null;
            this.dateEditAccessiAl.Location = new System.Drawing.Point(533, 21);
            this.dateEditAccessiAl.Name = "dateEditAccessiAl";
            this.dateEditAccessiAl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiAl.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiAl.Size = new System.Drawing.Size(130, 20);
            this.dateEditAccessiAl.TabIndex = 9;
            this.dateEditAccessiAl.EditValueChanged += new System.EventHandler(this.dateEditAccessiAl_EditValueChanged);
            // 
            // dateEditAccessiDal
            // 
            this.dateEditAccessiDal.EditValue = null;
            this.dateEditAccessiDal.Location = new System.Drawing.Point(364, 21);
            this.dateEditAccessiDal.Name = "dateEditAccessiDal";
            this.dateEditAccessiDal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiDal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiDal.Size = new System.Drawing.Size(130, 20);
            this.dateEditAccessiDal.TabIndex = 10;
            this.dateEditAccessiDal.EditValueChanged += new System.EventHandler(this.dateEditAccessiDal_EditValueChanged);
            // 
            // buttonAggiorna
            // 
            this.buttonAggiorna.Location = new System.Drawing.Point(669, 19);
            this.buttonAggiorna.Name = "buttonAggiorna";
            this.buttonAggiorna.Size = new System.Drawing.Size(75, 23);
            this.buttonAggiorna.TabIndex = 15;
            this.buttonAggiorna.Text = "Aggiorna";
            this.buttonAggiorna.UseVisualStyleBackColor = true;
            this.buttonAggiorna.Click += new System.EventHandler(this.buttonAggiorna_Click);
            // 
            // simpleButtonEsportaXslx
            // 
            this.simpleButtonEsportaXslx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonEsportaXslx.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonEsportaXslx.Appearance.Options.UseFont = true;
            this.simpleButtonEsportaXslx.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEsportaXslx.ImageOptions.Image")));
            this.simpleButtonEsportaXslx.Location = new System.Drawing.Point(1196, 12);
            this.simpleButtonEsportaXslx.Name = "simpleButtonEsportaXslx";
            this.simpleButtonEsportaXslx.Size = new System.Drawing.Size(130, 35);
            this.simpleButtonEsportaXslx.TabIndex = 16;
            this.simpleButtonEsportaXslx.Text = "Esporta Griglia";
            this.simpleButtonEsportaXslx.Click += new System.EventHandler(this.simpleButtonEsportaXslx_Click);
            // 
            // simpleButtonEsportaTutti
            // 
            this.simpleButtonEsportaTutti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonEsportaTutti.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButtonEsportaTutti.Appearance.Options.UseFont = true;
            this.simpleButtonEsportaTutti.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEsportaTutti.ImageOptions.Image")));
            this.simpleButtonEsportaTutti.Location = new System.Drawing.Point(1332, 12);
            this.simpleButtonEsportaTutti.Name = "simpleButtonEsportaTutti";
            this.simpleButtonEsportaTutti.Size = new System.Drawing.Size(130, 35);
            this.simpleButtonEsportaTutti.TabIndex = 17;
            this.simpleButtonEsportaTutti.Text = "Esporta Tutti";
            this.simpleButtonEsportaTutti.Click += new System.EventHandler(this.simpleButtonEsportaTutti_Click);
            // 
            // buttonOggi
            // 
            this.buttonOggi.Location = new System.Drawing.Point(874, 18);
            this.buttonOggi.Name = "buttonOggi";
            this.buttonOggi.Size = new System.Drawing.Size(75, 23);
            this.buttonOggi.TabIndex = 15;
            this.buttonOggi.Text = "Oggi";
            this.buttonOggi.UseVisualStyleBackColor = true;
            this.buttonOggi.Click += new System.EventHandler(this.buttonOggi_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(955, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Tutti Oggi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonTuttiTraLeDate
            // 
            this.buttonTuttiTraLeDate.Location = new System.Drawing.Point(750, 19);
            this.buttonTuttiTraLeDate.Name = "buttonTuttiTraLeDate";
            this.buttonTuttiTraLeDate.Size = new System.Drawing.Size(100, 22);
            this.buttonTuttiTraLeDate.TabIndex = 15;
            this.buttonTuttiTraLeDate.Text = "Tutti tra le date";
            this.buttonTuttiTraLeDate.UseVisualStyleBackColor = true;
            this.buttonTuttiTraLeDate.Click += new System.EventHandler(this.buttonTuttiTraLeDate_Click);
            // 
            // buttonResetLayout
            // 
            this.buttonResetLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResetLayout.Location = new System.Drawing.Point(1357, 619);
            this.buttonResetLayout.Name = "buttonResetLayout";
            this.buttonResetLayout.Size = new System.Drawing.Size(106, 19);
            this.buttonResetLayout.TabIndex = 15;
            this.buttonResetLayout.Text = "Resetta Layout";
            this.buttonResetLayout.UseVisualStyleBackColor = true;
            this.buttonResetLayout.Click += new System.EventHandler(this.buttonResetLayout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1475, 640);
            this.Controls.Add(this.simpleButtonEsportaTutti);
            this.Controls.Add(this.simpleButtonEsportaXslx);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTuttiTraLeDate);
            this.Controls.Add(this.buttonResetLayout);
            this.Controls.Add(this.buttonOggi);
            this.Controls.Add(this.buttonAggiorna);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.comboBoxEdit1);
            this.Controls.Add(this.dateEditAccessiAl);
            this.Controls.Add(this.dateEditAccessiDal);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consuntivo DDT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDTBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMovMag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMovMag;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.DateEdit dateEditAccessiAl;
        private DevExpress.XtraEditors.DateEdit dateEditAccessiDal;
        private System.Windows.Forms.BindingSource dDTBindingSource;
        private System.Windows.Forms.Button buttonAggiorna;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaXslx;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaTutti;
        private System.Windows.Forms.Button buttonOggi;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonTuttiTraLeDate;
        private System.Windows.Forms.Button buttonResetLayout;
        private DevExpress.XtraGrid.Columns.GridColumn colrowIdLink;
        private DevExpress.XtraGrid.Columns.GridColumn colPallet;
        private DevExpress.XtraGrid.Columns.GridColumn colCodMandante;
        private DevExpress.XtraGrid.Columns.GridColumn colMandante;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoMovimentazione;
        private DevExpress.XtraGrid.Columns.GridColumn coluniq;
        private DevExpress.XtraGrid.Columns.GridColumn colNumDDT;
        private DevExpress.XtraGrid.Columns.GridColumn colDataDDT;
        private DevExpress.XtraGrid.Columns.GridColumn colCommittente;
        private DevExpress.XtraGrid.Columns.GridColumn colDestinatario;
        private DevExpress.XtraGrid.Columns.GridColumn colNomeDestinatazione;
        private DevExpress.XtraGrid.Columns.GridColumn colIndirizzoDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colProvDestinazione;
        private DevExpress.XtraGrid.Columns.GridColumn colCorriere;
        private DevExpress.XtraGrid.Columns.GridColumn colRifOrdine;
        private DevExpress.XtraGrid.Columns.GridColumn colNoteDDT;
        private DevExpress.XtraGrid.Columns.GridColumn colCodiceProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colDescrizioneProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colGruppoProdotto;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantita;
        private DevExpress.XtraGrid.Columns.GridColumn colLotto;
        private DevExpress.XtraGrid.Columns.GridColumn colScadenza;
        private DevExpress.XtraGrid.Columns.GridColumn colSconto;
        private DevExpress.XtraGrid.Columns.GridColumn colImportoUnitario;
        private DevExpress.XtraGrid.Columns.GridColumn colPesoUnitario;
        private DevExpress.XtraGrid.Columns.GridColumn colConfezioniPerCollo;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroColli;
        private DevExpress.XtraGrid.Columns.GridColumn colCausale;
        private DevExpress.XtraGrid.Columns.GridColumn colOrdineGespe;
        private DevExpress.XtraGrid.Columns.GridColumn colCitta;
        private DevExpress.XtraGrid.Columns.GridColumn colRegione;
        private DevExpress.XtraGrid.Columns.GridColumn colTripGespe;
        private DevExpress.XtraGrid.Columns.GridColumn colShipGespe;
        private DevExpress.XtraGrid.Columns.GridColumn colTemperaturaTrasporto;
        private DevExpress.XtraGrid.Columns.GridColumn colAliquotaIva;
        private DevExpress.XtraGrid.Columns.GridColumn colDocNumGespe;
        private DevExpress.XtraGrid.Columns.GridColumn colCalcoloFatturazione;
    }
}

