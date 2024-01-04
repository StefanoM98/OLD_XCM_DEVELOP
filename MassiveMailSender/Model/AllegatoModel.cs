
using System.Collections.Generic;
using System.IO;

namespace MassiveMailSender.Model
{
    public class Allegato
    {
        public static List<Allegato> listaAllegati = new List<Allegato>();
        public int id { get; set; }
        public string fullPath { get; set; }
        public string fileName
        {

            get
            {
                return Path.GetFileName(fullPath);
            }
        }

    }
}
