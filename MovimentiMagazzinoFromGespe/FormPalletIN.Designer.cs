
namespace MovimentiMagazzinoFromGespe
{
	partial class FormPalletIN
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPalletIN));
			this.buttonMeseScorso = new System.Windows.Forms.Button();
			this.buttonMeseCorrente = new System.Windows.Forms.Button();
			this.buttonOggi = new System.Windows.Forms.Button();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.dateEditAccessiAl = new DevExpress.XtraEditors.DateEdit();
			this.dateEditAccessiDal = new DevExpress.XtraEditors.DateEdit();
			this.gridControlPalletIN = new DevExpress.XtraGrid.GridControl();
			this.pALLETINBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gridViewPalletIN = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.simpleButtonEsportaXslx = new DevExpress.XtraEditors.SimpleButton();
			this.buttonAggiornaDati = new System.Windows.Forms.Button();
			this.colID_REGISTRAZIONE_PALLET = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colFK_ID_ANAGRAFICA_CLIENTE = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colFK_ID_OPERATORE = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPALLET_IN1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDATA_INSERIMENTO = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colANAGRAFICA_CLIENTI = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colANAGRAFICA_OPERATORI = new DevExpress.XtraGrid.Columns.GridColumn();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControlPalletIN)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pALLETINBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewPalletIN)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonMeseScorso
			// 
			this.buttonMeseScorso.Location = new System.Drawing.Point(170, 36);
			this.buttonMeseScorso.Name = "buttonMeseScorso";
			this.buttonMeseScorso.Size = new System.Drawing.Size(100, 22);
			this.buttonMeseScorso.TabIndex = 31;
			this.buttonMeseScorso.Text = "Mese scorso";
			this.buttonMeseScorso.UseVisualStyleBackColor = true;
			this.buttonMeseScorso.Click += new System.EventHandler(this.buttonMeseScorso_Click);
			// 
			// buttonMeseCorrente
			// 
			this.buttonMeseCorrente.Location = new System.Drawing.Point(170, 11);
			this.buttonMeseCorrente.Name = "buttonMeseCorrente";
			this.buttonMeseCorrente.Size = new System.Drawing.Size(100, 22);
			this.buttonMeseCorrente.TabIndex = 32;
			this.buttonMeseCorrente.Text = "Mese corrente";
			this.buttonMeseCorrente.UseVisualStyleBackColor = true;
			this.buttonMeseCorrente.Click += new System.EventHandler(this.buttonMeseCorrente_Click);
			// 
			// buttonOggi
			// 
			this.buttonOggi.Location = new System.Drawing.Point(276, 11);
			this.buttonOggi.Name = "buttonOggi";
			this.buttonOggi.Size = new System.Drawing.Size(100, 22);
			this.buttonOggi.TabIndex = 34;
			this.buttonOggi.Text = "Oggi";
			this.buttonOggi.UseVisualStyleBackColor = true;
			this.buttonOggi.Click += new System.EventHandler(this.buttonOggi_Click);
			// 
			// labelControl4
			// 
			this.labelControl4.Location = new System.Drawing.Point(13, 41);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(9, 13);
			this.labelControl4.TabIndex = 29;
			this.labelControl4.Text = "Al";
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(13, 15);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(15, 13);
			this.labelControl3.TabIndex = 30;
			this.labelControl3.Text = "Dal";
			// 
			// dateEditAccessiAl
			// 
			this.dateEditAccessiAl.EditValue = null;
			this.dateEditAccessiAl.Location = new System.Drawing.Point(34, 38);
			this.dateEditAccessiAl.Name = "dateEditAccessiAl";
			this.dateEditAccessiAl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dateEditAccessiAl.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dateEditAccessiAl.Size = new System.Drawing.Size(130, 20);
			this.dateEditAccessiAl.TabIndex = 27;
			// 
			// dateEditAccessiDal
			// 
			this.dateEditAccessiDal.EditValue = null;
			this.dateEditAccessiDal.Location = new System.Drawing.Point(34, 12);
			this.dateEditAccessiDal.Name = "dateEditAccessiDal";
			this.dateEditAccessiDal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dateEditAccessiDal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dateEditAccessiDal.Size = new System.Drawing.Size(130, 20);
			this.dateEditAccessiDal.TabIndex = 28;
			// 
			// gridControlPalletIN
			// 
			this.gridControlPalletIN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridControlPalletIN.DataSource = this.pALLETINBindingSource;
			this.gridControlPalletIN.Location = new System.Drawing.Point(12, 84);
			this.gridControlPalletIN.MainView = this.gridViewPalletIN;
			this.gridControlPalletIN.Name = "gridControlPalletIN";
			this.gridControlPalletIN.Size = new System.Drawing.Size(1186, 468);
			this.gridControlPalletIN.TabIndex = 35;
			this.gridControlPalletIN.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPalletIN});
			// 
			// pALLETINBindingSource
			// 
			this.pALLETINBindingSource.DataSource = typeof(MovimentiMagazzinoFromGespe.PALLET_IN);
			// 
			// gridViewPalletIN
			// 
			this.gridViewPalletIN.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID_REGISTRAZIONE_PALLET,
            this.colFK_ID_ANAGRAFICA_CLIENTE,
            this.colANAGRAFICA_CLIENTI,
            this.colFK_ID_OPERATORE,
            this.colPALLET_IN1,
            this.colDATA_INSERIMENTO,
            this.colANAGRAFICA_OPERATORI});
			this.gridViewPalletIN.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
			this.gridViewPalletIN.GridControl = this.gridControlPalletIN;
			this.gridViewPalletIN.Name = "gridViewPalletIN";
			this.gridViewPalletIN.OptionsView.ColumnAutoWidth = false;
			this.gridViewPalletIN.OptionsView.ShowAutoFilterRow = true;
			this.gridViewPalletIN.OptionsView.ShowGroupPanel = false;
			// 
			// simpleButtonEsportaXslx
			// 
			this.simpleButtonEsportaXslx.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.simpleButtonEsportaXslx.Appearance.Options.UseFont = true;
			this.simpleButtonEsportaXslx.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEsportaXslx.ImageOptions.Image")));
			this.simpleButtonEsportaXslx.Location = new System.Drawing.Point(1056, 15);
			this.simpleButtonEsportaXslx.Name = "simpleButtonEsportaXslx";
			this.simpleButtonEsportaXslx.Size = new System.Drawing.Size(142, 47);
			this.simpleButtonEsportaXslx.TabIndex = 36;
			this.simpleButtonEsportaXslx.Text = "Esporta Griglia";
			this.simpleButtonEsportaXslx.Click += new System.EventHandler(this.simpleButtonEsportaXslx_Click);
			// 
			// buttonAggiornaDati
			// 
			this.buttonAggiornaDati.Location = new System.Drawing.Point(382, 12);
			this.buttonAggiornaDati.Name = "buttonAggiornaDati";
			this.buttonAggiornaDati.Size = new System.Drawing.Size(100, 22);
			this.buttonAggiornaDati.TabIndex = 34;
			this.buttonAggiornaDati.Text = "Aggiorna Dati";
			this.buttonAggiornaDati.UseVisualStyleBackColor = true;
			this.buttonAggiornaDati.Click += new System.EventHandler(this.buttonAggiornaDati_Click);
			// 
			// colID_REGISTRAZIONE_PALLET
			// 
			this.colID_REGISTRAZIONE_PALLET.Caption = "ID";
			this.colID_REGISTRAZIONE_PALLET.FieldName = "ID_REGISTRAZIONE_PALLET";
			this.colID_REGISTRAZIONE_PALLET.Name = "colID_REGISTRAZIONE_PALLET";
			this.colID_REGISTRAZIONE_PALLET.Visible = true;
			this.colID_REGISTRAZIONE_PALLET.VisibleIndex = 0;
			// 
			// colFK_ID_ANAGRAFICA_CLIENTE
			// 
			this.colFK_ID_ANAGRAFICA_CLIENTE.Caption = "ID CLIENTE";
			this.colFK_ID_ANAGRAFICA_CLIENTE.FieldName = "FK_ID_ANAGRAFICA_CLIENTE";
			this.colFK_ID_ANAGRAFICA_CLIENTE.Name = "colFK_ID_ANAGRAFICA_CLIENTE";
			this.colFK_ID_ANAGRAFICA_CLIENTE.Visible = true;
			this.colFK_ID_ANAGRAFICA_CLIENTE.VisibleIndex = 2;
			this.colFK_ID_ANAGRAFICA_CLIENTE.Width = 85;
			// 
			// colFK_ID_OPERATORE
			// 
			this.colFK_ID_OPERATORE.Caption = "ID OPERATORE";
			this.colFK_ID_OPERATORE.FieldName = "FK_ID_OPERATORE";
			this.colFK_ID_OPERATORE.Name = "colFK_ID_OPERATORE";
			this.colFK_ID_OPERATORE.Visible = true;
			this.colFK_ID_OPERATORE.VisibleIndex = 5;
			this.colFK_ID_OPERATORE.Width = 88;
			// 
			// colPALLET_IN1
			// 
			this.colPALLET_IN1.Caption = "PALLET IN";
			this.colPALLET_IN1.FieldName = "PALLET_IN1";
			this.colPALLET_IN1.Name = "colPALLET_IN1";
			this.colPALLET_IN1.Visible = true;
			this.colPALLET_IN1.VisibleIndex = 4;
			this.colPALLET_IN1.Width = 88;
			// 
			// colDATA_INSERIMENTO
			// 
			this.colDATA_INSERIMENTO.Caption = "DATA INSERIMENTO";
			this.colDATA_INSERIMENTO.FieldName = "DATA_INSERIMENTO";
			this.colDATA_INSERIMENTO.Name = "colDATA_INSERIMENTO";
			this.colDATA_INSERIMENTO.Visible = true;
			this.colDATA_INSERIMENTO.VisibleIndex = 1;
			this.colDATA_INSERIMENTO.Width = 145;
			// 
			// colANAGRAFICA_CLIENTI
			// 
			this.colANAGRAFICA_CLIENTI.Caption = "CLIENTE";
			this.colANAGRAFICA_CLIENTI.FieldName = "ANAGRAFICA_CLIENTI";
			this.colANAGRAFICA_CLIENTI.Name = "colANAGRAFICA_CLIENTI";
			this.colANAGRAFICA_CLIENTI.Visible = true;
			this.colANAGRAFICA_CLIENTI.VisibleIndex = 3;
			this.colANAGRAFICA_CLIENTI.Width = 184;
			// 
			// colANAGRAFICA_OPERATORI
			// 
			this.colANAGRAFICA_OPERATORI.FieldName = "ANAGRAFICA_OPERATORI";
			this.colANAGRAFICA_OPERATORI.Name = "colANAGRAFICA_OPERATORI";
			// 
			// FormPalletIN
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1210, 564);
			this.Controls.Add(this.simpleButtonEsportaXslx);
			this.Controls.Add(this.gridControlPalletIN);
			this.Controls.Add(this.buttonMeseScorso);
			this.Controls.Add(this.buttonMeseCorrente);
			this.Controls.Add(this.buttonAggiornaDati);
			this.Controls.Add(this.buttonOggi);
			this.Controls.Add(this.labelControl4);
			this.Controls.Add(this.labelControl3);
			this.Controls.Add(this.dateEditAccessiAl);
			this.Controls.Add(this.dateEditAccessiDal);
			this.Name = "FormPalletIN";
			this.Text = "FormPalletIN";
			this.Load += new System.EventHandler(this.FormPalletIN_Load);
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControlPalletIN)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pALLETINBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridViewPalletIN)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonMeseScorso;
		private System.Windows.Forms.Button buttonMeseCorrente;
		private System.Windows.Forms.Button buttonOggi;
		private DevExpress.XtraEditors.LabelControl labelControl4;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.DateEdit dateEditAccessiAl;
		private DevExpress.XtraEditors.DateEdit dateEditAccessiDal;
		private DevExpress.XtraGrid.GridControl gridControlPalletIN;
		private DevExpress.XtraGrid.Views.Grid.GridView gridViewPalletIN;
		private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaXslx;
		private System.Windows.Forms.BindingSource pALLETINBindingSource;
		private System.Windows.Forms.Button buttonAggiornaDati;
		private DevExpress.XtraGrid.Columns.GridColumn colID_REGISTRAZIONE_PALLET;
		private DevExpress.XtraGrid.Columns.GridColumn colFK_ID_ANAGRAFICA_CLIENTE;
		private DevExpress.XtraGrid.Columns.GridColumn colANAGRAFICA_CLIENTI;
		private DevExpress.XtraGrid.Columns.GridColumn colFK_ID_OPERATORE;
		private DevExpress.XtraGrid.Columns.GridColumn colPALLET_IN1;
		private DevExpress.XtraGrid.Columns.GridColumn colDATA_INSERIMENTO;
		private DevExpress.XtraGrid.Columns.GridColumn colANAGRAFICA_OPERATORI;
	}
}