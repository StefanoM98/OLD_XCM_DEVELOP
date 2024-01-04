
namespace GreenPassValidator
{
    partial class GestioneControlloAccessi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestioneControlloAccessi));
            this.gridControlAccessi = new DevExpress.XtraGrid.GridControl();
            this.utenteAccessoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewAccessi = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFK_UTENTE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCOGNOME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDATA_EVENTO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNOME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORA_ACCESSO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORA_USCITA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORE_DI_LAVORO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORA_ACCESSO_LAVORATIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORA_USCITA_LAVORATIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORE_DI_LAVORO_NETTE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGiornoDellaSettimana = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStraordinario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPermessi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateEditAccessiDal = new DevExpress.XtraEditors.DateEdit();
            this.dateEditAccessiAl = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonEsportaInExcel = new DevExpress.XtraEditors.SimpleButton();
            this.checkEditOspiti = new DevExpress.XtraEditors.CheckEdit();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonAggiornaGriglia = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAccessi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utenteAccessoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAccessi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOspiti.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlAccessi
            // 
            this.gridControlAccessi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlAccessi.DataSource = this.utenteAccessoBindingSource;
            this.gridControlAccessi.Location = new System.Drawing.Point(10, 57);
            this.gridControlAccessi.MainView = this.gridViewAccessi;
            this.gridControlAccessi.Name = "gridControlAccessi";
            this.gridControlAccessi.Size = new System.Drawing.Size(1231, 449);
            this.gridControlAccessi.TabIndex = 0;
            this.gridControlAccessi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAccessi});
            // 
            // utenteAccessoBindingSource
            // 
            this.utenteAccessoBindingSource.DataSource = typeof(GreenPassValidator.AccessiCustom);
            // 
            // gridViewAccessi
            // 
            this.gridViewAccessi.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFK_UTENTE,
            this.colCOGNOME,
            this.colDATA_EVENTO,
            this.colNOME,
            this.colORA_ACCESSO,
            this.colORA_USCITA,
            this.colORE_DI_LAVORO,
            this.colORA_ACCESSO_LAVORATIVA,
            this.colORA_USCITA_LAVORATIVA,
            this.colORE_DI_LAVORO_NETTE,
            this.colGiornoDellaSettimana,
            this.colStraordinario,
            this.colPermessi});
            this.gridViewAccessi.GridControl = this.gridControlAccessi;
            this.gridViewAccessi.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "DATA_EVENTO", this.colDATA_EVENTO, "Giorni Lavorati {0:0.##}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ORE_DI_LAVORO_NETTE", this.colORE_DI_LAVORO_NETTE, "{0:0.##}", 0D),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Max, "COGNOME", null, "{0}", ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Permessi", this.colPermessi, "{0:0.00}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Straordinario", this.colStraordinario, "{0:0.00}")});
            this.gridViewAccessi.Name = "gridViewAccessi";
            this.gridViewAccessi.OptionsBehavior.Editable = false;
            this.gridViewAccessi.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewAccessi.OptionsView.ColumnAutoWidth = false;
            this.gridViewAccessi.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridViewAccessi_RowStyle);
            // 
            // colFK_UTENTE
            // 
            this.colFK_UTENTE.FieldName = "FK_UTENTE";
            this.colFK_UTENTE.Name = "colFK_UTENTE";
            this.colFK_UTENTE.Visible = true;
            this.colFK_UTENTE.VisibleIndex = 0;
            // 
            // colCOGNOME
            // 
            this.colCOGNOME.Caption = "Cognome";
            this.colCOGNOME.FieldName = "COGNOME";
            this.colCOGNOME.Name = "colCOGNOME";
            this.colCOGNOME.Visible = true;
            this.colCOGNOME.VisibleIndex = 4;
            // 
            // colDATA_EVENTO
            // 
            this.colDATA_EVENTO.Caption = "Data";
            this.colDATA_EVENTO.FieldName = "DATA_EVENTO";
            this.colDATA_EVENTO.Name = "colDATA_EVENTO";
            this.colDATA_EVENTO.Visible = true;
            this.colDATA_EVENTO.VisibleIndex = 6;
            // 
            // colNOME
            // 
            this.colNOME.Caption = "Nome";
            this.colNOME.FieldName = "NOME";
            this.colNOME.Name = "colNOME";
            this.colNOME.Visible = true;
            this.colNOME.VisibleIndex = 5;
            // 
            // colORA_ACCESSO
            // 
            this.colORA_ACCESSO.Caption = "Ora accesso eff.";
            this.colORA_ACCESSO.DisplayFormat.FormatString = "HH:mm";
            this.colORA_ACCESSO.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colORA_ACCESSO.FieldName = "ORA_ACCESSO";
            this.colORA_ACCESSO.Name = "colORA_ACCESSO";
            this.colORA_ACCESSO.Visible = true;
            this.colORA_ACCESSO.VisibleIndex = 1;
            this.colORA_ACCESSO.Width = 20;
            // 
            // colORA_USCITA
            // 
            this.colORA_USCITA.Caption = "Ora uscita eff.";
            this.colORA_USCITA.DisplayFormat.FormatString = "HH:mm";
            this.colORA_USCITA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colORA_USCITA.FieldName = "ORA_USCITA";
            this.colORA_USCITA.Name = "colORA_USCITA";
            this.colORA_USCITA.Visible = true;
            this.colORA_USCITA.VisibleIndex = 2;
            this.colORA_USCITA.Width = 20;
            // 
            // colORE_DI_LAVORO
            // 
            this.colORE_DI_LAVORO.Caption = "Ore lav.";
            this.colORE_DI_LAVORO.DisplayFormat.FormatString = "0.00";
            this.colORE_DI_LAVORO.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colORE_DI_LAVORO.FieldName = "ORE_DI_LAVORO";
            this.colORE_DI_LAVORO.Name = "colORE_DI_LAVORO";
            this.colORE_DI_LAVORO.Visible = true;
            this.colORE_DI_LAVORO.VisibleIndex = 3;
            this.colORE_DI_LAVORO.Width = 20;
            // 
            // colORA_ACCESSO_LAVORATIVA
            // 
            this.colORA_ACCESSO_LAVORATIVA.Caption = "Ora entrata lav.";
            this.colORA_ACCESSO_LAVORATIVA.DisplayFormat.FormatString = "HH:mm";
            this.colORA_ACCESSO_LAVORATIVA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colORA_ACCESSO_LAVORATIVA.FieldName = "ORA_ACCESSO_LAVORATIVA";
            this.colORA_ACCESSO_LAVORATIVA.Name = "colORA_ACCESSO_LAVORATIVA";
            this.colORA_ACCESSO_LAVORATIVA.Visible = true;
            this.colORA_ACCESSO_LAVORATIVA.VisibleIndex = 8;
            this.colORA_ACCESSO_LAVORATIVA.Width = 123;
            // 
            // colORA_USCITA_LAVORATIVA
            // 
            this.colORA_USCITA_LAVORATIVA.Caption = "Ora uscita lav.";
            this.colORA_USCITA_LAVORATIVA.DisplayFormat.FormatString = "HH:mm";
            this.colORA_USCITA_LAVORATIVA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colORA_USCITA_LAVORATIVA.FieldName = "ORA_USCITA_LAVORATIVA";
            this.colORA_USCITA_LAVORATIVA.Name = "colORA_USCITA_LAVORATIVA";
            this.colORA_USCITA_LAVORATIVA.Visible = true;
            this.colORA_USCITA_LAVORATIVA.VisibleIndex = 9;
            this.colORA_USCITA_LAVORATIVA.Width = 132;
            // 
            // colORE_DI_LAVORO_NETTE
            // 
            this.colORE_DI_LAVORO_NETTE.Caption = "Ore lav. nette";
            this.colORE_DI_LAVORO_NETTE.DisplayFormat.FormatString = "0.00";
            this.colORE_DI_LAVORO_NETTE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colORE_DI_LAVORO_NETTE.FieldName = "ORE_DI_LAVORO_NETTE";
            this.colORE_DI_LAVORO_NETTE.Name = "colORE_DI_LAVORO_NETTE";
            this.colORE_DI_LAVORO_NETTE.Visible = true;
            this.colORE_DI_LAVORO_NETTE.VisibleIndex = 10;
            this.colORE_DI_LAVORO_NETTE.Width = 125;
            // 
            // colGiornoDellaSettimana
            // 
            this.colGiornoDellaSettimana.Caption = "Giorno";
            this.colGiornoDellaSettimana.FieldName = "GiornoDellaSettimana";
            this.colGiornoDellaSettimana.Name = "colGiornoDellaSettimana";
            this.colGiornoDellaSettimana.Visible = true;
            this.colGiornoDellaSettimana.VisibleIndex = 7;
            this.colGiornoDellaSettimana.Width = 101;
            // 
            // colStraordinario
            // 
            this.colStraordinario.DisplayFormat.FormatString = "0.00";
            this.colStraordinario.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colStraordinario.FieldName = "Straordinario";
            this.colStraordinario.Name = "colStraordinario";
            this.colStraordinario.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Straordinario", "Tot. {0:0.00}", 0D)});
            this.colStraordinario.Visible = true;
            this.colStraordinario.VisibleIndex = 11;
            // 
            // colPermessi
            // 
            this.colPermessi.DisplayFormat.FormatString = "0.00";
            this.colPermessi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPermessi.FieldName = "Permessi";
            this.colPermessi.Name = "colPermessi";
            this.colPermessi.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Permessi", "Tot. {0:0.00}")});
            this.colPermessi.Visible = true;
            this.colPermessi.VisibleIndex = 12;
            // 
            // dateEditAccessiDal
            // 
            this.dateEditAccessiDal.EditValue = null;
            this.dateEditAccessiDal.Location = new System.Drawing.Point(153, 12);
            this.dateEditAccessiDal.Name = "dateEditAccessiDal";
            this.dateEditAccessiDal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            this.dateEditAccessiDal.Properties.Appearance.Options.UseFont = true;
            this.dateEditAccessiDal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiDal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiDal.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Fluent;
            this.dateEditAccessiDal.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.dateEditAccessiDal.Size = new System.Drawing.Size(190, 32);
            this.dateEditAccessiDal.TabIndex = 1;
            this.dateEditAccessiDal.EditValueChanged += new System.EventHandler(this.dateEditAccessiDal_EditValueChanged);
            // 
            // dateEditAccessiAl
            // 
            this.dateEditAccessiAl.EditValue = null;
            this.dateEditAccessiAl.Location = new System.Drawing.Point(402, 12);
            this.dateEditAccessiAl.Name = "dateEditAccessiAl";
            this.dateEditAccessiAl.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 16F);
            this.dateEditAccessiAl.Properties.Appearance.Options.UseFont = true;
            this.dateEditAccessiAl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiAl.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditAccessiAl.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Fluent;
            this.dateEditAccessiAl.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.dateEditAccessiAl.Size = new System.Drawing.Size(190, 32);
            this.dateEditAccessiAl.TabIndex = 1;
            this.dateEditAccessiAl.EditValueChanged += new System.EventHandler(this.dateEditAccessiAl_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(381, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(15, 23);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Al";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(10, 15);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(137, 23);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Filtra Accessi dal";
            // 
            // simpleButtonEsportaInExcel
            // 
            this.simpleButtonEsportaInExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButtonEsportaInExcel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonEsportaInExcel.ImageOptions.Image")));
            this.simpleButtonEsportaInExcel.Location = new System.Drawing.Point(1108, 566);
            this.simpleButtonEsportaInExcel.Name = "simpleButtonEsportaInExcel";
            this.simpleButtonEsportaInExcel.Size = new System.Drawing.Size(133, 41);
            this.simpleButtonEsportaInExcel.TabIndex = 3;
            this.simpleButtonEsportaInExcel.Text = "Esporta in Excel";
            this.simpleButtonEsportaInExcel.Click += new System.EventHandler(this.simpleButtonEsportaInExcel_Click);
            // 
            // checkEditOspiti
            // 
            this.checkEditOspiti.Location = new System.Drawing.Point(1079, 31);
            this.checkEditOspiti.Name = "checkEditOspiti";
            this.checkEditOspiti.Properties.Caption = "Visualizza accessi ospiti";
            this.checkEditOspiti.Size = new System.Drawing.Size(162, 19);
            this.checkEditOspiti.TabIndex = 4;
            this.checkEditOspiti.Visible = false;
            this.checkEditOspiti.CheckedChanged += new System.EventHandler(this.checkEditOspiti_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1166, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 538);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(205, 69);
            this.listBox1.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl1.Location = new System.Drawing.Point(12, 519);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Assenti odierni";
            // 
            // simpleButtonAggiornaGriglia
            // 
            this.simpleButtonAggiornaGriglia.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonAggiornaGriglia.ImageOptions.Image")));
            this.simpleButtonAggiornaGriglia.Location = new System.Drawing.Point(598, 10);
            this.simpleButtonAggiornaGriglia.Name = "simpleButtonAggiornaGriglia";
            this.simpleButtonAggiornaGriglia.Size = new System.Drawing.Size(39, 35);
            this.simpleButtonAggiornaGriglia.TabIndex = 3;
            this.simpleButtonAggiornaGriglia.Click += new System.EventHandler(this.simpleButtonAggiornaGriglia_Click);
            // 
            // GestioneControlloAccessi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 609);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkEditOspiti);
            this.Controls.Add(this.simpleButtonAggiornaGriglia);
            this.Controls.Add(this.simpleButtonEsportaInExcel);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.dateEditAccessiAl);
            this.Controls.Add(this.dateEditAccessiDal);
            this.Controls.Add(this.gridControlAccessi);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("GestioneControlloAccessi.IconOptions.Icon")));
            this.Name = "GestioneControlloAccessi";
            this.Text = "Consulta Accessi";
            this.Load += new System.EventHandler(this.GestioneControlloAccessi_Load);
            this.Shown += new System.EventHandler(this.GestioneControlloAccessi_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAccessi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utenteAccessoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAccessi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiDal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditAccessiAl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditOspiti.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlAccessi;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAccessi;
        private DevExpress.XtraEditors.DateEdit dateEditAccessiDal;
        private DevExpress.XtraEditors.DateEdit dateEditAccessiAl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButtonEsportaInExcel;
        private System.Windows.Forms.BindingSource utenteAccessoBindingSource;
        private DevExpress.XtraEditors.CheckEdit checkEditOspiti;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonAggiornaGriglia;
        private DevExpress.XtraGrid.Columns.GridColumn colFK_UTENTE;
        private DevExpress.XtraGrid.Columns.GridColumn colCOGNOME;
        private DevExpress.XtraGrid.Columns.GridColumn colDATA_EVENTO;
        private DevExpress.XtraGrid.Columns.GridColumn colNOME;
        private DevExpress.XtraGrid.Columns.GridColumn colORA_ACCESSO;
        private DevExpress.XtraGrid.Columns.GridColumn colORA_USCITA;
        private DevExpress.XtraGrid.Columns.GridColumn colORE_DI_LAVORO;
        private DevExpress.XtraGrid.Columns.GridColumn colORA_ACCESSO_LAVORATIVA;
        private DevExpress.XtraGrid.Columns.GridColumn colORA_USCITA_LAVORATIVA;
        private DevExpress.XtraGrid.Columns.GridColumn colORE_DI_LAVORO_NETTE;
        private DevExpress.XtraGrid.Columns.GridColumn colGiornoDellaSettimana;
        private DevExpress.XtraGrid.Columns.GridColumn colStraordinario;
        private DevExpress.XtraGrid.Columns.GridColumn colPermessi;
    }
}