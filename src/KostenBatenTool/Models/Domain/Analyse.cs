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
        
        #endregion

        #region Constructors

        public Analyse(Organisatie organisatie)
        {
            Organisatie = organisatie;
            AanmaakDatum = DateTime.Now;
            Kosten = new List<Kost>();
            Baten = new List<Baat>();
            Kosten.Add(new LoonKost(this));
            Kosten.Add(new WerkkledijKost());
           
        }

        #endregion

        #region Methods

        
        public decimal BerekenNettoResultaat()
        {
            return BerekenBatenResultaat() - BerekenKostenResultaat();
        }

        public decimal BerekenKostenResultaat()
        {
            return Kosten.Select(k => k.BerekenResultaat()).Sum(); 
        }

        public decimal BerekenBatenResultaat()
        {
            return Baten.Select(b => b.BerekenResultaat()).Sum();
        }

        

        #endregion
    }
}
