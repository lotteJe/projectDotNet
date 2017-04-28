using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Organisatie
    {
        #region Fields
        private decimal _patronaleBijdrage = 0.35M;

        #endregion

        #region Properties

        public Contactpersoon Contactpersoon { get; set; }
        public string Afdeling { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public decimal UrenWerkWeek { get; set; } = 38M;
        public int OrganisatieId { get; set; }

        public decimal PatronaleBijdrage
        {
            get { return _patronaleBijdrage; }

            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentException("Patronale bijdrage moet tussen 0 en 1 liggen.");
                }
                else
                {
                    _patronaleBijdrage = value;
                }

            }
        }

        public string Logo { get; set; }

        #endregion

        protected Organisatie()
        {
            
        }
        #region Constructors

       
        public Organisatie(string naam, string straat, string huisnummer, string postcode, string gemeente)
        {
            Naam = naam;
            Straat = straat;
            Huisnummer = huisnummer;
            Postcode = postcode;
            Gemeente = gemeente;
            
        }

        #endregion





    }
}
