namespace UnitexFSC
{
    partial class GLS
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
            this.simpleButtonGls = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlGls = new DevExpress.XtraGrid.GridControl();
            this.unitexTripBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.glsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldocNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldocDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltransportType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcarrierID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcarrierDes = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlGls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitexTripBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonGls
            // 
            this.simpleButtonGls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonGls.Location = new System.Drawing.Point(8, 232);
            this.simpleButtonGls.Name = "simpleButtonGls";
            this.simpleButtonGls.Size = new System.Drawing.Size(233, 27);
            this.simpleButtonGls.TabIndex = 7;
            this.simpleButtonGls.Text = "Esporta";
            this.simpleButtonGls.Click += new System.EventHandler(this.simpleButtonGls_Click);
            // 
            // gridControlGls
            // 
            this.gridControlGls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlGls.DataSource = this.unitexTripBindingSource;
            this.gridControlGls.Location = new System.Drawing.Point(8, 11);
            this.gridControlGls.MainView = this.glsGridView;
            this.gridControlGls.Name = "gridControlGls";
            this.gridControlGls.Size = new System.Drawing.Size(225, 204);
            this.gridControlGls.TabIndex = 6;
            this.gridControlGls.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.glsGridView});
            // 
            // unitexTripBindingSource
            // 
            this.unitexTripBindingSource.DataSource = typeof(UnitexFSC.Code.APIs.TmsTripListTrip);
            // 
            // glsGridView
            // 
            this.glsGridView.ActiveFilterEnabled = false;
            this.glsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colid,
            this.coldocNumber,
            this.coldocDate,
            this.coltransportType,
            this.colcarrierID,
            this.colcarrierDes});
            this.glsGridView.GridControl = this.gridControlGls;
            this.glsGridView.Name = "glsGridView";
            this.glsGridView.OptionsBehavior.AllowPartialRedrawOnScrolling = false;
            this.glsGridView.OptionsBehavior.AllowValidationErrors = false;
            this.glsGridView.OptionsBehavior.Editable = false;
            this.glsGridView.OptionsCustomization.AllowColumnMoving = false;
            this.glsGridView.OptionsCustomization.AllowColumnResizing = false;
            this.glsGridView.OptionsCustomization.AllowFilter = false;
            this.glsGridView.OptionsCustomization.AllowGroup = false;
            this.glsGridView.OptionsCustomization.AllowQuickHideColumns = false;
            this.glsGridView.OptionsCustomization.AllowSort = false;
            this.glsGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.glsGridView.OptionsSelection.InvertSelection = true;
            this.glsGridView.OptionsView.ShowAutoFilterRow = true;
            this.glsGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colid
            // 
            this.colid.FieldName = "id";
            this.colid.Name = "colid";
            // 
            // coldocNumber
            // 
            this.coldocNumber.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coldocNumber.AppearanceCell.Options.UseFont = true;
            this.coldocNumber.Caption = "Numero Viaggio";
            this.coldocNumber.FieldName = "docNumber";
            this.coldocNumber.Name = "coldocNumber";
            this.coldocNumber.Visible = true;
            this.coldocNumber.VisibleIndex = 0;
            // 
            // coldocDate
            // 
            this.coldocDate.Caption = "Data Documento";
            this.coldocDate.FieldName = "docDate";
            this.coldocDate.Name = "coldocDate";
            this.coldocDate.Visible = true;
            this.coldocDate.VisibleIndex = 1;
            // 
            // coltransportType
            // 
            this.coltransportType.FieldName = "transportType";
            this.coltransportType.Name = "coltransportType";
            // 
            // colcarrierID
            // 
            this.colcarrierID.FieldName = "carrierID";
            this.colcarrierID.Name = "colcarrierID";
            // 
            // colcarrierDes
            // 
            this.colcarrierDes.FieldName = "carrierDes";
            this.colcarrierDes.Name = "colcarrierDes";
            // 
            // GLS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 269);
            this.Controls.Add(this.simpleButtonGls);
            this.Controls.Add(this.gridControlGls);
            this.Name = "GLS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GLS";
            ((System.ComponentModel.ISupportInitialize)(this.gridControlGls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitexTripBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonGls;
        private DevExpress.XtraGrid.GridControl gridControlGls;
        private DevExpress.XtraGrid.Views.Grid.GridView glsGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colid;
        private DevExpress.XtraGrid.Columns.GridColumn coldocNumber;
        private DevExpress.XtraGrid.Columns.GridColumn coldocDate;
        private DevExpress.XtraGrid.Columns.GridColumn coltransportType;
        private DevExpress.XtraGrid.Columns.GridColumn colcarrierID;
        private DevExpress.XtraGrid.Columns.GridColumn colcarrierDes;
        private System.Windows.Forms.BindingSource unitexTripBindingSource;
    }
}