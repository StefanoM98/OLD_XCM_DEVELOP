using System;
using System.Linq;

namespace GreenPassValidator
{
    public class AccessiCustom
    {
        ControlloAccessiXCMEntities db = new ControlloAccessiXCMEntities();
        public long FK_UTENTE { get; set; }
        public string COGNOME { get; set; }
        public DateTime DATA_EVENTO { get; set; }
        public string NOME { get; set; }
        public DateTime ORA_ACCESSO { get; set; }
        public DateTime? ORA_USCITA { get; set; }
        public double? ORE_DI_LAVORO { get; set; }
        public DateTime ORA_ACCESSO_LAVORATIVA { get; set; }
        public DateTime? ORA_USCITA_LAVORATIVA { get; set; }
        public double? ORE_DI_LAVORO_NETTE { get; set; }
        public string GiornoDellaSettimana { get; set; }
        public double? Straordinario { get; set; }
        public double? Permessi { get; set; }

        public AccessiCustom ClonaAccessi(long y)
        {
            var ana = db.Anagrafica.First(x => x.ID_ANAGRAFICA == y);
            return new AccessiCustom()
            {
                COGNOME = ana.COGNOME,
                NOME = ana.COGNOME,
                DATA_EVENTO = DateTime.Now
            };
        }
    }


}