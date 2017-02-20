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
        public Organisatie eigenOrganisatie { get; private set; }
        public IEnumerable<Analyse> Analyses { get; private set; }

        public ArbeidsBemiddelaar(string naam, string voornaam, string email):base(naam,voornaam, email)
        {
            Analyses = new List<Analyse>();

        }
    }
}
