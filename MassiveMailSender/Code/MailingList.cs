using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Spreadsheet;
using MassiveMailSender.Model;
using Newtonsoft.Json;

namespace MassiveMailSender.Code
{
    public class MailingList
    {
        public static List<DocumentRowModel> mailingList = new List<DocumentRowModel>();
        public static string dbName = "dfdsjcnd";
        public static string dbNameTemp = "dfdsjcndt";

        public void Import(string sourceFileName)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(sourceFileName);
            var wksheet = workbook.Worksheets[0];

            var docRange = wksheet.GetUsedRange();
            var totRighe = docRange.RowCount;


            for (int i = 2; i <= totRighe; i++)
            {
                var row = new DocumentRowModel()
                {
                    Id = i,
                    RagioneSociale = wksheet.Cells[$"A{i}"].Value.ToString(),
                    Citta  = wksheet.Cells[$"B{i}"].Value.ToString(),
                    Nazione = wksheet.Cells[$"C{i}"].Value.ToString(),
                    Indirizzo = wksheet.Cells[$"D{i}"].Value.ToString(),
                    Provincia = wksheet.Cells[$"E{i}"].Value.ToString(),
                    Recapiti = wksheet.Cells[$"F{i}"].Value.ToString(),
                    Mail = wksheet.Cells[$"G{i}"].Value.ToString(),
                    Settore = wksheet.Cells[$"H{i}"].Value.ToString()
                };
                mailingList.Add(row);
            }
            
        }

        public void SalvaMailingList()
        {
            File.WriteAllText(dbName, Crypt.Encrypt(JsonConvert.SerializeObject(mailingList)));
        }

        public void CaricaMailingList(bool temp)
        {
            if (temp)
            {
                mailingList =  JsonConvert.DeserializeObject<List<DocumentRowModel>>(Crypt.Decrypt(File.ReadAllText(dbNameTemp)));
                if (File.Exists(dbName))
                {
                    File.Delete(dbName);

                }
                File.Move(dbNameTemp, dbName);
            }
            else
            {
                mailingList =  JsonConvert.DeserializeObject<List<DocumentRowModel>>(Crypt.Decrypt(File.ReadAllText(dbName)));
                if (File.Exists(dbNameTemp))
                {
                    File.Delete(dbNameTemp);

                }
            }
        }

        internal void SalvaMailingListTemp()
        {
            File.WriteAllText(dbNameTemp, Crypt.Encrypt(JsonConvert.SerializeObject(mailingList)));
        }
        
        public void AggiornaRecordMailingList(DocumentRowModel rowAggiornata)
        {
            var daAggiornare = mailingList.FirstOrDefault(x => x.Id == rowAggiornata.Id);
            daAggiornare = rowAggiornata;
            SalvaMailingListTemp();
        }
    }
}
