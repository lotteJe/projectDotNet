using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Organisatie
    {
        public Contactpersoon Contactpersoon { get; private set; }
        public IEnumerable<Afdeling> Afdelingen { get; private set; }
        public string Naam { get; private set; }
        public string Straat { get; private set; }
        public int Huisnummer { get; private set; }
        public int Postcode { get; private set; }
        public string Gemeente { get; private set; }
        public int GemiddeldAantalWerkuren { get; private set; }
        public int PatronaleBijdrage { get; private set; }
        public string Logo { get; private set; }
    }
}
