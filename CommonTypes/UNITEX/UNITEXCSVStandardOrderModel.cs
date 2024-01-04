using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.UNITEX
{
	class UNITEXCSVStandardOrderModel
	{
        public string SegmentName { get; set; }
        public string ParcelBarcode { get; set; }
        public string externalRef { get; set; }//RTST
        public string Incoterm { get; set; }//RTST
        public string ServiceType { get; set; }//RTST
        public string CarrierType { get; set; }//RTST
        public string TransportType { get; set; }//RTST
        public string GoodsValue { get; set; }//RTST
        public string Barcode { get; set; }//RTST
        public string Info { get; set; }//RTST
        public string info0 { get; set; }//RTST
        public string InsideRef { get; set; }//RTST
        public string CashValue { get; set; }//RTST
        public string CashCurrency { get; set; }//RTST
        public string CashPaymentType { get; set; }//RTST
        public string LoadDate { get; set; }//RTST
        public string LoadTime { get; set; }//RTST
        public string LoadName { get; set; }//RTST
        public string LoadAddress { get; set; }//RTST
        public string LoadTown { get; set; }//RTST
        public string LoadCountry { get; set; }//RTST
        public string LoadDiscrict { get; set; }//RTST
        public string LoadZipCode { get; set; }//RTST
        public string UnloadDate { get; set; }//RTST
        public string UnloadTime { get; set; }//RTST
        public string UnloadName { get; set; }//RTST
        public string UnloadAddress { get; set; }//RTST
        public string UnloadTown { get; set; }//RTST
        public string UnloadCountry { get; set; }//RTST
        public string UnloadDistrict { get; set; }//RTST
        public string UnloadZipCode { get; set; }//RTST
        public string GrossWeight { get; set; }//RTST
        public string Meters { get; set; }//RTST
        public string Cube { get; set; }//RTST
        public string Plts { get; set; }//RTST
        public string PltsTotal { get; set; }//RTST
        public string GoodsWidth { get; set; }//RTST
        public string GoodsHeight { get; set; }//RTST
        public string GoodsDeep { get; set; }//RTST
        public string Packs { get; set; }//RTST
        public string DocDate { get; set; }//RTST
        public string DataTassativa { get; set; }//RTST
        public override string ToString()
        {//                 1            2           3              4          5                6             7          8       9     10       11           12           13             14               15          16         17          18           19          20           21              22             23          24             25           26           27          28             29                 30                31         32      33     34         35          36         37           38         39          40           41
            return $"{SegmentName};{externalRef};{Incoterm};{ServiceType};{CarrierType};{TransportType};{GoodsValue};{Barcode};{Info};{info0};{InsideRef};{CashValue};{CashCurrency};{CashPaymentType};{LoadDate};{LoadTime};{LoadName};{LoadAddress};{LoadTown};{LoadCountry};{LoadDiscrict};{LoadZipCode};{UnloadDate};{UnloadTime};{UnloadName};{UnloadAddress};{UnloadTown};{UnloadCountry};{UnloadDistrict};{UnloadZipCode};{GrossWeight};{Meters};{Cube};{Plts};{PltsTotal};{GoodsWidth};{GoodsHeight};{GoodsDeep};{Packs};{DataTassativa};{DocDate}";
        }
    }
}
