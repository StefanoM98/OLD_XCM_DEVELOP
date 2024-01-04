using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.CDL
{
    class Cliente
    {
        public string CustomerID { get; set; }
        public string WorkDir { get; set; }
        public List<string> Esiti { get; set; }
        
        public string FileName { get; set; }
    }
}
