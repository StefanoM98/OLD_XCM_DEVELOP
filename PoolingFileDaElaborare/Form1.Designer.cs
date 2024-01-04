
namespace PoolingFileDaElaborare
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
            this.simpleButtonTestaAutomazione = new DevExpress.XtraEditors.SimpleButton();
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.fileDaImportareBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPathCompleto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNomeFileEXT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPathFile = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDataInserimento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatoEsecuzione = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMsgStato = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.fileDaImportareBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonTestaAutomazione
            // 
            this.simpleButtonTestaAutomazione.Location = new System.Drawing.Point(12, 12);
            this.simpleButtonTestaAutomazione.Name = "simpleButtonTestaAutomazione";
            this.simpleButtonTestaAutomazione.Size = new System.Drawing.Size(121, 23);
            this.simpleButtonTestaAutomazione.TabIndex = 1;
            this.simpleButtonTestaAutomazione.Text = "Testa Automazione";
            this.simpleButtonTestaAutomazione.Click += new System.EventHandler(this.simpleButtonTestaAutomazione_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "XCM";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // fileDaImportareBindingSource
            // 
            this.fileDaImportareBindingSource.DataSource = typeof(PoolingFileDaElaborare.FileDaImportare);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.fileDaImportareBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(12, 41);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1097, 478);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPathCompleto,
            this.colNomeFileEXT,
            this.colPathFile,
            this.colTipo,
            this.colDataInserimento,
            this.colStatoEsecuzione,
            this.colMsgStato});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            // 
            // colPathCompleto
            // 
            this.colPathCompleto.FieldName = "PathCompleto";
            this.colPathCompleto.Name = "colPathCompleto";
            this.colPathCompleto.Width = 118;
            // 
            // colNomeFileEXT
            // 
            this.colNomeFileEXT.Caption = "Nome File";
            this.colNomeFileEXT.FieldName = "NomeFileEXT";
            this.colNomeFileEXT.Name = "colNomeFileEXT";
            this.colNomeFileEXT.OptionsColumn.ReadOnly = true;
            this.colNomeFileEXT.Visible = true;
            this.colNomeFileEXT.VisibleIndex = 2;
            this.colNomeFileEXT.Width = 158;
            // 
            // colPathFile
            // 
            this.colPathFile.Caption = "Mandante";
            this.colPathFile.FieldName = "PathFile";
            this.colPathFile.Name = "colPathFile";
            this.colPathFile.OptionsColumn.ReadOnly = true;
            this.colPathFile.Visible = true;
            this.colPathFile.VisibleIndex = 1;
            this.colPathFile.Width = 163;
            // 
            // colTipo
            // 
            this.colTipo.Caption = "Tipo";
            this.colTipo.FieldName = "Tipo";
            this.colTipo.Name = "colTipo";
            this.colTipo.OptionsColumn.ReadOnly = true;
            this.colTipo.Visible = true;
            this.colTipo.VisibleIndex = 3;
            this.colTipo.Width = 88;
            // 
            // colDataInserimento
            // 
            this.colDataInserimento.Caption = "Data Inserimento";
            this.colDataInserimento.FieldName = "DataInserimento";
            this.colDataInserimento.Name = "colDataInserimento";
            this.colDataInserimento.Visible = true;
            this.colDataInserimento.VisibleIndex = 0;
            this.colDataInserimento.Width = 133;
            // 
            // colStatoEsecuzione
            // 
            this.colStatoEsecuzione.FieldName = "StatoEsecuzione";
            this.colStatoEsecuzione.Name = "colStatoEsecuzione";
            this.colStatoEsecuzione.Width = 167;
            // 
            // colMsgStato
            // 
            this.colMsgStato.FieldName = "MsgStato";
            this.colMsgStato.Name = "colMsgStato";
            this.colMsgStato.Visible = true;
            this.colMsgStato.VisibleIndex = 4;
            this.colMsgStato.Width = 198;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(974, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(121, 23);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Test";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButtonTestaAutomazione_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 524);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButtonTestaAutomazione);
            this.Name = "Form1";
            this.Text = "Pooling File da importare su Gespe";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.fileDaImportareBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButtonTestaAutomazione;
        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.BindingSource fileDaImportareBindingSource;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colPathCompleto;
        private DevExpress.XtraGrid.Columns.GridColumn colNomeFileEXT;
        private DevExpress.XtraGrid.Columns.GridColumn colPathFile;
        private DevExpress.XtraGrid.Columns.GridColumn colTipo;
        private DevExpress.XtraGrid.Columns.GridColumn colDataInserimento;
        private DevExpress.XtraGrid.Columns.GridColumn colStatoEsecuzione;
        private DevExpress.XtraGrid.Columns.GridColumn colMsgStato;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}

