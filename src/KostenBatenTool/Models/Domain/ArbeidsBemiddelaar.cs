using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models
{
    public class ArbeidsBemiddelaar : Persoon
    {
        //public Organisatie eigenOrganisatie { get; private set; } ?? hoort bij analyse, niet jobcoach
        public IEnumerable<Analyse> Analyses { get; private set; }
        public string NaamOrganisatie { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }


        public ArbeidsBemiddelaar(string naam, string voornaam, string email, string naamOrganisatie, string straat, string huisnummer, int postcode, string gemeente) :base(naam,voornaam, email)
        {

            Analyses = new List<Analyse>();
            NaamOrganisatie = naamOrganisatie;
            Straat = straat;
            Huisnummer = huisnummer;
            Postcode = postcode;
            Gemeente = gemeente;

        }
    }
}
