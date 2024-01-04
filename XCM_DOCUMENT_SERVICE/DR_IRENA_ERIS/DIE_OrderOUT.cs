using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE.DR_IRENA_ERIS
{
    public class DIE_OrderOUT
    {

        // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class IConfOrder
        {

            private IConfOrderIConfOrderHead iConfOrderHeadField;

            private IConfOrderIConfOrdPos[] posField;

            /// <remarks/>
            public IConfOrderIConfOrderHead IConfOrderHead
            {
                get
                {
                    return this.iConfOrderHeadField;
                }
                set
                {
                    this.iConfOrderHeadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("IConfOrdPos", IsNullable = false)]
            public IConfOrderIConfOrdPos[] Pos
            {
                get
                {
                    return this.posField;
                }
                set
                {
                    this.posField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class IConfOrderIConfOrderHead
        {

            private string oPERATIONField;

            private string sTORE_ORD_NRField;

            private object oRD_NRField;

            private string oRDER_TYPEField;

            private object m3_ORDER_TYPEField;

            /// <remarks/>
            public string OPERATION
            {
                get
                {
                    return this.oPERATIONField;
                }
                set
                {
                    this.oPERATIONField = value;
                }
            }

            /// <remarks/>
            public string STORE_ORD_NR
            {
                get
                {
                    return this.sTORE_ORD_NRField;
                }
                set
                {
                    this.sTORE_ORD_NRField = value;
                }
            }

            /// <remarks/>
            public object ORD_NR
            {
                get
                {
                    return this.oRD_NRField;
                }
                set
                {
                    this.oRD_NRField = value;
                }
            }

            /// <remarks/>
            public string ORDER_TYPE
            {
                get
                {
                    return this.oRDER_TYPEField;
                }
                set
                {
                    this.oRDER_TYPEField = value;
                }
            }

            /// <remarks/>
            public object M3_ORDER_TYPE
            {
                get
                {
                    return this.m3_ORDER_TYPEField;
                }
                set
                {
                    this.m3_ORDER_TYPEField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class IConfOrderIConfOrdPos
        {

            private byte sNPOS_NRField;

            private string sKUField;

            private string pROD_NAME1Field;

            private byte nUMBERField;

            private string mEAS_UNITField;

            private string sERIAField;

            private uint vALID_DATEField;

            private object pROD_DATEField;

            private string lOG_STORE1Field;

            private byte lOG_STORE2Field;

            private object sHIP_DATEField;

            /// <remarks/>
            public byte SNPOS_NR
            {
                get
                {
                    return this.sNPOS_NRField;
                }
                set
                {
                    this.sNPOS_NRField = value;
                }
            }

            /// <remarks/>
            public string SKU
            {
                get
                {
                    return this.sKUField;
                }
                set
                {
                    this.sKUField = value;
                }
            }

            /// <remarks/>
            public string PROD_NAME1
            {
                get
                {
                    return this.pROD_NAME1Field;
                }
                set
                {
                    this.pROD_NAME1Field = value;
                }
            }

            /// <remarks/>
            public byte NUMBER
            {
                get
                {
                    return this.nUMBERField;
                }
                set
                {
                    this.nUMBERField = value;
                }
            }

            /// <remarks/>
            public string MEAS_UNIT
            {
                get
                {
                    return this.mEAS_UNITField;
                }
                set
                {
                    this.mEAS_UNITField = value;
                }
            }

            /// <remarks/>
            public string SERIA
            {
                get
                {
                    return this.sERIAField;
                }
                set
                {
                    this.sERIAField = value;
                }
            }

            /// <remarks/>
            public uint VALID_DATE
            {
                get
                {
                    return this.vALID_DATEField;
                }
                set
                {
                    this.vALID_DATEField = value;
                }
            }

            /// <remarks/>
            public object PROD_DATE
            {
                get
                {
                    return this.pROD_DATEField;
                }
                set
                {
                    this.pROD_DATEField = value;
                }
            }

            /// <remarks/>
            public string LOG_STORE1
            {
                get
                {
                    return this.lOG_STORE1Field;
                }
                set
                {
                    this.lOG_STORE1Field = value;
                }
            }

            /// <remarks/>
            public byte LOG_STORE2
            {
                get
                {
                    return this.lOG_STORE2Field;
                }
                set
                {
                    this.lOG_STORE2Field = value;
                }
            }

            /// <remarks/>
            public object SHIP_DATE
            {
                get
                {
                    return this.sHIP_DATEField;
                }
                set
                {
                    this.sHIP_DATEField = value;
                }
            }
        }


    }
}
