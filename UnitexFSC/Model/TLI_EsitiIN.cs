using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC.Model
{
    public class TLI_EsitiIN
    {
        //Data Consegna YYYYMMdd
        public string TBDAT { get; set; }
        public int[] idxTBDAT = new int[] { 22, 8 };

        //Ora Consegna HHMMSS
        public string TBORA { get; set; }
        public int[] idxTBORA = new int[] { 30, 6 };

        //Riferimento Cliente
        public string TBRMI { get; set; }
        public int[] idxTBRMI = new int[] { 66, 20 };

    }
}
