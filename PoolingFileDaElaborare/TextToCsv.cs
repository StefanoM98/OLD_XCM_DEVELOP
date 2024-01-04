using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet;
using System.Drawing;
using System.IO;
using DevExpress.XtraExport.Csv;
using DevExpress.Export.Xl;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PoolingFileDaElaborare
{
    public static class TextToCsv
    {

        static string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XCM Healthcare", "WorkPath");

        public static List<StatisticheMagazzino> PopolaListaDaCSV(string filepath)
        {
            var allrighe = File.ReadAllLines(filepath).Skip(1).ToList();
            var resp = new List<StatisticheMagazzino>();
            foreach (var r in allrighe)
            {
                var dett = r.Split(';');

                if (!string.IsNullOrEmpty(dett[0]))
                {
                    var newStatM = new StatisticheMagazzino()
                    {
                        Sede = (!string.IsNullOrEmpty(dett[0])) ? decimal.Parse(dett[0]) : 0,
                        Data = (!string.IsNullOrEmpty(dett[1])) ? DateTime.Parse(dett[1]) : DateTime.MinValue,
                        IdUtente = (!string.IsNullOrEmpty(dett[2])) ? decimal.Parse(dett[2]) : 0,
                        NomeUtente = dett[3],
                        NrMissioni = (!string.IsNullOrEmpty(dett[4])) ? decimal.Parse(dett[4]) : 0,
                        TotColli = (!string.IsNullOrEmpty(dett[5])) ? decimal.Parse(dett[5]) : 0,
                        VolTotale = (!string.IsNullOrEmpty(dett[6])) ? decimal.Parse(dett[6]) : 0,
                        Accettazione = (!string.IsNullOrEmpty(dett[7])) ? decimal.Parse(dett[7]) : 0,
                        ColliAccettati = (!string.IsNullOrEmpty(dett[8])) ? decimal.Parse(dett[8]) : 0,
                        VolAccettato = (!string.IsNullOrEmpty(dett[9])) ? decimal.Parse(dett[9]) : 0,
                        Stoccaggio = (!string.IsNullOrEmpty(dett[10])) ? decimal.Parse(dett[10]) : 0,
                        ColliStoccati = (!string.IsNullOrEmpty(dett[11])) ? decimal.Parse(dett[11]) : 0,
                        VolStoccato = (!string.IsNullOrEmpty(dett[12])) ? decimal.Parse(dett[12]) : 0,
                        Spostamenti = (!string.IsNullOrEmpty(dett[13])) ? decimal.Parse(dett[13]) : 0,
                        ColliSpostati = (!string.IsNullOrEmpty(dett[14])) ? decimal.Parse(dett[14]) : 0,
                        VolSpostato = (!string.IsNullOrEmpty(dett[15])) ? decimal.Parse(dett[15]) : 0,
                        Abbassamenti = (!string.IsNullOrEmpty(dett[16])) ? decimal.Parse(dett[16]) : 0,
                        ColliAbbassati = (!string.IsNullOrEmpty(dett[17])) ? decimal.Parse(dett[17]) : 0,
                        VolAbbassamenti = (!string.IsNullOrEmpty(dett[18])) ? decimal.Parse(dett[18]) : 0,
                        PrelieviPallet = (!string.IsNullOrEmpty(dett[19])) ? decimal.Parse(dett[19]) : 0,
                        ColliPrelieviPallet = (!string.IsNullOrEmpty(dett[20])) ? decimal.Parse(dett[20]) : 0,
                        VolPrelieviPallet = (!string.IsNullOrEmpty(dett[21])) ? decimal.Parse(dett[21]) : 0,
                        PrelieviPicking = (!string.IsNullOrEmpty(dett[22])) ? decimal.Parse(dett[22]) : 0,
                        ColliPrelieviPicking = (!string.IsNullOrEmpty(dett[23])) ? decimal.Parse(dett[23]) : 0,
                        VolPrelieviPicking = (!string.IsNullOrEmpty(dett[24])) ? decimal.Parse(dett[24]) : 0,

                    };
                    resp.Add(newStatM);
                }
            }
            return resp;

        }
        public static List<VociOrdine> ConvertiTestoOrdineFarmaImpresa(string text, out Exception eex)
        {
            eex = null;
            try
            {
                return CreaOrdineDaTestoFarmaImpresa(text);
            }
            catch (Exception ee)
            {
                //InviaAvvisoInQualcheModo
                System.Windows.Forms.MessageBox.Show($"Errore in fase di elaborazione {ee.Message}", "Errore",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                eex = ee;
                return null;
            }
        }
        public static List<VociOrdine> ConvertiTestoOrdinePMS(string text, out Exception eex)
        {
            eex = null;
            try
            {
                return CreaOrdineDaPMS(text);

            }
            catch (Exception ee)
            {
                eex = ee;
                System.Windows.Forms.MessageBox.Show($"Errore in fase di elaborazione {ee.Message}", "Errore",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }

        public static string ripulisciStringa(string str, string replaceWith)
        {
            var rx = @"[^0-9a-zA-Z-:/._+\s]";
            var ret = Regex.Replace(str, rx, replaceWith);

            return ret;
        }

        internal static List<string> ProdottiDaIgnorareAPS = new List<string>()
        {
            ".","","boant","comuso"
        };
        public static string ConvertiXSLtoCSV(string filepath, string mandante, string vettore)
        {

            var jn = ripulisciStringa(Path.GetFileNameWithoutExtension(filepath), "_");
            var dirjn = Path.GetDirectoryName(filepath);
            jn = jn + DateTime.Now.ToString("ddMMyyyy_mmssffff") + ".csv";
            var dest = Path.Combine(dirjn, jn);
            if (File.Exists(dest)) File.Delete(dest);

            var workbook = new Workbook();

            workbook.LoadDocument(filepath);

            if (mandante.ToLower().Trim() == "falqui" && !string.IsNullOrEmpty(vettore))
            {
                try
                {
                    var wksheet = workbook.Worksheets[0];
                    var docRange = wksheet.GetUsedRange();
                    var totRighe = docRange.RowCount;
                    workbook.BeginUpdate();
                    for (int i = 2; i <= totRighe; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(wksheet.Cells[$"A{i}"].Value.ToString()))
                        {
                            wksheet.Cells[$"W{i}"].Value = vettore;
                            wksheet.Cells[$"Y{i}"].Value = RegioniItaliane.EstraiRegione(wksheet.Cells[$"L{i}"].Value.ToString());
                        }
                        else
                        {
                            wksheet.Rows[i].Delete();
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Errore inserimento vettore per falqui");
                }
                finally
                {
                    workbook.EndUpdate();
                }
            }
            else if (mandante.ToLower().Trim() == "aps")
            {
                try
                {
                    var wksheet = workbook.Worksheets[0];
                    var docRange = wksheet.GetUsedRange();
                    var totRighe = docRange.RowCount;
                    var isAPS = mandante.ToLower() != "aps";
                    workbook.BeginUpdate();
                    for (int i = 1; i <= totRighe; i++)
                    {
                        var daIgnorare = ProdottiDaIgnorareAPS.Any(x => x.StartsWith(wksheet.Cells[$"AD{i}"].Value.ToString().ToLower()));

                        if (i == 50)
                        {

                        }
                        if (!daIgnorare)
                        {
                            wksheet.Cells[$"A{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"A{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"A{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"B{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"B{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"B{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"C{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"C{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"C{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"D{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"D{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"D{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"E{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"E{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"E{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"F{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"F{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"F{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"G{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"G{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"G{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"H{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"H{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"H{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"I{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"I{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"I{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"J{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"J{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"J{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"K{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"K{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"K{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"L{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"L{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"L{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"M{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"M{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"M{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"N{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"N{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"N{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"O{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"O{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"O{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"P{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"P{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"P{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"Q{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"Q{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"Q{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"R{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"R{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"R{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"S{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"S{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"S{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"T{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"T{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"T{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"U{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"U{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"U{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"V{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"V{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"V{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"W{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"W{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"W{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"X{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"X{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"X{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"Y{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"Y{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"Y{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"Z{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"Z{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"Z{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AA{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AA{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AA{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AB{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AB{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AB{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AC{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AC{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AC{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AD{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AD{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AD{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AE{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AE{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AE{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AF{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AF{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AF{i}"].Value.ToString().Replace("\r", " ").Replace("\n", " ").Trim();
                            wksheet.Cells[$"AG{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AG{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AG{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AH{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AH{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AH{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AI{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AI{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AI{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AJ{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AJ{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AJ{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AK{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AK{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AK{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AL{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AL{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AL{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AM{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AM{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AM{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AN{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AN{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AN{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AO{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AO{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AO{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AP{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AP{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AP{i}"].Value.ToString().Trim();
                            wksheet.Cells[$"AV{i}"].Value = (isAPS) ? ripulisciStringa(wksheet.Cells[$"AV{i}"].Value.ToString().Trim(), "") : wksheet.Cells[$"AV{i}"].Value.ToString().Trim();

                            if (string.IsNullOrWhiteSpace(wksheet.Cells[$"AR{i}"].Value.ToString()))
                            {
                                wksheet.Cells[$"AR{i}"].Value = wksheet.Cells[$"L{i}"].Value;
                                wksheet.Cells[$"AS{i}"].Value = wksheet.Cells[$"M{i}"].Value;
                                wksheet.Cells[$"AT{i}"].Value = wksheet.Cells[$"O{i}"].Value;
                                wksheet.Cells[$"AV{i}"].Value = wksheet.Cells[$"Q{i}"].Value;
                                wksheet.Cells[$"AU{i}"].Value = wksheet.Cells[$"P{i}"].Value;
                                wksheet.Cells[$"AW{i}"].Value = wksheet.Cells[$"R{i}"].Value;

                            }
                            else
                            {
                                wksheet.Cells[$"AR{i}"].Value = wksheet.Cells[$"AR{i}"].Value;
                                wksheet.Cells[$"AS{i}"].Value = wksheet.Cells[$"AS{i}"].Value;
                                wksheet.Cells[$"AT{i}"].Value = wksheet.Cells[$"AT{i}"].Value;
                                wksheet.Cells[$"AV{i}"].Value = wksheet.Cells[$"AV{i}"].Value;
                                wksheet.Cells[$"AU{i}"].Value = wksheet.Cells[$"AU{i}"].Value;
                            }

                        }
                        else
                        {
                            wksheet.Rows[i - 1].Delete();
                            if (i < totRighe)
                            {
                                totRighe--;
                                i--;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Errore inserimento vettore per aps");
                }
                finally
                {
                    workbook.EndUpdate();
                }
            }

            workbook.Options.Export.Csv.ValueSeparator = ';';
            workbook.SaveDocument(dest, DocumentFormat.Csv);

            if (File.Exists(dest)) { return dest; } else { return ""; }

        }
        public static bool ConvertiXSLtoCSV(string filepath)
        {

            var jn = Path.GetFileNameWithoutExtension(filepath);
            var dirjn = Path.GetDirectoryName(filepath);
            jn = jn + DateTime.Now.ToString("ddMMyyyy_mmssffff") + ".csv";
            var dest = Path.Combine(dirjn, jn);
            if (File.Exists(dest)) File.Delete(dest);

            var workbook = new Workbook();

            workbook.LoadDocument(filepath);

            workbook.Options.Export.Csv.ValueSeparator = ';';
            workbook.SaveDocument(dest, DocumentFormat.Csv);

            if (File.Exists(dest)) { return true; } else { return false; }

        }
        public static string PopolaIlFileExcelConIlNuovoOrdineESalvaInCSV(List<VociOrdine> nuovoOrdine, string tmpExcelPath, string vettoreMigliore)
        {

            Workbook workbook = new Workbook();
            try
            {
                workbook.LoadDocument(tmpExcelPath, DocumentFormat.Xlsx);

                var wksheet = workbook.Worksheets[0];
                workbook.BeginUpdate();
                for (int i = 0; i < nuovoOrdine.Count; i++)
                {
                    var ord = nuovoOrdine[i];

                    wksheet.Cells[$"A{i + 2}"].Value = ord.NumeroOrdine;
                    wksheet.Cells[$"B{i + 2}"].Value = ord.DataOrdine.ToString("dd/MM/yyyy");
                    wksheet.Cells[$"C{i + 2}"].Value = ord.Barcode;
                    wksheet.Cells[$"D{i + 2}"].Value = ord.CodProdotto;
                    wksheet.Cells[$"E{i + 2}"].Value = ord.Lotto;
                    wksheet.Cells[$"F{i + 2}"].Value = ord.DescrizioneProdotto;
                    wksheet.Cells[$"G{i + 2}"].Value = ord.QuantitaProdotto;
                    wksheet.Cells[$"H{i + 2}"].Value = ord.IVA;
                    wksheet.Cells[$"I{i + 2}"].Value = ord.Sconto;
                    wksheet.Cells[$"J{i + 2}"].Value = ord.ImportoUnitario;
                    wksheet.Cells[$"K{i + 2}"].Value = ord.ImportoTotale;

                    wksheet.Cells[$"L{i + 2}"].Value = ord.RagioneSocialeFatturazione;
                    wksheet.Cells[$"M{i + 2}"].Value = ord.PIVAFatturazione;
                    wksheet.Cells[$"N{i + 2}"].Value = ord.IndirizzoFatturazione;
                    wksheet.Cells[$"O{i + 2}"].Value = ord.CAPFatturazione;
                    wksheet.Cells[$"P{i + 2}"].Value = ord.CittaFatturazione;
                    wksheet.Cells[$"Q{i + 2}"].Value = ord.ProvFatturazione;

                    wksheet.Cells[$"R{i + 2}"].Value = ord.NomeDestinazione;
                    wksheet.Cells[$"S{i + 2}"].Value = ord.IndirizzoDestinazione;
                    wksheet.Cells[$"T{i + 2}"].Value = ord.CAPDestinazione;
                    wksheet.Cells[$"U{i + 2}"].Value = ord.CittaDestinazione;
                    wksheet.Cells[$"V{i + 2}"].Value = ord.ProvDestinazione;

                    wksheet.Cells[$"W{i + 2}"].Value = RegioniItaliane.EstraiRegione(ord.ProvDestinazione);
                    wksheet.Cells[$"X{i + 2}"].Value = ord.Note;
                    wksheet.Cells[$"Y{i + 2}"].Value = vettoreMigliore;

                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
            finally
            {
                workbook.EndUpdate();
            }

            var dest = Path.ChangeExtension(tmpExcelPath, ".csv");
            if (File.Exists(dest)) File.Delete(dest);
            workbook.Options.Export.Csv.ValueSeparator = ';';
            workbook.SaveDocument(dest, DocumentFormat.Csv);
            File.Delete(tmpExcelPath);

            return dest;
        }

        private static List<VociOrdine> CreaOrdineDaPMS(string text)
        {
            var resp = new List<VociOrdine>();

            try
            {
                var SezioniSeparate = text.Split(new[] { "--------------------------------------------------------------------------------" }, StringSplitOptions.None).ToList();
                var startWBuono = SezioniSeparate[0].Contains("BUONO ORDINE");
                var startWEmpty = string.IsNullOrEmpty(SezioniSeparate[0]);
                int start = 0;
                int VersFile = 0;
                if (startWBuono)
                {
                    SezioniSeparate.RemoveAt(0);
                }
                if (startWEmpty)
                {
                    start = 1;
                }
                if (SezioniSeparate[0].EndsWith("+"))
                {
                    start = 2;
                    VersFile = 3;
                }

                if (VersFile == 3)
                {
                    return MappaFilePMSVers3(text);

                }
                else
                {
                    var datiAnagrafici = SezioniSeparate.ToArray()[start];
                    string[] dettagliOrdine = null;
                    if (!startWBuono && !startWEmpty && VersFile != 2)
                    {
                        dettagliOrdine = SezioniSeparate.ToArray()[start + 2].Split(new[] { "\r\n" }, StringSplitOptions.None);
                        VersFile = 1;
                    }
                    else
                    {
                        dettagliOrdine = SezioniSeparate.ToArray()[start + 1].Split(new[] { "\r\n" }, StringSplitOptions.None);
                        VersFile = 2;
                    }
                    var referenzeProdotti = SezioniSeparate.ToArray()[start + 3].Split(new[] { "\r\n" }, StringSplitOptions.None);

                    #region Anagrafica
                    var dettDatiAnagrafici = datiAnagrafici.Split(new[] { "\r\n" }, StringSplitOptions.None);

                    var nomeFatturazione = "";
                    var indirizzoFatturazione = "";
                    var capFatturazione = "";
                    var cittaFatturazione = "";
                    var provFatturazione = "";
                    var pivaFatturazione = "";
                    //if (VersFile == 1)
                    {
                        nomeFatturazione = dettDatiAnagrafici[1].Replace("SPETT.", "").Substring(30).Trim();
                        indirizzoFatturazione = dettDatiAnagrafici[2].Substring(30).Trim();
                        capFatturazione = dettDatiAnagrafici[3].Substring(34, 7).Trim();
                        cittaFatturazione = dettDatiAnagrafici[3].Substring(41, 31).Trim();
                        provFatturazione = dettDatiAnagrafici[3].Substring(72).Trim();
                    }
                    //else if(VersFile == 2)
                    //{
                    //    nomeFatturazione = dettDatiAnagrafici[1].Replace("SPETT.","").Substring(30).Trim();
                    //    indirizzoFatturazione = dettDatiAnagrafici[2].Substring(30).Trim();
                    //    capFatturazione = dettDatiAnagrafici[3].Substring(34, 7).Trim();
                    //    cittaFatturazione = dettDatiAnagrafici[3].Substring(41, 31).Trim();
                    //    provFatturazione = dettDatiAnagrafici[3].Substring(72).Trim();
                    //}


                    if (dettDatiAnagrafici[4].StartsWith("P.IVA"))
                    {
                        pivaFatturazione = dettDatiAnagrafici[4].Substring(6).Trim();
                    }
                    else if (dettDatiAnagrafici[4].StartsWith("Partita IVA"))
                    {
                        pivaFatturazione = dettDatiAnagrafici[4].Substring(14, 18).Trim();
                    }
                    var nomeDestinazione = dettDatiAnagrafici[1].Substring(0, 29).Trim();
                    var indirizzoDestinazione = dettDatiAnagrafici[2].Substring(0, 29).Trim();
                    var capDestinazione = dettDatiAnagrafici[3].Substring(0, 5).Trim();
                    var cittaDestinazioane = dettDatiAnagrafici[3].Substring(6, 21).Trim();
                    var provDestinazione = dettDatiAnagrafici[3].Substring(27, 2).Trim();
                    #endregion

                    #region Dettagli ordine
                    var numeroDoc = "";
                    if (VersFile == 1)
                    {
                        numeroDoc = dettagliOrdine[start + 1].Substring(59, 8).Trim();
                    }
                    else if (VersFile == 2)
                    {
                        numeroDoc = dettagliOrdine[start + 2].Substring(59, 8).Trim();
                    }
                    var dataOrdine = DateTime.MinValue;

                    if (VersFile == 1)
                    {
                        var dataS = dettagliOrdine[1].Substring(71).Trim();
                        dataOrdine = DateTime.Parse(dataS);
                    }
                    else if (VersFile == 2)
                    {
                        var dataS = dettagliOrdine[2].Substring(68).Trim();
                        dataOrdine = DateTime.Parse(dataS);
                    }
                    #endregion

                    #region Prodotti
                    for (int i = 0; i < referenzeProdotti.Length; i++)
                    {
                        var riga = referenzeProdotti[i];
                        if (riga.StartsWith(" "))
                        {
                            #region RilevaEConvertiCampi
                            if (string.IsNullOrWhiteSpace(riga))
                            {
                                continue;
                            }
                            decimal sco = -1;
                            try
                            {
                                var scS = riga.Substring(75, 5).Trim();
                                sco = decimal.Parse(scS);
                            }
                            catch { }
                            decimal pU = -1;
                            try
                            {
                                var puS = riga.Substring(66, 8).Trim();
                                pU = decimal.Parse(puS);
                            }
                            catch { }
                            decimal qP = -1;
                            try
                            {
                                var qPs = riga.Substring(37, 9).Trim();
                                qP = decimal.Parse(qPs);
                            }
                            catch { }
                            string cP = "ND";
                            try
                            {
                                cP = riga.Substring(47, 10).Trim();
                            }
                            catch { }
                            string dP = "ND";
                            try
                            {
                                dP = riga.Substring(5, 32).Trim();
                            }
                            catch { }
                            #endregion

                            var nO = new VociOrdine
                            {
                                DataOrdine = dataOrdine,
                                NumeroOrdine = numeroDoc,
                                DataConsegnaRichiesta = null,

                                NomeDestinazione = nomeDestinazione,
                                IndirizzoDestinazione = indirizzoDestinazione,
                                CAPDestinazione = capDestinazione,
                                CittaDestinazione = cittaDestinazioane,
                                ProvDestinazione = provDestinazione,

                                RagioneSocialeFatturazione = nomeFatturazione,
                                IndirizzoFatturazione = indirizzoFatturazione,
                                CAPFatturazione = capFatturazione,
                                CittaFatturazione = cittaFatturazione,
                                ProvFatturazione = provFatturazione,
                                PIVAFatturazione = pivaFatturazione,

                                Barcode = "",
                                CodProdotto = cP,
                                DescrizioneProdotto = dP,
                                Lotto = "",
                                QuantitaProdotto = qP,
                                ImportoUnitario = pU,
                                ImportoTotale = 0,
                                IVA = 0,
                                Note = "",
                                Sconto = sco,
                                Regione = RegioniItaliane.EstraiRegione(provDestinazione)
                            };
                            resp.Add(nO);
                        }
                        else if (riga.StartsWith("----"))
                        {
                            break;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }

            return resp;
        }

        public static List<VociOrdine> ConvertiXLSAPS(string daImportare, out Exception ex)
        {
            throw new NotImplementedException();
        }

        private static List<VociOrdine> MappaFilePMSVers3(string text)
        {
            var SezioniSeparate = text.Split(new[] { "--------------------------------------------------------------------------------" }, StringSplitOptions.None).ToList();
            var numOrd = SezioniSeparate[0].Replace("\r\n", "").Substring(12, 17).Trim();
            var dataRef = SezioniSeparate[0].Replace("\r\n", "").Substring(35, 10).Trim();
            List<VociOrdine> ordine = new List<VociOrdine>();
            #region Intestazione
            var intestazione = SezioniSeparate[1].Split(new[] { "\r\n" }, StringSplitOptions.None);
            var nomeDestinazione = intestazione[1].Substring(42, 35).Trim();
            var indDest = intestazione[2].Substring(42, 35).Trim();
            //var capComm = intestazione[3].Substring(1, 5).Trim();
            //var indComm = intestazione[2].Substring(1, 35).Trim();
            //var committente = intestazione[1].Substring(1, 35).Trim();
            #endregion

            #region RigheOrdine
            for (int i = 11; i <= intestazione.Count(); i++)
            {
                if (intestazione[i].StartsWith("+"))
                {
                    break;
                }
                bool haOmaggi = decimal.Parse(intestazione[i].Substring(63, 4).Trim()) > 0;
                var rOrd = new VociOrdine
                {
                    CodProdotto = intestazione[i].Substring(1, 9).Trim(),
                    DescrizioneProdotto = intestazione[i].Substring(11, 40).Trim(),
                    QuantitaProdotto = decimal.Parse(intestazione[i].Substring(57, 5).Trim()),
                    ImportoUnitario = decimal.Parse(intestazione[i].Substring(69, 9).Trim()),
                    DataOrdine = DateTime.Parse(dataRef),
                    NumeroOrdine = numOrd,
                    NomeDestinazione = nomeDestinazione,
                    IndirizzoDestinazione = indDest,
                };

                ordine.Add(rOrd);

                if (haOmaggi)
                {
                    var omgOrd = new VociOrdine
                    {
                        CodProdotto = intestazione[i].Substring(1, 9).Trim(),
                        DescrizioneProdotto = intestazione[i].Substring(11, 40).Trim(),
                        QuantitaProdotto = decimal.Parse(intestazione[i].Substring(63, 4).Trim()),
                        Sconto = 100,
                        ImportoUnitario = 0,
                        DataOrdine = DateTime.Parse(dataRef),
                        NumeroOrdine = numOrd,
                        NomeDestinazione = nomeDestinazione,
                        IndirizzoDestinazione = indDest,
                    };
                    ordine.Add(omgOrd);
                }
            }
            return ordine;
            #endregion

        }

        private static List<VociOrdine> CreaOrdineDaTestoFarmaImpresa(string text)
        {
            var resp = new List<VociOrdine>();
            List<string> listaTesto = new List<string>();
            listaTesto.AddRange(text.Split(new[] { "\n" }, StringSplitOptions.None));
            var datadoc = DateTime.Parse(listaTesto.FirstOrDefault(x => x.StartsWith("FATTURA")).Split(' ')[2]);
            var NumeroOrdine = listaTesto.FirstOrDefault(x => x.StartsWith("FATTURA")).Split(' ')[1];

            var NomeDestinazione = listaTesto[9];
            var IndirizzoDestinazione = listaTesto[14];
            string tt = listaTesto[17];

            #region Destinazione
            int VersFile = 0;
            bool StessoIndirizzoDestinazioneDiFatturazione = false;
            string CapDestinazione = "";
            string ProvDestinazione = "";
            string CittaDestinazione = "";

            #region Individua destinazione
            var tts = tt.Split(' ');
            if (tts.Count() >= 3)
            {
                CapDestinazione = tt.Split(' ')[0];
                ProvDestinazione = tt.Split(' ').Last();
                var capL = CapDestinazione.Length;
                var ll = (CapDestinazione.Length - (tt.Length - 2)) * -1;
                CittaDestinazione = tt.Substring(capL, ll).Trim();
            }
            else if (tts.Count() == 2)
            {
                CapDestinazione = tt.Split(' ')[0];
                ProvDestinazione = tt.Split(' ').Last();
                CittaDestinazione = ProvDestinazione;
            }
            else
            {
                VersFile = 1;

                StessoIndirizzoDestinazioneDiFatturazione = true;
            }
            #endregion
            #endregion


            var RagioneSocialeFatturazione = listaTesto[1];
            var IndirizzoFatturazione = listaTesto[2];

            #region Fatturazione

            string CapFatturazione = "";
            string ProvFatturazione = "";
            string CittaFatturazione = "";
            string tt2 = listaTesto[3];
            var tts2 = tt2.Split();

            if (tts2.Count() >= 3)
            {
                CapFatturazione = tts2[0];
                ProvFatturazione = tts2.Last();
                var capL2 = CapFatturazione.Length;

                var ll2 = (CapFatturazione.Length - (tt2.Length - 2)) * -1;
                CittaFatturazione = tt2.Substring(capL2, ll2).Trim();
            }
            else if (tts2.Count() == 2)
            {
                CapFatturazione = tts2[0];
                ProvFatturazione = tts2.Last();
                CittaFatturazione = ProvDestinazione;
            }
            else
            {
                throw new Exception("Indirizzo malformato");
            }

            if (StessoIndirizzoDestinazioneDiFatturazione)
            {
                CittaDestinazione = CittaFatturazione;
                CapDestinazione = CapFatturazione;
                ProvDestinazione = ProvFatturazione;
                IndirizzoDestinazione = IndirizzoFatturazione;
                NomeDestinazione = RagioneSocialeFatturazione;
            }

            string PivaFatturazione = "";
            if (VersFile == 0)
            {
                PivaFatturazione = listaTesto[11].Split(' ')[0];
            }
            else if (VersFile == 1)
            {
                PivaFatturazione = listaTesto[9].Split(' ')[0];
            }


            #endregion


            var preNote = listaTesto.IndexOf("codice e descrizione articoli colli conf. u.m. quantità prezzo % sconto importo c.iva");
            var Note = listaTesto[preNote + 1];
            if (!Note.StartsWith("***"))
            {
                Note = "";
            }
            int numeroReferenze = listaTesto.Where(x => x.ToLower().StartsWith("lotto:")).Count();
            List<int> indexLotti = new List<int>();
            bool piuForgli = false;
            if (listaTesto.Count > 69)
            {
                piuForgli = listaTesto[69] == "S E G U E";
            }

            for (int i = 0; i < listaTesto.Count; i++)
            {
                string[] gt2 = null;
                string descP = "";
                int qta = 0;
                decimal impU = 0;
                decimal ImpT = 0;
                decimal sco = 0;
                string codP = "";
                string lt = "";
                decimal iva = 0;

                if (listaTesto[i].ToLower().StartsWith("lotto:"))
                {
                    try
                    {
                        if (!piuForgli || (piuForgli && i < 69))
                        {
                            gt2 = listaTesto[i - 2].Split(new[] { " - " }, StringSplitOptions.None)[1].Split(' ');
                            descP = listaTesto[i - 2].Split('-')[0].Trim();
                            qta = Convert.ToInt32(decimal.Parse(gt2[2].Trim()));
                            impU = decimal.Parse(gt2[3].Trim());
                            ImpT = decimal.Parse(gt2[5].Trim());
                            sco = decimal.Parse(gt2[4].Trim());
                            if (sco < 1) { sco = sco * -1; }
                            codP = listaTesto[i - 1].Split(' ')[0].Trim();
                            lt = listaTesto[i].Split(' ')[2].Trim();
                            iva = decimal.Parse(gt2[6].Trim());
                        }
                        else if (piuForgli && i > 69)
                        {
                            var tst = listaTesto[i - 2];

                            if (tst.StartsWith("codice e descrizione articoli colli con"))
                            {
                                tst = listaTesto[i - 46];
                                if (!tst.StartsWith("FT."))
                                {
                                    continue;
                                }
                            }
                            gt2 = tst.Split(new[] { " - " }, StringSplitOptions.None)[1].Split(' ');
                            descP = tst.Split('-')[0].Trim();
                            qta = Convert.ToInt32(decimal.Parse(gt2[2].Trim()));
                            impU = decimal.Parse(gt2[3].Trim());
                            ImpT = decimal.Parse(gt2[5].Trim());
                            sco = decimal.Parse(gt2[4].Trim());
                            if (sco < 1) { sco = sco * -1; }
                            codP = listaTesto[i - 1].Split(' ')[0].Trim();
                            lt = listaTesto[i].Split(' ')[2].Trim();
                            iva = decimal.Parse(gt2[6].Trim());
                        }
                    }
                    catch { }

                    var nOrdine = new VociOrdine
                    {
                        CAPFatturazione = CapFatturazione,
                        CAPDestinazione = CapDestinazione,
                        CittaFatturazione = CittaFatturazione,
                        CittaDestinazione = CittaDestinazione,
                        DataOrdine = datadoc,
                        IndirizzoFatturazione = IndirizzoFatturazione,
                        IndirizzoDestinazione = IndirizzoDestinazione,
                        Note = Note,
                        NumeroOrdine = NumeroOrdine,
                        ProvFatturazione = ProvFatturazione,
                        ProvDestinazione = ProvDestinazione,
                        PIVAFatturazione = PivaFatturazione,
                        RagioneSocialeFatturazione = RagioneSocialeFatturazione,
                        DescrizioneProdotto = descP,
                        CodProdotto = codP,
                        QuantitaProdotto = qta,
                        ImportoUnitario = impU,
                        ImportoTotale = ImpT,
                        Sconto = sco,
                        Lotto = lt,
                        IVA = iva,
                        NomeDestinazione = NomeDestinazione,
                        Regione = RegioniItaliane.EstraiRegione(ProvDestinazione)
                    };
                    resp.Add(nOrdine);
                }
            }

            return resp;
        }

        public static List<VociOrdine> ConvertiTestoOrdineDomus(string testoPDF, out Exception ex)
        {
            ex = null;
            var resp = new List<VociOrdine>();
            List<string> listaTesto = new List<string>();
            listaTesto.AddRange(testoPDF.Split(new[] { "\n" }, StringSplitOptions.None));
            var datadoc = DateTime.Parse(listaTesto[5].Split(' ')[1]);
            var NumeroOrdine = listaTesto[5].Split(' ')[0];


            #region Destinazione
            int VersFile = 0;
            bool StessoIndirizzoDiSpedizione = false;
            if (listaTesto[24].ToLower() == "idem")
            {
                StessoIndirizzoDiSpedizione = true;
            }
            string CapDestinazione = "";
            string ProvDestinazione = "";
            string CittaDestinazione = "";

            var NomeDestinazione = listaTesto[13].Substring(7);
            var IndirizzoDestinazione = listaTesto[15];
            string tt = listaTesto[16];

            #region Individua destinazione
            var tts = tt.Split(' ');
            if (tts.Count() >= 3)
            {
                CapDestinazione = tt.Split(' ')[0];
                ProvDestinazione = tt.Replace("(", "").Replace(")", "").Split(' ').Last();
                var capL = CapDestinazione.Length;
                var ll = (CapDestinazione.Length - (tt.Length - 4)) * -1;
                CittaDestinazione = tt.Substring(capL, ll).Trim();
            }
            else if (tts.Count() == 2)
            {
                CapDestinazione = tt.Split(' ')[0];
                ProvDestinazione = tt.Split(' ').Last();
                CittaDestinazione = ProvDestinazione;
            }
            else
            {
                VersFile = 1;

                StessoIndirizzoDiSpedizione = true;
            }
            #endregion
            #endregion


            #region Fatturazione
            var RagioneSocialeFatturazione = "Domus Petri Pharmaceuticals s.r.l.";
            var IndirizzoFatturazione = "SS Sannitica km 20,700";
            string CapFatturazione = "81020";
            string ProvFatturazione = "CE";
            string CittaFatturazione = "San Marco Evangelista";
            string tt2 = listaTesto[3];
            string PivaFatturazione = "04334930619";
            #endregion


            var Note = "";

            int inizioProdotti = listaTesto.FindIndex(x => x == "Codice AIC") + 1;
            int FineProdotti = listaTesto.FindIndex(x => x == "Pallets Peso                        Tot. Pezzi") - 1;


            for (int i = inizioProdotti; i <= FineProdotti; i++)
            {
                var rigaProdottoSplit = listaTesto[i].Split(' ');
                string codP = rigaProdottoSplit[0];
                var scadenza = rigaProdottoSplit.Last();
                string lt = rigaProdottoSplit[rigaProdottoSplit.Length - 2];
                var qq = rigaProdottoSplit[rigaProdottoSplit.Length - 3];
                int qta = int.Parse(qq);

                var trunkCP = listaTesto[i].Substring(codP.Length).TrimStart();

                string descP = trunkCP.Substring(0, trunkCP.Length - lt.Length - qq.Length - scadenza.Length - 3).Trim();



                var nOrdine = new VociOrdine
                {
                    CAPFatturazione = CapFatturazione,
                    CAPDestinazione = CapDestinazione,
                    CittaFatturazione = CittaFatturazione,
                    CittaDestinazione = CittaDestinazione,
                    DataOrdine = datadoc,
                    IndirizzoFatturazione = IndirizzoFatturazione,
                    IndirizzoDestinazione = IndirizzoDestinazione,
                    Note = Note,
                    NumeroOrdine = NumeroOrdine,
                    ProvFatturazione = ProvFatturazione,
                    ProvDestinazione = ProvDestinazione,
                    PIVAFatturazione = PivaFatturazione,
                    RagioneSocialeFatturazione = RagioneSocialeFatturazione,
                    DescrizioneProdotto = descP,
                    CodProdotto = codP,
                    QuantitaProdotto = qta,
                    ImportoUnitario = 0,
                    ImportoTotale = 0,
                    Sconto = 0,
                    Lotto = lt,
                    IVA = 0,
                    NomeDestinazione = NomeDestinazione,
                    Regione = RegioniItaliane.EstraiRegione(ProvDestinazione)
                };
                resp.Add(nOrdine);

            }

            return resp;
        }

        public static List<VociOrdine> ConvertiInOrdinePolaris(FileDaImportare selezionato, out Exception ex, out int NumOrdiniExcel)
        {
            List<VociOrdine> resp = new List<VociOrdine>();
            Workbook daImportare = new Workbook();
            ex = null;
            NumOrdiniExcel = 1;
            try
            {
                daImportare.LoadDocument(selezionato.PathCompleto);

                var wksheet = daImportare.Worksheets[0];

                var rg = wksheet.GetUsedRange();


                for (int i = 2; i <= rg.RowCount; i++)
                {

                    var cP = wksheet.Cells[$"C{i }"].Value.ToString();
                    if (string.IsNullOrEmpty(cP))
                    {
                        continue;
                    }
                    var dOs = wksheet.Cells[$"B{i}"].Value.ToString();
                    var dO = (!string.IsNullOrWhiteSpace(dOs)) ? DateTime.Parse(dOs) : resp.Last().DataOrdine;

                    var qPs = wksheet.Cells[$"E{i }"].Value.ToString();
                    var qP = (!string.IsNullOrWhiteSpace(qPs)) ? decimal.Parse(qPs) : 0;

                    var iPs = wksheet.Cells[$"I{i }"].Value.ToString();
                    var iP = (!string.IsNullOrWhiteSpace(iPs)) ? decimal.Parse(iPs) : 0;

                    var sPs = wksheet.Cells[$"H{i }"].Value.ToString();
                    var sP = (!string.IsNullOrWhiteSpace(sPs)) ? decimal.Parse(sPs) : 0;
                    if (sP < 0) { sP = sP * -1; }

                    var iuPs = wksheet.Cells[$"F{i }"].Value.ToString();
                    var iuP = (!string.IsNullOrWhiteSpace(iuPs)) ? decimal.Parse(iuPs) : 0;

                    var itPs = wksheet.Cells[$"G{i }"].Value.ToString();
                    var itP = (!string.IsNullOrWhiteSpace(itPs)) ? decimal.Parse(itPs) : 0;

                    var nOs = wksheet.Cells[$"A{i}"].Value.ToString();
                    var nOc = "";
                    if (string.IsNullOrWhiteSpace(nOs))
                    {
                        for (int y = resp.Count(); y > 0; y--)
                        {
                            if (!string.IsNullOrWhiteSpace(resp[y - 1].NumeroOrdine))
                            {
                                nOc = resp[y - 1].NumeroOrdine;
                                break;
                            }
                        }

                    }
                    else
                    {
                        nOc = nOs;
                    }



                    var nTs = wksheet.Cells[$"W{i }"].Value.ToString();
                    string nT = "";
                    if (resp.Count() > 0)
                    {
                        nT = (!string.IsNullOrWhiteSpace(nTs)) ? nTs : (resp.Last().Note);
                    }
                    else
                    {
                        nT = (!string.IsNullOrWhiteSpace(nTs)) ? nTs : "";
                    }
                    var rGs = wksheet.Cells[$"L{i }"].Value.ToString();
                    var rG = (!string.IsNullOrWhiteSpace(rGs)) ? rGs : resp.Last().RagioneSocialeFatturazione;

                    var iFs = wksheet.Cells[$"N{i }"].Value.ToString();
                    var iF = (!string.IsNullOrWhiteSpace(iFs)) ? iFs : resp.Last().IndirizzoFatturazione;

                    var pIs = wksheet.Cells[$"M{i }"].Value.ToString().Replace(" ", "");
                    var pI = (!string.IsNullOrWhiteSpace(pIs)) ? pIs : resp.Last().PIVAFatturazione;

                    var cFs = wksheet.Cells[$"O{i }"].Value.ToString();
                    var cF = (!string.IsNullOrWhiteSpace(cFs)) ? cFs : resp.Last().CAPFatturazione;

                    var cTs = wksheet.Cells[$"P{i }"].Value.ToString();
                    var cT = (!string.IsNullOrWhiteSpace(cTs)) ? cTs : resp.Last().CittaFatturazione;

                    var pFs = wksheet.Cells[$"Q{i }"].Value.ToString();
                    var pF = (!string.IsNullOrWhiteSpace(pFs)) ? pFs : resp.Last().ProvFatturazione;

                    var nDs = wksheet.Cells[$"R{i }"].Value.ToString();
                    var nD = (!string.IsNullOrWhiteSpace(nDs)) ? nDs : resp.Last().NomeDestinazione;

                    var iDs = wksheet.Cells[$"S{i }"].Value.ToString();
                    var iD = (!string.IsNullOrWhiteSpace(iDs)) ? iDs : resp.Last().IndirizzoDestinazione;

                    var cDs = wksheet.Cells[$"T{i }"].Value.ToString();
                    var cD = (!string.IsNullOrWhiteSpace(cDs)) ? cDs : resp.Last().CAPDestinazione;

                    var cTds = wksheet.Cells[$"U{i }"].Value.ToString();
                    var cTd = (!string.IsNullOrWhiteSpace(cTds)) ? cTds : resp.Last().CittaDestinazione;

                    var pDs = wksheet.Cells[$"V{i }"].Value.ToString();
                    var pD = (!string.IsNullOrWhiteSpace(pDs)) ? pDs : resp.Last().ProvDestinazione;

                    var rDs = wksheet.Cells[$"X{i }"].Value.ToString();
                    var rD = (!string.IsNullOrWhiteSpace(pDs)) ? RegioniItaliane.EstraiRegione(pDs) : RegioniItaliane.EstraiRegione(resp.Last().ProvDestinazione);


                    if (i > 2 && !string.IsNullOrWhiteSpace(nOc) && nOc != resp.Last().NumeroOrdine)
                    {
                        NumOrdiniExcel++;
                    }

                    var nO = new VociOrdine()
                    {

                        Barcode = wksheet.Cells[$"J{i }"].Value.ToString(),
                        CodProdotto = cP,
                        Lotto = wksheet.Cells[$"K{i + 2}"].Value.ToString(),
                        DescrizioneProdotto = wksheet.Cells[$"D{i }"].Value.ToString(),
                        QuantitaProdotto = qP,
                        IVA = iP,
                        Sconto = sP,
                        ImportoUnitario = iuP,
                        ImportoTotale = itP,

                        NumeroOrdine = nOc,
                        DataOrdine = dO,
                        Note = nT,

                        RagioneSocialeFatturazione = rG,
                        PIVAFatturazione = pI,
                        IndirizzoFatturazione = iF,
                        CAPFatturazione = cF,
                        CittaFatturazione = cT,
                        ProvFatturazione = pF,

                        NomeDestinazione = nD,
                        IndirizzoDestinazione = iD,
                        CAPDestinazione = cD,
                        CittaDestinazione = cTd,
                        ProvDestinazione = pD,
                        Regione = rD
                    };
                    resp.Add(nO);

                }
            }
            catch (Exception ee)
            {
                ex = ee;
            }

            return resp;
        }

        public static string CopiaModelloExcel(string pathfile)
        {
            var exeP = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);


            if (!Directory.Exists(appdata)) Directory.CreateDirectory(appdata);
            var resp = Path.Combine(appdata, Path.ChangeExtension(Path.GetFileName(pathfile), "xlsx"));
            if (File.Exists(resp)) File.Delete(resp);
            File.Copy("TemplateOrdini.xlsx", $"{resp}");
            return resp;
        }
    }
}
