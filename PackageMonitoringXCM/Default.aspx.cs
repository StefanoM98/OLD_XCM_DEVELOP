using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using PackageMonitoringXCM.Code;

namespace PackageMonitoringXCM
{
    public partial class FormLayout : System.Web.UI.Page
    {
        //RegistrazioneContenitore Registra = new RegistrazioneContenitore();
        //Logger _logger = new Logger();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        DB db = new DB();

        #region page_event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshData();
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            GestioneFocus();
        }
        #endregion

        #region button_event
        protected void saveButton_Click(object sender, EventArgs e)
        {
            var idDocumento = long.Parse(idDocumentoTextBox.Text.Trim());
            var larghezzaPallet = decimal.Parse(larghezzaTextBox.Text.Trim());
            var altezzaPallet = decimal.Parse(altezzaTextBox.Text.Trim());
            var profonditaPallet = decimal.Parse(profonditaTextBox.Text.Trim());
            var pesoPallet = decimal.Parse(pesoTextBox.Text.Trim());

            db.SalvaRegistrazioneContenitori(idDocumento, larghezzaPallet, altezzaPallet, profonditaPallet, pesoPallet);
            SvuotaCampi();
            RedirectResponse();
        }

        protected void resetButton_Click(object sender, EventArgs e)
        {
            if (idDocumentoTextBox.Text.Length > 0)
            {
                db.EliminaDocumentoDallaLista(long.Parse(idDocumentoTextBox.Text.Trim()));
                SvuotaCampi();
            }
            RedirectResponse();
        }
        #endregion

        #region textBox_event
        protected void customTextChanged(object sender, EventArgs e)
        {
            ASPxTextBox s = (ASPxTextBox)sender;
            
            if (s != null)
            {
                if (s.ID == idDocumentoTextBox.ID)
                {
                    Session["idDocumento"] = idDocumentoTextBox.Text.Trim();
                    try
                    {
                        var parametri = db.OttieniParametriPallet(long.Parse(idDocumentoTextBox.Text.Trim()));
                        if (parametri != null)
                        {
                            if (parametri.LARGHEZZA_PALLET != null)
                            {
                                Session["larghezza"] = parametri.LARGHEZZA_PALLET.ToString();
                                larghezzaTextBox.Text = parametri.LARGHEZZA_PALLET.ToString();
                            }
                            if (parametri.ALTEZZA_PALLET != null)
                            {
                                Session["altezza"] = parametri.ALTEZZA_PALLET.ToString();
                                altezzaTextBox.Text = parametri.ALTEZZA_PALLET.ToString();
                            }
                            if (parametri.PROFONDITA_PALLET != null)
                            {
                                Session["profondita"] = parametri.PROFONDITA_PALLET.ToString();
                                profonditaTextBox.Text = parametri.PROFONDITA_PALLET.ToString();
                            }
                            if (parametri.PESO_PALLET != null)
                            {
                                Session["peso"] = parametri.PESO_PALLET.ToString();
                                pesoTextBox.Text = parametri.PESO_PALLET.ToString();
                            }
                        }
                    }
                    catch (Exception IdDocumentoException)
                    {
                        _logger.Error(IdDocumentoException);
                        return;
                    }
                    
                }
                else if (s.ID == qrCodeTextBox.ID)
                {
                    Session["qrCodeText"] = qrCodeTextBox.Text.Trim();

                    try
                    {
                        var idCartone = int.Parse(qrCodeTextBox.Text.Split('|')[0]);

                        var idDocumento = long.Parse(idDocumentoTextBox.Text.Trim());

                        db.AggiungiRegistrazioneContenitore(idCartone, idDocumento);

                        if (qrCodeTextBox.Text.Length > 0)
                            qrCodeTextBox.Text = String.Empty;
                        Session["qrCodeIsValid"] = true;
                    }
                    catch (Exception qrCodeNonValido)
                    {
                        _logger.Error(qrCodeNonValido);
                        Session["qrCodeIsValid"] = false;
                        RedirectResponse();
                        return;
                    }

                }
                else if (s.ID == larghezzaTextBox.ID)
                {
                    if (IsDecimalFormat(larghezzaTextBox.Text.Trim()))
                    {
                        Session["larghezza"] = larghezzaTextBox.Text.Trim();
                        Session["validazioneLarghezza"] = true;
                        db.AggiungiParametroPallet(long.Parse(idDocumentoTextBox.Text.Trim()), decimal.Parse(larghezzaTextBox.Text.Trim()), "larghezza");                        
                    }
                    else
                    {
                        Session["validazioneLarghezza"] = false;
                    }

                }
                else if (s.ID == altezzaTextBox.ID)
                {
                    if (IsDecimalFormat(altezzaTextBox.Text.Trim()))
                    {
                        Session["altezza"] = altezzaTextBox.Text.Trim();
                        Session["validazioneAltezza"] = true;
                        db.AggiungiParametroPallet(long.Parse(idDocumentoTextBox.Text.Trim()), decimal.Parse(altezzaTextBox.Text.Trim()), "altezza");

                    }
                    else
                    {
                        Session["validazioneAltezza"] = false;
                    }


                }
                else if (s.ID == profonditaTextBox.ID)
                {
                    if (IsDecimalFormat(profonditaTextBox.Text.Trim()))
                    {
                        Session["profondita"] = profonditaTextBox.Text.Trim();
                        Session["validazioneProfondita"] = true;
                        db.AggiungiParametroPallet(long.Parse(idDocumentoTextBox.Text.Trim()), decimal.Parse(profonditaTextBox.Text.Trim()), "profondita");
                    }
                    else
                    {
                        Session["validazioneProfondita"] = false;
                    }

                }
                else if (s.ID == pesoTextBox.ID)
                {
                    if (IsDecimalFormat(pesoTextBox.Text.Trim()))
                    {
                        Session["peso"] = pesoTextBox.Text.Trim();
                        Session["validazionePeso"] = true;
                        db.AggiungiParametroPallet(long.Parse(idDocumentoTextBox.Text.Trim()), decimal.Parse(pesoTextBox.Text.Trim()), "peso");
                    }
                    else
                    {
                        Session["validazionePeso"] = false;
                    }
                }

                RedirectResponse();
            }

        }
        #endregion

        #region gridView_event
        protected void GridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                var idDoc2 = long.Parse(idDocumentoTextBox.Text);
                var desc = e.Keys[0].ToString();

                var daAggiornare = db.db.RC_TEMPORANEA.FirstOrDefault(x => x.ID_DOCUMENTO == idDoc2 && x.DESCRIZIONE_CONTENITORE == desc);
                
                if (daAggiornare != null)
                {
                    IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
                    enumerator.Reset();


                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Key.ToString() == "QUANTITA_CONTENITORE")
                        {
                            int qta = int.Parse(enumerator.Value.ToString());
                            daAggiornare.QUANTITA_CONTENITORE = qta;
                            db.db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception GridViewRowUpdating)
            {
                _logger.Error(GridViewRowUpdating);
            }
            finally
            {
                GridView1.CancelEdit();
                e.Cancel = true;
            }
        }
        #endregion

        #region helper
        protected void RedirectResponse()
        {
            Response.Redirect("Default.aspx");
        }

        protected void RefreshData()
        {
            idDocumentoTextBox.Text = (string)Session["idDocumento"];
            larghezzaTextBox.Text = (string)Session["larghezza"];
            altezzaTextBox.Text = (string)Session["altezza"];
            profonditaTextBox.Text = (string)Session["profondita"];
            pesoTextBox.Text = (string)Session["peso"];

            if (Session["qrCodeIsValid"] != null && !(bool)Session["qrCodeIsValid"])
            { 
                qrCodeTextBox.HelpText = "Attenzione: il cartone inserito non è valido";
                qrCodeTextBox.HelpTextStyle.ForeColor = System.Drawing.Color.Tomato;
            }
            ValidazioneCampiDecimali();
            GridView1.DataBind();
        }

        protected void GestioneFocus()
        {
            if (idDocumentoTextBox.Text.Length > 0)
            {
                if (larghezzaTextBox.Text.Length > 0)
                {
                    if (altezzaTextBox.Text.Length > 0)
                    {
                        if (profonditaTextBox.Text.Length > 0)
                        {
                            if (pesoTextBox.Text.Length > 0)
                            {
                                saveButton.Focus();
                            }
                            else
                            {
                                pesoTextBox.Focus();
                            }
                        }
                        else
                        {
                            profonditaTextBox.Focus();
                        }

                    }
                    else
                    {
                        altezzaTextBox.Focus();
                    }
                }
                else
                {
                    qrCodeTextBox.Focus();
                    if (GridView1.VisibleRowCount > 0)
                    {
                        idDocumentoTextBox.ReadOnly = true;
                    }
                }

            }
            else
            {
                idDocumentoTextBox.Focus();
            }

        }
        
        protected void SvuotaCampi()
        {

            idDocumentoTextBox.Text = String.Empty;
            Session["idDocumento"] = String.Empty;

            larghezzaTextBox.Text = String.Empty;
            Session["larghezza"] = String.Empty;

            altezzaTextBox.Text = String.Empty;
            Session["altezza"] = String.Empty;

            profonditaTextBox.Text = String.Empty;
            Session["profondita"] = String.Empty;

            pesoTextBox.Text = String.Empty;
            Session["peso"] = String.Empty;

            Session["qrCodeIsValid"] = true;
            qrCodeTextBox.BackColor = System.Drawing.Color.Empty;

            Session["validazioneLarghezza"] = true;
            larghezzaTextBox.HelpText = String.Empty;

            Session["validazioneAltezza"] = true;
            altezzaTextBox.HelpText = String.Empty;

            Session["validazioneProfondita"] = true;
            profonditaTextBox.HelpText = String.Empty;

            Session["validazionePeso"] = true;
            pesoTextBox.HelpText = String.Empty;

        }

        protected bool IsDecimalFormat(string input)
        {
            Decimal dummy;
            return Decimal.TryParse(input, out dummy);
        }

        protected void ValidazioneCampiDecimali()
        {
            string helpText = "Attenzione: il formato inserito non è corretto";

            if (Session["validazioneLarghezza"] != null && !(bool)Session["validazioneLarghezza"])
            {
                larghezzaTextBox.HelpText = helpText;
                larghezzaTextBox.HelpTextStyle.ForeColor = System.Drawing.Color.Tomato;
            }
            if (Session["validazioneAltezza"] != null && !(bool)Session["validazioneAltezza"])
            {
                altezzaTextBox.HelpText = helpText;
                altezzaTextBox.HelpTextStyle.ForeColor = System.Drawing.Color.Tomato;
            }
            if (Session["validazioneProfondita"] != null && !(bool)Session["validazioneProfondita"])
            {
                profonditaTextBox.HelpText = helpText;
                profonditaTextBox.HelpTextStyle.ForeColor = System.Drawing.Color.Tomato;
            }
            if (Session["validazionePeso"] != null && !(bool)Session["validazionePeso"])
            {
                pesoTextBox.HelpText = helpText;
                pesoTextBox.HelpTextStyle.ForeColor = System.Drawing.Color.Tomato;
            }
        }
        #endregion

    }
}