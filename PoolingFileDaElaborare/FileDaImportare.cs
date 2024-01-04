using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public class FileDaImportare
    {
        public string PathCompleto { get; set; }
        public string NomeFileEXT { get
            {
                return Path.GetFileName(PathCompleto);
            } 
        }
        public string PathFile { get
            {
                try
                {
                    return Path.GetDirectoryName(PathCompleto).Split('\\')[4];
                }
                catch (Exception ee)
                {

                    return ee.Message;
                }
            } 
        }
        public string Tipo { get
            {
                try
                {
                    
                    return Path.GetExtension(PathCompleto);
                }
                catch (Exception ee)
                {

                    return ee.Message;
                }
                
            } 
        }
        public DateTime DataInserimento { get; set; }

        public int StatoEsecuzione { get; set; }
        public string MsgStato { get; set; }

    }
}
