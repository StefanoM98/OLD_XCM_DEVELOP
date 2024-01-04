using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitexFSC.Code.APIs;
using UnitexFSC.Model;

namespace UnitexFSC.Code
{
    public class TLI
    {
        public static List<string> ProduciCSVEsiti(string inputFileName)
        {

            var tutteLeRighe = File.ReadAllLines(inputFileName);
            List<string> csvContent = new List<string>();

            for(int i = 0; i < tutteLeRighe.Count(); i++)
            {
                var r = tutteLeRighe[i];

                var element = new TLI_EsitiIN();

                element.TBRMI = r.Substring(element.idxTBRMI[0], element.idxTBRMI[1]);
                element.TBDAT = r.Substring(element.idxTBDAT[0], element.idxTBDAT[1]);
                element.TBORA = r.Substring(element.idxTBORA[0], element.idxTBORA[1]);

                var DataConsenga = DateTime.ParseExact($"{element.TBDAT} {element.TBORA}" , "yyyyMMdd HHmmss", null);
                var csvLine = $"{element.TBRMI.Trim()}/SH;{DataConsenga};30";

                csvContent.Add(csvLine);
            }

            return csvContent;
        }

        public static void Insert(string record, int position, int lentgh, StringBuilder row, string str, bool reverse)
        {
            if(record.Length < lentgh)
            {
                if (reverse)
                {
                    row.Insert(position, str, lentgh - record.Length);
                    row.Insert((position + lentgh) - record.Length, record, 1);
                }
                else
                {

                    row.Insert(position, record, 1);
                    row.Insert(position + record.Length, str, lentgh - record.Length);
                }

            }
            else if(record.Length > lentgh)
            {
                row.Insert(position, record.Substring(0, lentgh), 1);
            }
            else
            {
                row.Insert(position, record, 1);
            }
        }


        public static List<string> ProduciShipmentOUT(string tripNumber)
        {
            EspritecAPI_UNITEX.Init("dvalitutti", "Dv$2022!", "UNITEX");
            var trip = EspritecAPI_UNITEX.TmsTripList().FirstOrDefault(x => x.docNumber == tripNumber);

            if(trip != null)
            {
                var shipments = EspritecAPI_UNITEX.TmsTripStopList(trip.id);
                List<string> content = new List<string>();
                foreach(var ship in shipments)
                {
                    var line = new TLI_ShipmentOUT();

                    var shDetail = EspritecAPI_UNITEX.TmsShipmentGet(ship.shipID);

                    StringBuilder row = new StringBuilder();
                    if(shDetail != null)
                    {
                        line.DKREC = " ";
                        Insert(line.DKREC, line.idxDKREC[0], line.idxDKREC[1], row, " ", false);

                        line.DKTIP = "1";
                        Insert(line.DKTIP, line.idxDKTIP[0], line.idxDKTIP[1], row, " ", false);

                        line.DKDTB = shDetail.docDate.ToString("yyyyMMdd");
                        Insert(line.DKDTB, line.idxDKDTB[0], line.idxDKDTB[1], row, " ", false);

                        line.DKRMI = shDetail.docNumber.Split('/')[0];
                        Insert(line.DKRMI, line.idxDKRMI[0], line.idxDKRMI[1], row, " ", false);

                        line.DKRM2 = shDetail.externRef.Replace("/ORM", "");
                        Insert(line.DKRM2, line.idxDKRM2[0], line.idxDKRM2[1], row, " ", false);

                        //Rif operatore logistico
                        line.DKRUL = "";
                        Insert(line.DKRUL, line.idxDKRUL[0], line.idxDKRUL[1], row, " ", false);

                        line.DKDTX = shDetail.docDate.ToString("yyyyMMdd");
                        Insert(line.DKDTX, line.idxDKDTX[0], line.idxDKDTX[1], row, " ", false);

                        line.DKANB = shDetail.docDate.ToString("yy");
                        Insert(line.DKANB, line.idxDKANB[0], line.idxDKANB[1], row, " ", false);

                        //Filiale bollettazione
                        line.DKFBO = shDetail.ownerAgency;
                        Insert(line.DKFBO, line.idxDKFBO[0], line.idxDKFBO[1], row, " ", false);

                        //Progressivo chiave 7
                        line.DKKEY = "01"; //TODO: che ci mettiamo
                        Insert(line.DKKEY, line.idxDKKEY[0], line.idxDKKEY[1], row, " ", false);

                        //Codice corrispondente
                        line.DKBOR = ""; 
                        Insert(line.DKBOR, line.idxDKBOR[0], line.idxDKBOR[1], row, " ", false);

                        //Data borderò
                        line.DKDBO = trip.docDate.ToString("yyyyMMdd");
                        Insert(line.DKDBO, line.idxDKDBO[0], line.idxDKDBO[1], row, " ", false);

                        #region Cliente mittente
                        //Filiale
                        line.DKMFI = "";
                        Insert(line.DKMFI, line.idxDKMFI[0], line.idxDKMFI[1], row, " ", false);

                        //Codice cliente
                        line.DKMCO = "";
                        Insert(line.DKMCO, line.idxDKMCO[0], line.idxDKMCO[1], row, " ", false);

                        //Mittente cottratto
                        line.DKMCN = "";
                        Insert(line.DKMCN, line.idxDKMCN[0], line.idxDKMCN[1], row, " ", false);
                        #endregion
                        
                        #region Mittente Spedizione
                        //TODO: customerDes or Consignee or firstStopD
                        //Ragione sociale
                        line.DKMIT = shDetail.consigneeDes;
                        Insert(line.DKMIT, line.idxDKMIT[0], line.idxDKMIT[1], row, " ", false);

                        line.DKMIN = shDetail.consigneeAddress;
                        Insert(line.DKMIN, line.idxDKMIN[0], line.idxDKMIN[1], row, " ", false);

                        line.DKMLO = shDetail.consigneeLocation;
                        Insert(line.DKMLO, line.idxDKMLO[0], line.idxDKMLO[1], row, " ", false);

                        line.DKMPR = ship.district; //TODO: non c'è il district in consignee??
                        Insert(line.DKMPR, line.idxDKMPR[0], line.idxDKMPR[1], row, " ", false);

                        line.DKMCP = shDetail.consigneeZipcode;
                        Insert(line.DKMCP, line.idxDKMCP[0], line.idxDKMCP[1], row, " ", false);

                        line.DKMST = shDetail.consigneeCountry;
                        Insert(line.DKMST, line.idxDKMST[0], line.idxDKMST[1], row, " ", false);

                        //Codice magazzino di partenza?? impostato (BOLEG)
                        line.DKMAE = "";
                        Insert(line.DKMAE, line.idxDKMAE[0], line.idxDKMAE[1], row, " ", false);

                        #endregion

                        #region Destinatario Spedizione
                        //TODO: Consignee or Unload
                        //Ragione sociale
                        line.DKDIT = ship.description;
                        Insert(line.DKDIT, line.idxDKDIT[0], line.idxDKDIT[1], row, " ", false);

                        line.DKDIN = ship.address;
                        Insert(line.DKDIN, line.idxDKDIN[0], line.idxDKDIN[1], row, " ", false);

                        line.DKDLO = ship.location;
                        Insert(line.DKDLO, line.idxDKDLO[0], line.idxDKDLO[1], row, " ", false);

                        line.DKDPR = ship.district; //TODO: non c'è il district in consignee??
                        Insert(line.DKDPR, line.idxDKDPR[0], line.idxDKDPR[1], row, " ", false);

                        line.DKDCP = ship.zipCode;
                        Insert(line.DKDCP, line.idxDKDCP[0], line.idxDKDCP[1], row, " ", false);

                        line.DKDST = "";//ship.country;
                        Insert(line.DKDST, line.idxDKDST[0], line.idxDKDST[1], row, "0", true);

                        //Codice aereoporto destinazione
                        line.DKDAE = "001";
                        Insert(line.DKDAE, line.idxDKDAE[0], line.idxDKDAE[1], row, "0", true);
                        #endregion

                        //Codice merce
                        line.DKCNM = "";
                        Insert(line.DKCNM, line.idxDKCNM[0], line.idxDKCNM[1], row, " ", false);

                        //Codice mezzo
                        line.DKDNM = "";
                        Insert(line.DKDNM, line.idxDKDNM[0], line.idxDKDNM[1], row, " ", false);


                        line.DKBAN = ship.totalPallet.ToString();
                        Insert(line.DKBAN, line.idxDKBAN[0], line.idxDKBAN[1], row, "0", true);

                        //Da Vedere
                        line.DKPES = ship.grossWeight.ToString().Replace(",", String.Empty).Replace("0", String.Empty);
                        Insert(line.DKPES, line.idxDKPES[0], line.idxDKPES[1], row, "0", true);


                        line.DKCOL = ship.packs.ToString();
                        Insert(line.DKCOL, line.idxDKCOL[0], line.idxDKCOL[1], row, "0", true);

                        //Volume
                        line.DKVOL = "0".ToString().Replace(",", String.Empty);
                        Insert(line.DKVOL, line.idxDKVOL[0], line.idxDKVOL[1], row, "0", true);

                        //Fascia segnacolli
                        line.DKCOD = "1";
                        Insert(line.DKCOD, line.idxDKCOD[0], line.idxDKCOD[1], row, "0", true);

                        //Segnacollo dal
                        line.DKSED = "";
                        Insert(line.DKSED, line.idxDKSED[0], line.idxDKSED[1], row, "0", true);

                        //al Segnacollo
                        line.DKSEA = "";
                        Insert(line.DKSEA, line.idxDKSEA[0], line.idxDKSEA[1], row, "0", true);

                        //Segnacolli diversi - Impostato ad 1 se ci sono segnacolli al dettaglio
                        line.DKT01 = "1";
                        Insert(line.DKT01, line.idxDKT01[0], line.idxDKT01[1], row, "0", false);

                        //Importo contrassegno
                        line.DKIF1 = shDetail.cashValue.ToString().Replace(",", String.Empty); 
                        Insert(line.DKIF1, line.idxDKIF1[0], line.idxDKIF1[1], row, "0", true);

                        //Divisa contrassegno
                        line.DKDI1 = shDetail.cashCurrency;
                        Insert(line.DKDI1, line.idxDKDI1[0], line.idxDKDI1[1], row, " ", false);

                        //Importo anticipata
                        line.DKIF2 = "";
                        Insert(line.DKIF2, line.idxDKIF2[0], line.idxDKIF2[1], row, "0", true);

                        //Divisa anticipata
                        line.DKDI2 = "";
                        Insert(line.DKDI2, line.idxDKDI2[0], line.idxDKDI2[1], row, " ", false);


                        //N Fattura anticipata
                        line.DKFTA = "";
                        Insert(line.DKFTA, line.idxDKFTA[0], line.idxDKFTA[1], row, "0", true);

                        //Codice vettore anticipata
                        line.DKVE1 = "";
                        Insert(line.DKVE1, line.idxDKVE1[0], line.idxDKVE1[1], row, " ", false);

                        //Tot fattura x p/ass
                        line.DKITF = "";
                        Insert(line.DKITF, line.idxDKITF[0], line.idxDKITF[1], row, "0", true);

                        //Div fatt P/ass
                        line.DKDIF = "";
                        Insert(line.DKDIF, line.idxDKDIF[0], line.idxDKDIF[1], row, " ", false);

                        //Tipo fattura P/ass
                        line.DKTPA= "";
                        Insert(line.DKTPA, line.idxDKTPA[0], line.idxDKTPA[1], row, " ", false);

                        //N ° fattura P/ass
                        line.DKNRF = "";
                        Insert(line.DKNRF, line.idxDKNRF[0], line.idxDKNRF[1], row, "0", true);

                        //Data Fattura P/ass
                        line.DKDTF = "";
                        Insert(line.DKDTF, line.idxDKDTF[0], line.idxDKDTF[1], row, "0", false);

                        //Vettore ritiro
                        line.DKCDR = "";
                        Insert(line.DKCDR, line.idxDKCDR[0], line.idxDKCDR[1], row, " ", false);

                        //Data ritiro merce
                        line.DKDTR = "1";
                        Insert(line.DKDTR, line.idxDKDTR[0], line.idxDKDTR[1], row, "0", true);

                        //Tipo servizio // Specifica tabella
                        line.DKT02 = "";
                        Insert(line.DKT02, line.idxDKT02[0], line.idxDKT02[1], row, " ", false);

                        //Giorno di chiusura
                        line.DKCHI = "";
                        Insert(line.DKCHI, line.idxDKCHI[0], line.idxDKCHI[1], row, " ", false);

                        //Test giorno di chiusura
                        line.DKTCH= "";
                        Insert(line.DKTCH, line.idxDKTCH[0], line.idxDKTCH[1], row, " ", false);

                        //Data consegna tassativa
                        line.DKDCV = "";
                        Insert(line.DKDCV, line.idxDKDCV[0], line.idxDKDCV[1], row, " ", false);

                        //Tipo consegna
                        line.DKT03 = "";
                        Insert(line.DKT03, line.idxDKT03[0], line.idxDKT03[1], row, " ", false);

                        line.DKNOT = shDetail.publicNote.Trim();
                        Insert(line.DKNOT, line.idxDKNOT[0], line.idxDKNOT[1], row, " ", false);

                        content.Add(row.ToString()); 
                    }

                }
            }


            var tutteLeRighe = File.ReadAllLines("");
            List<string> csvContent = new List<string>();

            for (int i = 0; i < tutteLeRighe.Count(); i++)
            {
                var r = tutteLeRighe[i];

                var element = new TLI_EsitiIN();

                element.TBRMI = r.Substring(element.idxTBRMI[0], element.idxTBRMI[1]);
                element.TBDAT = r.Substring(element.idxTBDAT[0], element.idxTBDAT[1]);
                element.TBORA = r.Substring(element.idxTBORA[0], element.idxTBORA[1]);

                var DataConsenga = DateTime.ParseExact($"{element.TBDAT} {element.TBORA}", "yyyyMMdd HHmmss", null);
                var csvLine = $"{element.TBRMI.Trim()}/SH;{DataConsenga};30";

                csvContent.Add(csvLine);
            }

            return csvContent;
        }
    }
}
