using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Newtonsoft.Json;
using PeterO.Cbor;
using ZXing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Diagnostics;
using System.Web;
using System.Runtime.Serialization.Json;
using AForge.Imaging.Filters;
using RestSharp;
using System.Net;
using ZXing.Common;
using ZBar;


namespace GreenPassValidator
{
    public partial class Form1 : Form
    {
        #region AttributiPrivati
        //HttpClient _clientHTTP = new HttpClient();

        //string GreenPassDemoMioScaduto = "HC1:6BFOXN%TS3DHPVO13J /G-/2YRVA.QKW83:BXG4CH23IRM*4 33HUVAD6MO6 NI4EFSYS:%OD3P9B9LGFIE9MIHJ6W48UK.GCY0H4P .G $4VYCDEBD0HX2JN*4CY035T395*CBVZ0K1HL$0VONYZ00OP748$NI4L6E QSO19N8XQ1./R2X57 9HQ1%*J$U9BO12/J4-R.YHB%ROYJO 9WVHEY5CY5ZIEQKERQ8IY1I$HH%U-Z9T3PN%2 NVV5TN%2LXKW/SS KN8TXP4SN0X6T:1JT 456L X4CZKHKB-43:C3D DBQ99Q9E$B ZJ83BWUSLC30DJV1J TT* 1NS4KCT5XK BFU*0O05QAT063F%CD 810H% 0R%0ZD5CC9T0H%%2762L*81WF66TSMDWTEMC5Q%F:1R$06UJ79X1OR3/92V7O2/IFVV8:3YRJ:UMJWP416FOBQZJ5G72GD1VJVGCKWI90RXSR9OM325:-F+4NW8H:30XOAK4";
        //string GreenPassDemoMioValido = "HC1:6BFOXN%TS3DHPVO13J /G-/2YRVA.QKW83:BXG4CH23IRM*4TBJG%BZEJI93-MPW$NLEEMJC7ZSC K1U7*$K3$OHBW24FAL86H0VOCIL8-TINPM Q5TM8*N9YJ26H02 U9ZI4Q5%H0WO8H%NKHK1JAF.7$BGR-S3D1PI0%BKL0H/2P8-9Q:GJ/CS-D1*2F*44.82:G-+D+ 2 5PW 2L/IA/9/:IPHN6D7LLK*2HG%89UVZ0LMWSCUHPVFNXUJRHQJA8RUEIAYQE*C2:JG*PEMN9FTIPPAAMI PQVW5/O10+HT+6SZ4RZ4E%5B/9BL59IE+P10T9NVPI$E.+0CPI2YUPGAIS7%UG.UI5XUN HFRMLNKNM8POCEUGP$I/XK$M8JJ16YBXN91M4U01$2G7EC%/C.MPOU01%F%Q7./5AV2T$RH/O65C*QLWOI7YPRSFL6H.38HTTZVM2VT6J6P%GYP20MC1DS10J8:2NZS.ZS %LA9LSW9-C3VC0+BE";

        //RestClient client = null;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice CaptureDevice;
        Mirror filterMirror = new Mirror(false, true);
        SaturationCorrection filterSaturation = new SaturationCorrection(-0.5f);
        BarcodeReader barcodeReader = new BarcodeReader();
        int blink = 0;
        Color toBlink = Color.PaleGreen;
        Color DefBackColor = Form1.DefaultBackColor;
        bool checkPresenzeDaily = false;
        string fileNotifiche = Properties.Settings.Default.FullPathNotifichePresenze;
        DateTime LastCheck = DateTime.MaxValue;
        DateTime Oggi = DateTime.Now.Date;
        //#if DEBUG
        //        string APIPort = "44370";
        //        string APIAddress = "https://localhost";
        //#else
        //        string APIPort = "9443";
        //        string APIAddress = "https://192.168.2.254";
        //#endif
        #endregion

        #region ctor
        public Form1()
        {
            InitializeComponent();
            checkPresenzeDaily = ValutaSeGiaNotificato();
#if DEBUG
            comboBoxDevice.Visible = true;
#endif
        }

        private bool ValutaSeGiaNotificato()
        {
            try
            {
                if (!File.Exists(fileNotifiche))
                {
                    var tt = File.Create(fileNotifiche);
                    tt.Flush();
                    tt.Close();

                    return false;
                }
                else
                {
                    var _file = File.ReadAllLines(fileNotifiche);
                    if (_file.Count() > 1)
                    {
                        File.Delete(fileNotifiche);
                        var tt = File.Create(fileNotifiche);
                        tt.Flush();
                        File.WriteAllText(fileNotifiche, $"{DateTime.Now.Date}|true");
                        return true;
                    }
                    else if (_file.Count() == 0)
                    {
                        File.WriteAllText(fileNotifiche, $"{DateTime.Now.Date}|false");
                        return false;
                    }
                    else
                    {
                        var contenuto = _file[0].Split('|');

                        var data = DateTime.Parse(contenuto[0]);
                        var notifica = bool.Parse(contenuto[1]);

                        if (data < DateTime.Now.Date)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                GestoreMail.SegnalaErroreDev(ee);
                return false;
            }

        }
        #endregion

        object semaphoro = new object();
        #region Timer
        private void timerRefreshCamer_Tick(object sender, EventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                
                var img = (Bitmap)pictureBox1.Image;

                /*Bitmap QRCropped = IndividuaQRCodeNellImmagine(img);*/
                Result resultr = null;
                try
                {
                    //filterSaturation.ApplyInPlace(img);
                    resultr = barcodeReader.Decode(img);
                    if (resultr != null || resultr.BarcodeFormat == BarcodeFormat.QR_CODE)
                    {
                        Console.Beep(1000, 350);

                        Task.Factory.StartNew(delegate { GestisciLetturaQRCode(resultr); }).ContinueWith(delegate { FinalizzaGraficaAFineLettura(); });

                        //timerRefreshCamera.Stop();
                    }
                }
                catch
                {
                    //Console.Beep(500, 350);
                }
                finally
                {
                    //timerRefreshCamera.Start();
                }
            }

        }
        private void IndividuaQRCodeNellImmagine(Bitmap img)
        {
            using (ImageScanner scanner = new ImageScanner())
            {
                scanner.Cache = false;
                List<Symbol> symbols = scanner.Scan(img);
                foreach (Symbol symbol in symbols)
                    Console.WriteLine("\t" + symbol.ToString());
            }

        }
        private void timerOraRilevamento_Tick(object sender, EventArgs e)
        {

            labelControlOrarioPC.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void timerBlink_Tick(object sender, EventArgs e)
        {
            labelRisultatoScansione.Text = TestoLabelRisultato;
            if (blink < 2)
            {
                blink++;
                if (this.BackColor != toBlink)
                {
                    this.BackColor = toBlink;
                    if (!labelRisultatoScansione.Visible)
                    {
                        labelRisultatoScansione.Visible = true;
                    }
                }
                else
                {
                    this.BackColor = DefBackColor;
                }
            }
            else
            {
                this.BackColor = DefBackColor;
                blink = 0;
                if (labelRisultatoScansione.Visible)
                {
                    labelRisultatoScansione.Visible = false;
                }
                TimerBlink.Stop();
            }

        }
        #endregion

        #region Metodi di verifica
        private Anagrafica AnagraficaDaGreenPass(string qrString, out bool isOspite)
        {
            isOspite = false;
            byte[] tempCbor = Base45.Decode(Encoding.ASCII.GetBytes(qrString.Substring(4).Replace("\r\n", "")));
            var tt = Unzip(tempCbor);

            CBORObject cbor = CBORObject.DecodeFromBytes(tt, CBOREncodeOptions.Default);
            CBORObject cbor0 = CBORObject.DecodeFromBytes(cbor[0].GetByteString());
            CBORObject cbor2 = CBORObject.DecodeFromBytes(cbor[2].GetByteString());

            string jsonTemp = cbor2.ToJSONString();
            string res = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(jsonTemp), Formatting.Indented);

            Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            var greenObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json["-260"].ToString());
            string tttt = greenObj["1"].ToString().Replace("\r\n", "");
            var jj = JsonConvert.DeserializeObject<_1>(tttt);

            var COGNOME = jj.nam.fn;
            var NOME = jj.nam.gn;
            var DATA_NASCITA = DateTime.Parse(jj.dob);

            var db = new ControlloAccessiXCMEntities();
            var utenteInVerifica = db.Anagrafica.FirstOrDefault(x => x.COGNOME == COGNOME && x.NOME == NOME && x.DATA_NASCITA == DATA_NASCITA);

            if (utenteInVerifica == null)
            {
                isOspite = true;
                utenteInVerifica = new Anagrafica()
                {
                    ID_ANAGRAFICA = db.Anagrafica.First(x => x.COGNOME == "OSPITE").ID_ANAGRAFICA,
                    NOME = NOME,
                    COGNOME = COGNOME,
                    DATA_NASCITA = DATA_NASCITA,
                    LIVELLO = 2
                };
                TestoLabelRisultato = $"Benvenuto in azienda";
            }

            return utenteInVerifica;
        }
        private bool RegistraEvento(bool isIngresso, Anagrafica ana)
        {
            try
            {
                var db = new ControlloAccessiXCMEntities();
                if (isIngresso)
                {
                    var ne = new Accessi
                    {
                        DATA_VERIFICA = DateTime.Now,
                        FK_UTENTE = ana.ID_ANAGRAFICA,
                        ORA_INGRESSO = DateTime.Now,
                    };
                    db.Accessi.Add(ne);
                    db.SaveChanges();
                }
                else
                {
                    var tt = db.Accessi.ToList();
                    var regUscita = db.Accessi.First(x => x.FK_UTENTE == ana.ID_ANAGRAFICA && x.ORA_INGRESSO.Day == DateTime.Now.Day && x.ORA_INGRESSO.Month == DateTime.Now.Month && x.DATA_VERIFICA.Year == DateTime.Now.Year).ORA_USCITA = DateTime.Now;

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show("Errore inserimento evento", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        private void GestisciEventoAccesso(Anagrafica AnagraficaAccesso, bool isOspite)
        {
            //lock (semaphoro)
            {
                var db = new ControlloAccessiXCMEntities();
                if (!isOspite && AnagraficaAccesso != null)
                {
                    var ana = db.Anagrafica.FirstOrDefault(x => x.ID_ANAGRAFICA == AnagraficaAccesso.ID_ANAGRAFICA);
                    if (ana != null)
                    {
                        if (!ana.ENABLED)
                        {
                            labelRisultatoScansione.Text = "Accesso Revocato";
                            labelRisultatoScansione.BackColor = Color.Tomato;
                            TimerBlink.Start();
                            GestoreMail.SegnalaErroreDev($"Accesso revocato", $"Si segnala che {ana.NOME} {ana.COGNOME} ha tentato l'accesso ma esso risulta revocato a DB");
                        }
                        var utenteEntrato = db.Accessi.FirstOrDefault(x => x.FK_UTENTE == ana.ID_ANAGRAFICA && x.DATA_VERIFICA.Day == DateTime.Now.Day && x.DATA_VERIFICA.Month == DateTime.Now.Month && x.DATA_VERIFICA.Year == DateTime.Now.Year);
                        if (utenteEntrato != null)
                        {
                            if (DateTime.Now - utenteEntrato.DATA_VERIFICA < TimeSpan.FromHours(2))
                            {
                                TestoLabelRisultato = $"Accesso già registrato {AnagraficaAccesso.COGNOME} {AnagraficaAccesso.NOME}";
                                return;
                            }

                            if (ana.LIVELLO > 1 && ana.LIVELLO != 3)
                            {
                                RegistraEvento(false, ana);
                            }
                            TestoLabelRisultato = $"Arrivederci {AnagraficaAccesso.COGNOME} {AnagraficaAccesso.NOME}";
                        }
                        else
                        {
                            RegistraEvento(true, AnagraficaAccesso);
                            TestoLabelRisultato = $"Benvenuto {AnagraficaAccesso.COGNOME} {AnagraficaAccesso.NOME}";
                        }
                    }
                }
                else
                {
                    //RegistraEvento(true, AnagraficaAccesso);
                }
            }

        }
        private Anagrafica AnagraficaDaQR(string v, bool isXCMQR, out bool isOspite)
        {
            var utenteInVerifica = new Anagrafica();

            if (isXCMQR)
            {
                isOspite = false;
                var specQR = v.Split('|');
                utenteInVerifica = new Anagrafica()
                {
                    ID_ANAGRAFICA = long.Parse(specQR[1]),
                    COGNOME = specQR[2],
                    NOME = specQR[3],
                    CODICE_FISCALE = specQR[4],
                };
            }
            else
            {
                isOspite = true;
                utenteInVerifica = null;//AnagraficaDaGreenPass(v, out isOspite);
            }
            return utenteInVerifica;
        }
        #endregion

        #region Eventi Form
        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CaptureDevice == null) { return; }
            else if (CaptureDevice.IsRunning)
            {
                CaptureDevice.Stop();
            }

            CaptureDevice.NewFrame += CaptureDevice_NewFrame;
            CaptureDevice.Start();

            //System.Threading.Thread.Sleep(5000);
            //if (!CaptureDevice.IsRunning)
            //{
            //    CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBoxDevice.SelectedIndex].MonikerString);
            //    CaptureDevice.NewFrame += CaptureDevice_NewFrame;
            //    CaptureDevice.Start();

            //    System.Threading.Thread.Sleep(5000);

            //    if (!CaptureDevice.IsRunning)
            //    {
            //        Environment.Exit(1);
            //    }
            //}

        }
        private void Form1_Load(object sender, EventArgs e)
        {
#if DEBUG
            buttonTest.Visible = true;
            this.TopMost = false;

#endif
            //barcodeReader.AutoRotate = true;
            //barcodeReader.TryInverted = true;
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
            barcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            //barcodeReader.Options = new DecodingOptions { TryHarder = true };

            try
            {
                filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (FilterInfo filterInfo in filterInfoCollection)
                {
                    comboBoxDevice.Items.Add(filterInfo.Name);
                    comboBoxDevice.SelectedIndex = 0;
                }

                CaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBoxDevice.SelectedIndex].MonikerString);
                CaptureDevice.NewFrame += CaptureDevice_NewFrame;
                CaptureDevice.Start();
                timerRefreshCamera.Start();
            }
            catch (Exception)
            {
                MessageBox.Show($"CAM NON RILEVATA", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //timerRefreshCamera.Start();
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CaptureDevice == null) return;
            if (CaptureDevice.IsRunning)
            {
                CaptureDevice.Stop();
            }
        }
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                var capture = (Bitmap)eventArgs.Frame.Clone();

                //ContrastCorrection filter2 = new ContrastCorrection(15);
                //BrightnessCorrection filter3 = new BrightnessCorrection(-25);
                //ExtractBiggestBlob ff = new ExtractBiggestBlob();
                //filter2.ApplyInPlace(capture);
                //var tt = ff.Apply(capture);
                //filter3.ApplyInPlace(capture);
                //var gFilter = new GaussianBlur(2);
                //gFilter.ApplyInPlace(capture);

                filterMirror.ApplyInPlace(capture);
                pictureBox1.Image = capture;
            }
            catch (Exception ee)
            {
                Environment.Exit(0);
            }
        }

        #endregion

        #region Lettura QRCode
        private void GestisciLetturaQRCode(Result result)
        {
            try
            {
                var AnagraficaAccesso = new Anagrafica();
                var isXCMQR = result.ToString().StartsWith("XCM");

                AnagraficaAccesso = AnagraficaDaQR(result.ToString(), isXCMQR, out bool isOspite);


                if (!isXCMQR)
                {
                    labelRisultatoScansione.Text = "QR non riconoscito";
                    TimerBlink.Start();
                    return;
                    //bool GPValido = VerificaGreenPassAPIRest(result.ToString());
                    //if (true)
                    //{
                    //    GestoreMail.SendMailInvalidGreenPass(AnagraficaAccesso.NOME, AnagraficaAccesso.COGNOME, AnagraficaAccesso.DATA_NASCITA.ToString("dd/MM/yyyy"));
                    //}
                }

                GestisciEventoAccesso(AnagraficaAccesso, isOspite);
            }
            catch (Exception ee)
            {
                MessageBox.Show($"Errore in lettura QR\r\nSono supportati solo QR code Aziendali e Green Pass", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        string TestoLabelRisultato = "";
        private bool VerificaGreenPassAPIRest(string v)
        {

            var tttt = HttpUtility.UrlEncode(v).ToUpper();
            //Era 1.145
            var client = new RestClient($"https://192.168.1.1.4.5.:4777/Validation/bool?input={tttt}");
            client.Timeout = -1;
            client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            var resp = response.Content == "true";
            if (resp)
            {
                toBlink = Color.PaleGreen;
                TestoLabelRisultato = "Green Pass Valido";
            }
            else
            {
                toBlink = Color.Tomato;
                TestoLabelRisultato = "Green Pass NON Valido";
            }
            //AvviaTimer(TimerBlink);
            return resp;
        }
        private byte[] Unzip(byte[] data)
        {
            var outputStream = new MemoryStream();
            using (var compressedStream = new MemoryStream(data))
            using (var inputStream = new InflaterInputStream(compressedStream))
            {
                inputStream.CopyTo(outputStream);
                outputStream.Position = 0;

                return outputStream.ToArray();
            }
        }

        #endregion

        #region EventiFormInvoke               
        private void FinalizzaGraficaAFineLettura()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                TimerBlink.Start();
                timerRefreshCamera.Start();
            });

        }
        #endregion

        #region Test
        private void buttonStart_Click(object sender, EventArgs e)
        {
            //VerificaQR(GreenPassDemoMioScaduto);
            //VerificaQR(GreenPassDemoMioValido);


            //VerificabyHttpClient(GreenPassDemoMioScaduto);
            //VerificabyHttpClient(GreenPassDemoMioValido);
        }
        #endregion

        private void buttonTest_Click(object sender, EventArgs e)
        {

            pictureBox1.Load("immagineTest.jpg");
            timerCheckPresenzeDaily_Tick(null, null);
        }
        private void timerFullScreen_Tick(object sender, EventArgs e)
        {
            timerFullScreen.Stop();
            panel5.BringToFront();
            panel1.BringToFront();


            Task.Delay(5000);
            this.WindowState = FormWindowState.Maximized;
        }

        private void timerCheckPresenzeDaily_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Properties.Settings.Default.NotificheAttive)
                {
                    timerCheckPresenzeDaily.Stop();
                    timerCheckPresenzeDaily.Enabled = false;
                    return;
                }
                if (!checkPresenzeDaily && DateTime.Now.Hour >= 13)
                {

                    var db = new ControlloAccessiXCMEntities();
                    var anagraficheDipendenti = db.Anagrafica.Select(x => x).ToList();
                    var presenti = db.Accessi.Where(x => x.DATA_VERIFICA > Oggi).ToList();

                    List<Anagrafica> assenti = new List<Anagrafica>();

                    foreach (var p in anagraficheDipendenti)
                    {
                        if (!p.ENABLED)
                        {
                            continue;
                        }

                        if (p.ID_ANAGRAFICA == 8 || p.ID_ANAGRAFICA == 2 || p.ID_ANAGRAFICA == 4 || p.ID_ANAGRAFICA == 16) continue;
                        var ass = presenti.FirstOrDefault(x => x.FK_UTENTE == p.ID_ANAGRAFICA);
                        if (ass == null)
                        {
                            assenti.Add(p);
                        }
                    }
                    if (assenti.Count() > 0)
                    {
                        GestoreMail.SendReportAssenzeDigitali(assenti);
                    }
                    File.Delete(fileNotifiche);
                    File.WriteAllText(fileNotifiche, $"{DateTime.Now.Date}|true");
                    checkPresenzeDaily = true;

                }

            }
            catch (Exception ee)
            {
                GestoreMail.SegnalaErroreDev(ee);
                checkPresenzeDaily = false;
            }
        }

        private void timerRedLine_Tick(object sender, EventArgs e)
        {
            panel5.Visible = !panel5.Visible;
        }
    }
}
