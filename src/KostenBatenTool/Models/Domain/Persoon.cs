using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public abstract class Persoon
    {
        #region Properties

        public string Naam { get;  set; }
        public string Voornaam { get;  set; }
        public string Email { get;  set; }
        public int PersoonID { get; set; }
        
        #endregion

        #region Constructors
        protected Persoon(string naam, string voornaam, string email)
        {
            Naam = naam;
            Voornaam = voornaam;
            Email = email;
        }

        #endregion


    }
}
