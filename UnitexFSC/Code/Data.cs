using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC.Code
{
    public class Data
    {
        public static string FileName = "EsitiSettings.txt";

        public static List<FileHeader> db { get; set; }

        public static void Init()
        {
            if(db is null)
            {
                db = new List<FileHeader>();
                List<string> lines = File.ReadAllLines(FileName).ToList();

                foreach (var line in lines)
                {
                    var values = line.Split('|');

                    var fileHeader = new FileHeader()
                    {
                        Name = values[0],
                        DocumentNumber = values[1],
                        TrackingDate = values[2],
                        //StatusCode = values[2],
                        MancaSH = Convert.ToBoolean(values[3]),
                        //MancaStatusCode = Convert.ToBoolean(values[4]),
                        //MancaZero = Convert.ToBoolean(values[5]),
                    };

                    db.Add(fileHeader);

                }
            }

        }

        public static void RefreshDb()
        {
            List<string> lines = new List<string>();
            foreach(var elem in db)
            {
                var newLine = $"{elem.Name}|{elem.DocumentNumber}|{elem.TrackingDate}|{elem.MancaSH}";
                lines.Add(newLine);
            }

            File.WriteAllLines(FileName, lines);
        }
    }
}
