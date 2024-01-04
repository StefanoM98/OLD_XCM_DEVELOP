using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitexFSC.Model;

namespace UnitexFSC
{
    public partial class XcmForm : DevExpress.XtraEditors.XtraForm
    {

        API api = new API();

        public XcmForm()
        {
            InitializeComponent();

            var trips = api.GetTrips();


            if(trips.Count() == 0)
            {
                if (XtraMessageBox.Show("Non sono stati trovati viaggi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    return;
                }

            }
            else
            {
                listBindingSource.DataSource = trips.OrderByDescending(x=> x.docDate);
            }

        }

        private void simpleButtonRiceviXCM_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var record = gridView1.GetFocusedRow() as TripXCM;
                if (record == null)
                {
                    if (XtraMessageBox.Show("Impossibile leggere il bordero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        return;
                    }
                }

                if (api.GetShips(record.docNumber))
                {
                    XtraMessageBox.Show(this, "Import terminato\r\nsaranno necessari fino a 5 minuti per vedere l'inport su GESPE", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        
                }
                else
                {
                    XtraMessageBox.Show(this, "Import fallito\r\ncontatta il reparto IT", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                }

                this.Dispose();
                this.Close();
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show(this, $"Import fallito, contattare il reparto IT\r\n{ee.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }
    }
}