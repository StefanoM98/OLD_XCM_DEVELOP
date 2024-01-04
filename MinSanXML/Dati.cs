using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinSanXML
{
    public class DatiHead
    {
        public string TipoTrasmissione { get; set; }
        public string TipoMittente { get; set; }
        public string TipoDestinatario { get; set; }
        public int Mese { get; set; }
        public int Anno { get; set; }
    }
    public class DatiBody
    {
      
        public string ID_DEST { get; set; }
        public string CODICE_AIC { get; set; }
        public string QUANTITA { get; set; }
        public string VALORE { get; set; }
    }
}
