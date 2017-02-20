using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public abstract class Persoon
    {
        public string Naam { get;  set; }
        public string Voornaam { get;  set; }
        public string Email { get;  set; }

        public Persoon(string naam, string voornaam, string email){
            Naam = naam;
            Voornaam = voornaam;
            Email = email;
            }
    }
}
