using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.LookAndFeel.DXSkinColors;

namespace GreenPassValidator
{
    public partial class GestioneControlloAccessi : XtraForm
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endtDate = DateTime.Now;
        List<AccessiCustom> DbAccessi = new List<AccessiCustom>();
        List<Anagrafica> AnagraficaAziendale = new List<Anagrafica>();
        string modelloPresenze = "MODELLO_PRESENZE.xlsx";

        public GestioneControlloAccessi()
        {
            InitializeComponent();

            var now = DateTime.Now;
            var dinM = DateTime.DaysInMonth(now.Year, now.Month);
            startDate = new DateTime(now.Year, now.Month, 1);
            endtDate = new DateTime(now.Year, now.Month, dinM);

            dateEditAccessiDal.DateTime = startDate;
            dateEditAccessiAl.DateTime = endtDate;
        }

        private void GestioneControlloAccessi_Load(object sender, EventArgs e)
        {
#if DEBUG 
            button1.Visible = true;
#endif
            PopolaAnagrafica();
            CaricaAccessiDipendenti();
        }

        private void PopolaAnagrafica()
        {
            var db = new ControlloAccessiXCMEntities();
            AnagraficaAziendale.AddRange(db.Anagrafica);
        }

        private void CaricaAccessiDipendenti()
        {
            try
            {
                gridViewAccessi.BeginUpdate();
                listBox1.BeginUpdate();
                DbAccessi.Clear();
                listBox1.Items.Clear();
                var db = new ControlloAccessiXCMEntities();
                var logAccessi = db.Accessi.Where(x => x.DATA_VERIFICA >= startDate && x.DATA_VERIFICA <= endtDate && !x.IS_OSPITE).OrderBy(x => x.DATA_VERIFICA).ToList();

                foreach (var loga in logAccessi)
                {


                    var rifAna = AnagraficaAziendale.First(x => x.ID_ANAGRAFICA == loga.FK_UTENTE);
                    //if (rifAna.COGNOME != "RENDINA")
                    //{
                    //    continue;
                    //}

                    double? oreLavoro = CalcolaOreLavoro(loga.ORA_INGRESSO, loga.ORA_USCITA);
                    var oAl = ArrotondaAl15(true, loga.ORA_INGRESSO).Value;
                    var oUl = ArrotondaAl15(false, loga.ORA_USCITA);
                    var dettAcc = new AccessiCustom
                    {
                        FK_UTENTE = rifAna.ID_ANAGRAFICA,
                        COGNOME = rifAna.COGNOME,
                        DATA_EVENTO = loga.DATA_VERIFICA,
                        NOME = rifAna.NOME,
                        ORA_ACCESSO = loga.ORA_INGRESSO,
                        ORA_USCITA = loga.ORA_USCITA,
                        ORE_DI_LAVORO = oreLavoro,
                        ORA_ACCESSO_LAVORATIVA = oAl,
                        ORA_USCITA_LAVORATIVA = oUl,
                        ORE_DI_LAVORO_NETTE = CalcolaOreLavoro(oAl, oUl),
                        GiornoDellaSettimana = loga.DATA_VERIFICA.DayOfWeek.ToString(),

                    };

                    if (dettAcc.DATA_EVENTO.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (dettAcc.ORE_DI_LAVORO_NETTE > 7)
                        {
                            dettAcc.Straordinario = dettAcc.ORE_DI_LAVORO_NETTE - 7;
                        }
                        else if (dettAcc.ORE_DI_LAVORO_NETTE < 7)
                        {
                            dettAcc.Permessi = (dettAcc.ORE_DI_LAVORO_NETTE - 7) * -1;
                        }
                        else
                        {
                            //in orario
                        }

                    }
                    else if (dettAcc.DATA_EVENTO.DayOfWeek == DayOfWeek.Saturday || dettAcc.DATA_EVENTO.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dettAcc.Straordinario = dettAcc.ORE_DI_LAVORO_NETTE;
                    }
                    else
                    {
                        if (dettAcc.ORE_DI_LAVORO_NETTE > 8)
                        {
                            dettAcc.Straordinario = dettAcc.ORE_DI_LAVORO_NETTE - 8;
                        }
                        else if (dettAcc.ORE_DI_LAVORO_NETTE < 8)
                        {
                            dettAcc.Permessi = (dettAcc.ORE_DI_LAVORO_NETTE - 8) * -1;
                        }
                        else
                        {
                            //in orario
                        }
                    }
                    DbAccessi.Add(dettAcc);
                }

                utenteAccessoBindingSource.DataSource = DbAccessi;

                List<string> assenti = new List<string>();


                var accessiOdierni = DbAccessi.Where(x => x.DATA_EVENTO.Date == DateTime.Now.Date).ToList();

                foreach (var dipe in AnagraficaAziendale)
                {
                    if (accessiOdierni != null && accessiOdierni.Count() == 0) { break; }
                    if (dipe.ID_ANAGRAFICA == 2) continue;
                    else if (dipe.ID_ANAGRAFICA == 4) continue;
                    else if (dipe.ID_ANAGRAFICA == 8) continue;
                    else if (!dipe.ENABLED) continue;
                    var entrato = accessiOdierni.FirstOrDefault(x => x.FK_UTENTE == dipe.ID_ANAGRAFICA);
                    if (entrato == null)
                    {
                        assenti.Add(dipe.COGNOME);
                    }
                }
                foreach (var ass in assenti)
                {
                    listBox1.Items.Add(ass);
                }
            }
            finally
            {
                gridViewAccessi.EndUpdate();
                listBox1.EndUpdate();
            }

        }

        private DateTime? ArrotondaAl15(bool isAccesso, DateTime? oraEventoEU)
        {
            if (!oraEventoEU.HasValue) return null;

            if (isAccesso)
            {
                var min = oraEventoEU.Value.Minute;
                if (min == 0)
                {
                    return oraEventoEU;
                }
                else if (min <= 15)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 15, 0);
                }
                else if (min > 15 && min <= 30)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 30, 0);
                }
                else if (min > 30 && min <= 45)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 45, 0);
                }
                else if (min > 45 && min <= 59)
                {
                    var nd = oraEventoEU.Value.AddHours(1);
                    return new DateTime(nd.Year, nd.Month, nd.Day, nd.Hour, 0, 0);
                }
                else return oraEventoEU;
            }
            else
            {
                var min = oraEventoEU.Value.Minute;
                if (min == 0)
                {
                    return oraEventoEU;
                }
                else if (min < 15)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 0, 0);
                }
                else if (min >= 15 && min < 30)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 15, 0);
                }
                else if (min >= 30 && min < 45)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 30, 0);
                }
                else if (min >= 45 && min <= 59)
                {
                    return new DateTime(oraEventoEU.Value.Year, oraEventoEU.Value.Month, oraEventoEU.Value.Day, oraEventoEU.Value.Hour, 45, 0);
                }
                else return oraEventoEU;
            }
        }

        private double? CalcolaOreLavoro(DateTime oRA_INGRESSO, DateTime? oRA_USCITA)
        {
            if (oRA_USCITA != null)
            {
                var tsE = oRA_INGRESSO;
                var tsU = oRA_USCITA.Value;

                var oreLav = tsU - tsE;

                if (oreLav > TimeSpan.FromHours(4))
                {
                    oreLav = oreLav - TimeSpan.FromHours(1);
                }
                return oreLav.TotalMinutes / 60;
            }
            else
            {
                if (oRA_INGRESSO.DayOfWeek == DayOfWeek.Friday)
                {
                    return 7.0;
                }
                else
                {
                    return 8.0;
                }
            }

        }

        private void GestioneControlloAccessi_Shown(object sender, EventArgs e)
        {

            gridViewAccessi.BeginUpdate();
            gridViewAccessi.Columns.First(x => x.Name == colFK_UTENTE.Name).GroupIndex = 1;
            gridViewAccessi.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {



        }

        private void checkEditOspiti_CheckedChanged(object sender, EventArgs e)
        {
            gridViewAccessi.BeginDataUpdate();
            try
            {
                DbAccessi.Clear();
                if (checkEditOspiti.Checked)
                {
                    var db = new ControlloAccessiXCMEntities();
                    AnagraficaAziendale.AddRange(db.Anagrafica);
                    var logAccessi = db.Accessi.Where(x => x.DATA_VERIFICA.Day >= startDate.Day && x.DATA_VERIFICA.Month >= startDate.Month &&
                                                      x.DATA_VERIFICA.Day <= endtDate.Day && x.DATA_VERIFICA.Month <= endtDate.Month && x.IS_OSPITE).OrderBy(x => x.DATA_VERIFICA).ToList();

                    foreach (var loga in logAccessi)
                    {

                        var dettAcc = new AccessiCustom
                        {
                            FK_UTENTE = 2,
                            COGNOME = loga.OSPITE_COGNOME,
                            DATA_EVENTO = loga.DATA_VERIFICA,
                            NOME = loga.OSPITE_NOME,
                            ORA_ACCESSO = loga.ORA_INGRESSO

                        };
                        DbAccessi.Add(dettAcc);
                    }
                }
                else
                {
                    CaricaAccessiDipendenti();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                gridViewAccessi.EndDataUpdate();
            }

        }

        private void simpleButtonEsportaInExcel_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog();
            fd.DefaultExt = "xlsx";
            fd.FileName = $"Export accessi dal {startDate.ToString("ddMMyyyy")} al {endtDate.ToString("ddMMyyyyy")}.xlsx";
            fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var resp = fd.ShowDialog();
            try
            {
                if (resp == DialogResult.OK)
                {
                    gridControlAccessi.ExportToXlsx(fd.FileName);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show($"{ee.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void dateEditAccessiDal_EditValueChanged(object sender, EventArgs e)
        {
            startDate = dateEditAccessiDal.DateTime.Date;

        }

        private void simpleButtonAggiornaGriglia_Click(object sender, EventArgs e)
        {
            CaricaAccessiDipendenti();
        }

        private void dateEditAccessiAl_EditValueChanged(object sender, EventArgs e)
        {
            endtDate = dateEditAccessiAl.DateTime.Date + TimeSpan.FromHours(23) - TimeSpan.FromMinutes(1);

        }

        private void gridViewAccessi_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string festivo = View.GetRowCellDisplayText(e.RowHandle, colGiornoDellaSettimana);
                if (festivo.ToLower() == "saturday" || festivo.ToLower() == "sunday")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.HighPriority = true;
                }
            }
        }
    }
}
