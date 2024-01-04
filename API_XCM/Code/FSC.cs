using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using API_XCM.Models;
using API_XCM.Models.UNITEX;
using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace API_XCM.Code
{
    public class FSC
    {
        private Helper helper = new Helper();

        internal static Logger _loggerCode = LogManager.GetLogger("loggerCode");

        FTPClass _FTP = new FTPClass();

        string WorkDir = Path.Combine(@"C:\UnitexStorico\Clienti", "FSC");
        UNITEXEntities unitexDB = new UNITEXEntities();


        public List<InterpreteFSC> ParseShipments(List<InterpreteFSC> nuoveRighe)
        {
            try
            {
                List<InterpreteFSC> righePulite = PulisciBorderauFSC(nuoveRighe);

                List<InterpreteFSC> righeFinali = righePulite;
                //List<InterpreteFSC> righeFinali = AccorpaSpedizioniFSC(righePulite);
                if (righeFinali.Count > 0)
                {
                    ProduciCSV(righeFinali);
                }
                return righeFinali;
            }

            catch (Exception ParseShipmentsException)
            {
                _loggerCode.Error(ParseShipmentsException, ParseShipmentsException.Message);
                return new List<InterpreteFSC>();
            }

        }
        public List<InterpreteFSC> PulisciBorderauFSC(List<InterpreteFSC> righeNuovoBorderau)
        {
            var resp = new List<InterpreteFSC>();
            var dataUltimaRigaFSC = DammiUltimoInserimentoFSC();
            var ords = righeNuovoBorderau.OrderBy(x => x.DataStampaBorderau).ToList();
            foreach (var ship in ords)
            {
                var dt = DateTime.Parse(ship.DataStampaBorderau);
                resp.Add(ship);

                //res.Add(ship) era dentro questo IF per aggiornare il last time nel DB
                if (dt > dataUltimaRigaFSC)
                {
                }
            }
            return resp;
        }
        public List<InterpreteFSC> AccorpaSpedizioniFSC(List<InterpreteFSC> righePulite)
        {
            var resp = new List<InterpreteFSC>();

            var spedizioniCollettameFrigo = righePulite.Where(x => x.Colli > 0 && x.PresenzaFrigo.ToLower() == "true").ToList();
            var spedizioniPalletFrigo = righePulite.Where(x => x.Pallet > 0 && x.PresenzaFrigo.ToLower() == "true").ToList();

            foreach (var ship in spedizioniCollettameFrigo)
            {
                var isPresente = resp.FirstOrDefault(x => x.Indirizzo == ship.Indirizzo && x.Citta == ship.Citta && x.Pallet == 0);
                if (isPresente != null)
                {
                    isPresente.Colli += ship.Colli;
                    isPresente.Peso += ship.Colli * 2.0;
                    AggiornaDataAccorpataERiferimenti(isPresente, ship);
                }
                else
                {
                    ship.Carrier = "COLLO";
                    ship.TrasportType = "2-8";
                    ship.Peso = AggiornaPesoSpedizioneFSC(ship.Pallet, ship.Colli, ship.Peso);
                    resp.Add(ship);
                }
            }

            foreach (var ship in spedizioniPalletFrigo)
            {
                var isPresente = resp.FirstOrDefault(x => x.Indirizzo == ship.Indirizzo && x.Citta == ship.Citta && x.Colli == 0 && x.PresenzaFrigo.ToLower() == "true");

                if (isPresente != null)
                {
                    isPresente.Pallet += ship.Pallet;
                    isPresente.Peso += ship.Pallet * 40.0;
                    AggiornaDataAccorpataERiferimenti(isPresente, ship);
                }
                else
                {
                    ship.Carrier = "PLT";
                    ship.TrasportType = "2-8";
                    ship.Peso = AggiornaPesoSpedizioneFSC(ship.Pallet, ship.Colli, ship.Peso);
                    resp.Add(ship);
                }
            }

            var spedizioniCollettame = righePulite.Where(x => x.Colli > 0 && x.PresenzaFrigo.ToLower() == "false").ToList();

            var spedizioniPallet = righePulite.Where(x => x.Pallet > 0 && x.PresenzaFrigo.ToLower() == "false").ToList();

            foreach (var ship in spedizioniCollettame)
            {
                var isPresente = resp.FirstOrDefault(x => x.Indirizzo == ship.Indirizzo && x.Citta == ship.Citta && x.Pallet == 0 && x.PresenzaFrigo.ToLower() == "false");

                if (isPresente != null)
                {
                    isPresente.Colli += ship.Colli;
                    isPresente.Peso += ship.Colli * 2.0;
                    AggiornaDataAccorpataERiferimenti(isPresente, ship);
                }
                else
                {
                    ship.Carrier = "COLLO";
                    ship.Peso = AggiornaPesoSpedizioneFSC(ship.Pallet, ship.Colli, ship.Peso);
                    resp.Add(ship);
                }
            }

            foreach (var ship in spedizioniPallet)
            {
                var isPresente = resp.FirstOrDefault(x => x.Indirizzo == ship.Indirizzo && x.Citta == ship.Citta && x.Colli == 0 && x.PresenzaFrigo.ToLower() == "false");

                if (isPresente != null)
                {
                    isPresente.Pallet += ship.Pallet;
                    isPresente.Peso += ship.Pallet * 40.0;
                    AggiornaDataAccorpataERiferimenti(isPresente, ship);
                }
                else
                {
                    ship.Carrier = "PLT";
                    ship.Peso = AggiornaPesoSpedizioneFSC(ship.Pallet, ship.Colli, ship.Peso);
                    resp.Add(ship);
                }
            }

            return resp.OrderBy(x => x.DataStampaBorderau).ToList();
        }
        public void ProduciCSV(List<InterpreteFSC> righeFinali)
        {
            DateTime ultimoDTTinserito = DateTime.MinValue;
            List<string> righeCSV = new List<string>();

            try
            {
                foreach (var ship in righeFinali)
                {
                    var dt = DateTime.Parse(ship.DataStampaBorderau);

                    if (ultimoDTTinserito < dt)
                    {
                        ultimoDTTinserito = dt;
                    }

                    string newLine = $"{ship.DataStampaBorderau};{ship.NumeroDocumento};{ship.NomeClienteFornitore};{ship.Colli};{ship.Pallet};{ship.Peso};{ship.Citta};{ship.CAP};{ship.Indirizzo};{ship.Provincia};{ship.RiferimentoOrdine};{ship.Note};{ship.NumCell};{ship.Carrier};{ship.TrasportType}";
                    righeCSV.Add(newLine);

                }

                var fn = $"FSC_{DateTime.Now.ToString("yyyyMMdd_HHmmssffff")}.csv";
                if (!Directory.Exists(WorkDir))
                {
                    Directory.CreateDirectory(WorkDir);
                }
                var dest = Path.Combine(WorkDir, fn);

                var _fclient = _FTP.CreaClientFTP("unitexFSC", "FSCunitex");

                if (File.Exists(dest))
                {
                    File.Delete(dest);
                }
                File.WriteAllLines(dest, righeCSV);

                _fclient.UploadFile(dest, Path.Combine("IN", fn), FluentFTP.FtpRemoteExists.Overwrite);

                var dataUltimoDTT = unitexDB.LastDTTFSC.FirstOrDefault();

                if (dataUltimoDTT != null)
                {
                    dataUltimoDTT.DATE = ultimoDTTinserito;
                    unitexDB.SaveChanges();
                }
            }
            catch (Exception ProduciCSVException)
            {
                _loggerCode.Error(ProduciCSVException, ProduciCSVException.Message);
                return;
            }

        }
        private DateTime DammiUltimoInserimentoFSC()
        {
            return unitexDB.LastDTTFSC.FirstOrDefault().DATE;
        }
        public double AggiornaPesoSpedizioneFSC(int pallet, int colli, double peso)
        {
            var pesoFinale = peso;

            if (pesoFinale == 0)
            {
                if (pallet > 0)
                {
                    pesoFinale = pallet * 100.0;
                }
                else
                {
                    pesoFinale = colli * 2.0;
                }
            }
            return pesoFinale;
        }
        private void AggiornaDataAccorpataERiferimenti(InterpreteFSC ship, InterpreteFSC shipDaAccorpare)
        {
            var dtA = DateTime.Parse(ship.DataStampaBorderau);
            var dtB = DateTime.Parse(shipDaAccorpare.DataStampaBorderau);

            if (dtA < dtB)
            {
                ship.DataStampaBorderau = dtB.ToString("o");
            }
            ship.NumeroDocumento = $"{ship.NumeroDocumento}-{shipDaAccorpare.NumeroDocumento}";
        }

    }
}