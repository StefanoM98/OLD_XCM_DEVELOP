using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX
{

    public class RootobjectGoodsUpdate
    {
        public GoodsUpdate goods { get; set; }
    }

    public class GoodsUpdate
    {
        public int id { get; set; }
        public int loadStopID { get; set; }
        public int unLoadStopID { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string holderID { get; set; }
        public string packsTypeID { get; set; }
        public string packsTypeDes { get; set; }
        public int packs { get; set; }
        public decimal floorPallet { get; set; }
        public int totalPallet { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal meters { get; set; }
        public int seat { get; set; }
        public decimal height { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public string containerNo { get; set; }
    }

}
