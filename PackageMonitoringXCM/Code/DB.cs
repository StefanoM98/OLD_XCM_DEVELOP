using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PackageMonitoringXCM.Code
{
    public class DB
    {
        public ContenitoriXCM db = new ContenitoriXCM();

        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void AggiungiRegistrazioneContenitore(int idCartone, long idDocumento)
        {
            try
            {
                var anaContenitori = db.ANAGRAFICA_CONTENITORI.First(x => x.TIPO_CONTENITORE == idCartone);

                var isPresente = db.RC_TEMPORANEA.FirstOrDefault(x => x.ID_DOCUMENTO == idDocumento && x.DESCRIZIONE_CONTENITORE == anaContenitori.DESCRIZIONE_CONTENITORE);

                if (isPresente != null)
                {
                    isPresente.QUANTITA_CONTENITORE++;
                    this.db.SaveChanges();
                    _logger.Info($"Aggiuntom {isPresente}");
                }
                else
                {
                    var nr = new RC_TEMPORANEA()
                    {
                        ID_DOCUMENTO = idDocumento,
                        DESCRIZIONE_CONTENITORE = anaContenitori.DESCRIZIONE_CONTENITORE,
                        TIPO_CONTENITORE = idCartone,
                        QUANTITA_CONTENITORE = 1
                    };
                    this.db.RC_TEMPORANEA.Add(nr);
                    this.db.SaveChanges();
                    _logger.Info($"Aggiunto {nr}");
                }
            }
            catch (Exception AggiungiRegistrazioneContenitore)
            {
                _logger.Error(AggiungiRegistrazioneContenitore);
                return;
            }
        }

        public IQueryable<RC_TEMPORANEA> RigheDaAggiornare(long idDocumento, string descrizioneContenitore)
        {
            return this.db.RC_TEMPORANEA.Where(x => x.ID_DOCUMENTO == idDocumento && x.DESCRIZIONE_CONTENITORE == descrizioneContenitore);
        }

        public void EliminaDocumentoDallaLista(long idDocumento)
        {
            try
            {
                var righeDaEliminare = this.db.RC_TEMPORANEA.Where(x => x.ID_DOCUMENTO == idDocumento).ToList();

                foreach (var r in righeDaEliminare)
                {
                    this.db.RC_TEMPORANEA.Remove(r);
                }
                this.db.SaveChanges();
            }
            catch (Exception EliminaDocumentoDallaLista)
            {
                _logger.Error(EliminaDocumentoDallaLista);
                return;
            }
        }

        public void SalvaRegistrazioneContenitori(long idDocumento, decimal larghezzaPallet, decimal altezzaPallet, decimal profonditaPallet, decimal pesoPallet)
        {
            try
            {
                var nuovoPallet = new PALLET()
                {
                    LARGHEZZA = larghezzaPallet,
                    ALTEZZA = altezzaPallet,
                    PROFONDITA = profonditaPallet,
                    PESO = pesoPallet
                };
                this.db.PALLET.Add(nuovoPallet);
                this.db.SaveChanges();

                var listaDaRegistrare = this.db.RC_TEMPORANEA.Where(x => x.ID_DOCUMENTO == idDocumento).ToList();


                foreach (var c in listaDaRegistrare)
                {
                    var contCorrID = this.db.ANAGRAFICA_CONTENITORI.First(x => x.TIPO_CONTENITORE == c.TIPO_CONTENITORE).ID_ANAGRAFICA_CONTENITORE;
                    var rc = new REGISTRAZIONE_CONTENITORE()
                    {
                        ID_DOCUMENTO = c.ID_DOCUMENTO,
                        ANAGRAFICA_CONTENITORE = contCorrID,
                        QUANTITA_CONTENITORE = c.QUANTITA_CONTENITORE,
                        ID_PALLET = nuovoPallet.ID_PALLET
                    };
                    this.db.REGISTRAZIONE_CONTENITORE.Add(rc);

                }
                this.db.SaveChanges();
                this.EliminaDocumentoDallaLista(idDocumento);

            }
            catch (Exception SalvaRegistrazioneContenitori)
            {
                _logger.Error(SalvaRegistrazioneContenitori);
                return;
            }
        }

        public void AggiungiParametroPallet(long idDocumento, decimal parametro, string tipoParametro)
        {
            var daAggiornare = this.db.RC_TEMPORANEA.Where(x => x.ID_DOCUMENTO == idDocumento).ToList();

            
            if (daAggiornare != null)
            {
                foreach(var c in daAggiornare)
                {
                    if (tipoParametro == "larghezza")
                    {
                        c.LARGHEZZA_PALLET = parametro;
                        this.db.SaveChanges();
                    }
                    else if (tipoParametro == "altezza")
                    {
                        c.ALTEZZA_PALLET = parametro;
                        this.db.SaveChanges();
                    }
                    else if (tipoParametro == "profondita")
                    {
                        c.PROFONDITA_PALLET = parametro;
                        this.db.SaveChanges();
                    }
                    else if (tipoParametro == "peso")
                    {
                        c.PESO_PALLET = parametro;
                        this.db.SaveChanges();
                    }
                }
            }
            else
            {
                return;
            }
        }

        public RC_TEMPORANEA OttieniParametriPallet(long idDocumento)
        {
            return this.db.RC_TEMPORANEA.FirstOrDefault(x => x.ID_DOCUMENTO == idDocumento);
        }
    }
}