using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Contactpersoon : Persoon
    {
        public Contactpersoon(string naam, string voornaam, string email):base(naam,voornaam, email)
        {

        }
    }
}
