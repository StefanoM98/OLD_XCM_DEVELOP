
namespace UnitexFSC
{
    partial class XcmForm
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.listBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldocNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldocDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltransportType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcarrierID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcarrierDes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButtonRiceviXCM = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.DataSource = this.listBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(233, 207);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ActiveFilterEnabled = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colid,
            this.coldocNumber,
            this.coldocDate,
            this.coltransportType,
            this.colcarrierID,
            this.colcarrierDes});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowPartialRedrawOnScrolling = false;
            this.gridView1.OptionsBehavior.AllowValidationErrors = false;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsSelection.InvertSelection = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
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
            // simpleButtonRiceviXCM
            // 
            this.simpleButtonRiceviXCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButtonRiceviXCM.Location = new System.Drawing.Point(12, 236);
            this.simpleButtonRiceviXCM.Name = "simpleButtonRiceviXCM";
            this.simpleButtonRiceviXCM.Size = new System.Drawing.Size(233, 27);
            this.simpleButtonRiceviXCM.TabIndex = 5;
            this.simpleButtonRiceviXCM.Text = "Importa XCM";
            this.simpleButtonRiceviXCM.Click += new System.EventHandler(this.simpleButtonRiceviXCM_Click);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Sharp Plus";
            // 
            // XcmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(257, 275);
            this.Controls.Add(this.simpleButtonRiceviXCM);
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = global::UnitexFSC.Properties.Resources.unitex_logo_small;
            this.MaximizeBox = false;
            this.Name = "XcmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XCM Healthcare";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colid;
        private DevExpress.XtraGrid.Columns.GridColumn coldocNumber;
        private DevExpress.XtraGrid.Columns.GridColumn coldocDate;
        private DevExpress.XtraGrid.Columns.GridColumn coltransportType;
        private DevExpress.XtraGrid.Columns.GridColumn colcarrierID;
        private DevExpress.XtraGrid.Columns.GridColumn colcarrierDes;
        private DevExpress.XtraEditors.SimpleButton simpleButtonRiceviXCM;
        private System.Windows.Forms.BindingSource listBindingSource;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}