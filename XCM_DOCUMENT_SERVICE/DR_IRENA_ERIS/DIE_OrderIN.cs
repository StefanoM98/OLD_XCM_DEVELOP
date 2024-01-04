using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE.DR_IRENA_ERIS
{
    public class DIE_OrderIN
    {

        // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class IOrder
        {

            private IOrderMessageId messageIdField;

            private IOrderIOrdHead iOrdHeadField;

            private IOrderIOrdPos[] posField;

            /// <remarks/>
            public IOrderMessageId MessageId
            {
                get
                {
                    return this.messageIdField;
                }
                set
                {
                    this.messageIdField = value;
                }
            }

            /// <remarks/>
            public IOrderIOrdHead IOrdHead
            {
                get
                {
                    return this.iOrdHeadField;
                }
                set
                {
                    this.iOrdHeadField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("IOrdPos", IsNullable = false)]
            public IOrderIOrdPos[] Pos
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
        public partial class IOrderMessageId
        {

            private string idField;

            /// <remarks/>
            public string ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class IOrderIOrdHead
        {

            private string oPERATIONField;

            private string sTORE_ORD_NRField;

            private uint oRD_NRField;

            private string oRDER_TYPEField;

            private string m3_ORDER_TYPEField;

            private string fR_CODEField;

            private string fR_NAME1Field;

            private string fR_NAME2Field;

            private object fR_POST_CODEField;

            private string fR_CITYField;

            private string fR_STREETField;

            private string fR_COUNTRYField;

            private uint dEPART_DFROMField;

            private uint dEPART_DTOField;

            private string rEMARKS1Field;

            private object aDD_NRField;

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
            public uint ORD_NR
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
            public string M3_ORDER_TYPE
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

            /// <remarks/>
            public string FR_CODE
            {
                get
                {
                    return this.fR_CODEField;
                }
                set
                {
                    this.fR_CODEField = value;
                }
            }

            /// <remarks/>
            public string FR_NAME1
            {
                get
                {
                    return this.fR_NAME1Field;
                }
                set
                {
                    this.fR_NAME1Field = value;
                }
            }

            /// <remarks/>
            public string FR_NAME2
            {
                get
                {
                    return this.fR_NAME2Field;
                }
                set
                {
                    this.fR_NAME2Field = value;
                }
            }

            /// <remarks/>
            public object FR_POST_CODE
            {
                get
                {
                    return this.fR_POST_CODEField;
                }
                set
                {
                    this.fR_POST_CODEField = value;
                }
            }

            /// <remarks/>
            public string FR_CITY
            {
                get
                {
                    return this.fR_CITYField;
                }
                set
                {
                    this.fR_CITYField = value;
                }
            }

            /// <remarks/>
            public string FR_STREET
            {
                get
                {
                    return this.fR_STREETField;
                }
                set
                {
                    this.fR_STREETField = value;
                }
            }

            /// <remarks/>
            public string FR_COUNTRY
            {
                get
                {
                    return this.fR_COUNTRYField;
                }
                set
                {
                    this.fR_COUNTRYField = value;
                }
            }

            /// <remarks/>
            public uint DEPART_DFROM
            {
                get
                {
                    return this.dEPART_DFROMField;
                }
                set
                {
                    this.dEPART_DFROMField = value;
                }
            }

            /// <remarks/>
            public uint DEPART_DTO
            {
                get
                {
                    return this.dEPART_DTOField;
                }
                set
                {
                    this.dEPART_DTOField = value;
                }
            }

            /// <remarks/>
            public string REMARKS1
            {
                get
                {
                    return this.rEMARKS1Field;
                }
                set
                {
                    this.rEMARKS1Field = value;
                }
            }

            /// <remarks/>
            public object ADD_NR
            {
                get
                {
                    return this.aDD_NRField;
                }
                set
                {
                    this.aDD_NRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class IOrderIOrdPos
        {

            private byte sNPOS_NRField;

            private string sKUField;

            private string pROD_NAME1Field;

            private byte nUMBERField;

            private string mEAS_UNITField;

            private object sERIAField;

            private object vALID_DATEField;

            private object pROD_DATEField;

            private string lOG_STORE1Field;

            private byte lOG_STORE2Field;

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
            public object SERIA
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
            public object VALID_DATE
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
        }


    }
}
