using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Organisatie
    {
        
        
        #region Properties

        public Contactpersoon Contactpersoon { get; set; }
        public IEnumerable<Afdeling> Afdelingen { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
        public int Huisnummer { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public double UrenWerkWeek { get; set; }
        public double PatronaleBijdrage { get; set; }
        public string Logo { get; set; } 
        #endregion
    }
}
