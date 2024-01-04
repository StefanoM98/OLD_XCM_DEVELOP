using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC.Code
{
    public class FileHeader
    {

        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string TrackingDate { get; set; }
        public string StatusCode { get; set; }

        public bool MancaSH { get; set; }
        public bool MancaStatusCode { get; set; }
        public bool MancaZero { get; set; }

        public List<string> ColumnsName { get; set; }

        public FileHeader()
        {
            this.ColumnsName = new List<string>();
        }

        public static FileHeader ImprotaFileHeader()
        {
            FileHeader header = new FileHeader();

            header.DocumentNumber = "DDT";
            header.TrackingDate = "Esiti";
            header.MancaSH = true;
            header.MancaStatusCode = true;
            header.MancaZero = true;

            return header;
        }

        public static FileHeader AllWaysFileHeader()
        {
            FileHeader header = new FileHeader();

            header.DocumentNumber = "RIF ORDINE";
            header.TrackingDate = "DATA CONSEGNA";
            header.MancaSH = false;
            header.MancaStatusCode = true;
            header.MancaZero = true;

            return header;
        }

        public static FileHeader EmmeaFileHeader()
        {
            FileHeader header = new FileHeader();

            header.DocumentNumber = "num_rife";
            header.TrackingDate = "dat_stat";
            header.MancaSH = true;
            header.MancaStatusCode = true;
            header.MancaZero = true;

            return header;
        }

        public static FileHeader TliFileHeader()
        {
            FileHeader header = new FileHeader();

            header.DocumentNumber = "Riferimento";
            header.TrackingDate = "Data conse";
            header.MancaSH = false;
            header.MancaStatusCode = true;
            header.MancaZero = true;

            return header;  
        }

        public static FileHeader GetHeader(string name)
        {
            if (name == "IMPROTA") return ImprotaFileHeader();
            if (name == "ALLWAYS") return AllWaysFileHeader();
            if (name == "EMMEA") return EmmeaFileHeader();
            if (name == "TLI") return TliFileHeader();
            return new FileHeader();
        }
    }
}
