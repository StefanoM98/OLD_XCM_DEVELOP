using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovimentiMagazzinoFromGespe
{
    public partial class ANAGRAFICA_CLIENTI_CUST : ANAGRAFICA_CLIENTI
    {
        public override string ToString()
        {
            return RAGIONE_SOCIALE;
        }
    }
}
