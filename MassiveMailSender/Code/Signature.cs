using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveMailSender.Code
{
    public class Signature
    {
        public static string _signature { get; set; }
        public static string firmaMailFileName = "FirmaMail.html";
        public static string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MassiveMailSender");
        public static bool changed = false;

        public static bool Exist()
        {
            return File.Exists(Path.Combine(appDataDir, firmaMailFileName));
        }

        public static void ReadFromFile()
        {
            _signature = File.ReadAllText(Path.Combine(appDataDir, firmaMailFileName));
        }

        public static void Save(string signature)
        {
            if (!string.IsNullOrEmpty(signature))
            {
                File.WriteAllText(Path.Combine(appDataDir, firmaMailFileName), signature);
                _signature = signature;
                changed = true;
            }
        }
    }
}
