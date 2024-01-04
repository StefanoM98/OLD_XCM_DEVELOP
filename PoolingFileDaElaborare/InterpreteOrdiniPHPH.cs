using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public class InterpreteOrdiniPHPH_Header
    {
        public string TestoFileHeader { get; set; }

        #region Committente
        public string FattNome
        {
            get
            {
                return TestoFileHeader.Substring(70, 30).Trim();
            }
        }
        public string FattIndirizzo
        {
            get
            {
                return TestoFileHeader.Substring(130, 30).Trim();
            }
        }
        public string FattCitta
        {
            get
            {
                return TestoFileHeader.Substring(160, 30).Trim();
            }
        }
        public string FattProvincia
        {
            get
            {
                return TestoFileHeader.Substring(190, 2).Trim();
            }
        }
        public string FattCAP
        {
            get
            {
                return TestoFileHeader.Substring(192, 5).Trim();
            }
        }
        public string FattPIVA
        {
            get
            {
                return TestoFileHeader.Substring(197, 16).Trim();
            }
        }
        #endregion

        #region Destinatario
        public string SpedNome
        {
            get
            {
                return string.Join("-", TestoFileHeader.Substring(213, 30).Trim(), TestoFileHeader.Substring(244, 30).Trim());
            }
        }
        public string SpedIndirizzo
        {
            get
            {
                return TestoFileHeader.Substring(273, 30).Trim();
            }
        }
        public string SpedCitta
        {
            get
            {
                return TestoFileHeader.Substring(303, 30).Trim();
            }
        }
        public string SpedProvincia
        {
            get
            {
                return TestoFileHeader.Substring(333, 2).Trim();
            }
        }
        public string SpedCAP
        {
            get
            {
                return TestoFileHeader.Substring(335, 5).Trim();
            }
        }
        #endregion
    }
    public class InterpreteOrdiniPHPH_Row
    {
        public string TestoFileRow { get; set; }

        public string DeliveryNO
        {
            get
            {
                return TestoFileRow.Substring(8, 20).Trim();
            }
        }
        public string Lotto
        {
            get
            {
                return TestoFileRow.Substring(64, 20).Trim();
            }
        }
        public string CodiceArticolo
        {
            get
            {
                return TestoFileRow.Substring(39, 25).Trim();
            }
        }
        public decimal Quantita
        {
            get
            {
                return decimal.Parse(TestoFileRow.Substring(84, 11).Trim());//Ultime tre cifre sono dopo la virgola
            }
        }
    }
    public class InterpreteOrdiniPHPH_Comment
    {

    }
}
