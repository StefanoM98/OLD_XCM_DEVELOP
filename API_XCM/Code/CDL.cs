using API_XCM.Code.APIs;
using API_XCM.EF;
using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace API_XCM.Code
{


    public class CDL
    {
        public static void CDLStats()
        {
            CDLEntities entity = new CDLEntities();
            var fromDate = new DateTime(2022, 01, 01, 00, 00, 00);
            var shipments = entity.va_bolle_sat.Where(x => x.data_ora_inserimento > fromDate).ToList();

            List<string> provinceTriveneto = File.ReadAllLines(@"C:\UNITEX\ProvincieTriveneto.txt").ToList();

            Workbook workbook = new Workbook();


            var wksheet = workbook.Worksheets[0];

            var docRange = wksheet.GetUsedRange();
            var totRighe = docRange.RowCount;
            var rowIndex = 2;

            foreach (var pv in provinceTriveneto)
            {
                //var ships = AllShips.Where(x => x.consigneeDistrict == pv).ToList();

                var cnt = shipments.Where(x => x.prov_destinatario == pv).ToList().Count();


                wksheet.Cells[$"A{rowIndex}"].Value = pv;
                wksheet.Cells[$"B{rowIndex}"].Value = cnt;
                rowIndex++;

            }

            workbook.SaveDocument(@"C:\UNITEX\CDLTrivenetoStats.xlsx", DocumentFormat.Xlsx);


        }

    }

    public class CDLModel
    {
        public string Stato { get; set; }
        public string DocNumber { get; set; }
        public string DataDocumento { get; set; }
        public string DataConsegna { get; set; }
        public string ExternalRef { get; set; }
        public string InternalRef { get; set; }
        public string Vettore { get; set; }

        public static CDLModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            CDLModel list = new CDLModel();
            list.Stato = Convert.ToString(values[0]);
            list.DocNumber = Convert.ToString(values[1]);
            //list.DataDocumento = Convert.ToString(values[2]);
            list.DataConsegna = Convert.ToString(values[2]);
            list.InternalRef = Convert.ToString(values[3]);
            list.ExternalRef = Convert.ToString(values[4]);
            //list.Vettore = Convert.ToString(values[5]);
            return list;
        }
    }

    public class ClarityModel
    {
        public string RifCDL { get; set; }
        public string ExtRif { get; set; }
        public string Vettore { get; set; }
        //public string ExternalRef { get; set; }
        //public string InternalRef { get; set; }
        public string Unload { get; set; }
        public string Stato { get; set; }

        public static ClarityModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            ClarityModel list = new ClarityModel();
            list.RifCDL = Convert.ToString(values[0]);
            list.ExtRif = Convert.ToString(values[1]);
            //list.Stato = Convert.ToString(values[2]);
            //list.Unload = Convert.ToString(values[3]);
            return list;
        }
    }

    public class Esiti
    {
        public string UnitexId { get; set; }
        public string ExternalRef { get; set; }
        public DateTime DataTracking { get; set; }
        public int Stato { get; set; }

        public string CustomerID { get; set; }
        public int ShipID { get; set; }
        public int StatudId { get; set; }

        public static Esiti FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            Esiti esiti = new Esiti();
            var firstElement = Convert.ToString(values[0]);
            if (firstElement.ToLower().Contains("/sh"))
            {
                esiti.UnitexId = firstElement;
            }
            else
            {

                esiti.ExternalRef = firstElement;
            }
            esiti.DataTracking = Convert.ToDateTime(values[1]);
            //esiti.StatudId = Convert.ToInt32(values[2]);

            return esiti;
        }

    }

    public class CDLModel2
    {
        public string RifCDL { get; set; }
        public string Stato { get; set; }
        public string ExtRif { get; set; }
        public string Vettore { get; set; }
        public string Unload { get; set; }

        public static CDLModel2 FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            CDLModel2 list = new CDLModel2();
            list.RifCDL = Convert.ToString(values[0]);
            list.ExtRif = Convert.ToString(values[1]);
            list.Stato = Convert.ToString(values[2]);

            list.Vettore = Convert.ToString(values[3]);
            //list.Unload = Convert.ToString(values[3]);
            return list;
        }
    }


    public class UnitexModel
    {
        public string Stato { get; set; }
        public string RifUnitex { get; set; }
        public string RifCdl { get; set; }
        public string RifInternal { get; set; }
        public string DataCons { get; set; }
        public string Vettore { get; set; }

        public static UnitexModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(';');
            UnitexModel list = new UnitexModel();
            list.Stato = Convert.ToString(values[0]);
            list.RifUnitex = Convert.ToString(values[1]);
            list.DataCons = Convert.ToString(values[2]);
            list.RifInternal = Convert.ToString(values[3]);
            list.RifCdl = Convert.ToString(values[4]);
            list.Vettore = Convert.ToString(values[5]);
            return list;
        }
    }
}