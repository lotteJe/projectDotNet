using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Analyse
    {
        #region Properties

        public ICollection<Kost> Kosten { get; set; }
        public ICollection<Baat> Baten { get; set; }
        public Organisatie Organisatie { get; set; }
        public DateTime AanmaakDatum { get; private set; }
        public decimal NettoResultaat { get; private set; }
        public decimal KostenResultaat { get; private set; }
        public decimal BatenResultaat { get; private set; }
        #endregion

        #region Constructors

        public Analyse(Organisatie organisatie)
        {
            Organisatie = organisatie;
            AanmaakDatum = DateTime.Now;
            Kosten = new List<Kost>();
            Kosten.Add(new LoonKost(this));
            Baten = new List<Baat>();
           
        }

        #endregion

        #region Methods

        public void BerekenNettoResultaat()
        {
            NettoResultaat = BatenResultaat - KostenResultaat;
        }

        public void BerekenKostenResultaat()
        {
            KostenResultaat = Kosten.Sum( k => k.Resultaat);
        }

        public void BerekenBatenResultaat()
        {
            BatenResultaat = Baten.Sum(b => b.Resultaat);
        }

        

        #endregion
    }
}
