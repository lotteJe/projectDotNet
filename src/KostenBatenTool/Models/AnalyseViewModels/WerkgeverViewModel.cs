using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class WerkgeverViewModel
    {
        public int AnalyseId { get; set; }
        [Required(ErrorMessage = "Naam van de organisatie is verplicht")]
        [Display(Name = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Straat")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Display(Name = "Huisnummer")]
        [RegularExpression(@"\d{1,4}[[:alpha:]]{0,1}", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public string Huisnummer { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode")]
        [RegularExpression(@"[1-9][0-9]{3}[A-Za-z]{0,2}", ErrorMessage = "Moet een getal zijn, mag maximum 2 letters bevatten!")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }


        [Required(ErrorMessage = "Aantal werkuren is verplicht")]
        [RegularExpression("^[0-9][0-9]?([,][0-9]+)?$|^100$", ErrorMessage = "De waarde moet positief zijn, eventueel gescheiden door een komma.")]
        [Display(Name = "Aantal werkuren per week")]
        public string Werkuren { get; set; }

        [Required(ErrorMessage = "Bijdrage is verplicht")]
        [RegularExpression("^[0-9][0-9]?([,][0-9]+)?$|^100$", ErrorMessage = "De waarde moet tussen 0 en 100 liggen, eventueel gescheiden door een komma.")]
        [Display(Name = "Patronale bijdrage")]
        public string Bijdrage { get; set; }

        [Display(Name = "Afdeling")]
        public string Afdeling { get; set; }

        public string EmailContactpersoon { get; set; }

        public string NaamContactpersoon { get; set; }
        public string VoornaamContactpersoon { get; set; }

        public int OrganisatieId { get; set; }
        public WerkgeverViewModel()
        {
            Werkuren = "38";
            Bijdrage = "35";
        }

        public WerkgeverViewModel(Organisatie o)
        {
            Werkuren = string.Format("{0:00.##}", o.UrenWerkWeek);
            Bijdrage = string.Format("{0:00.##}", o.PatronaleBijdrage * 100);
            Afdeling = o.Afdeling;
            Gemeente = o.Gemeente;
            Postcode = o.Postcode;
            Huisnummer = o.Huisnummer;
            Straat = o.Straat;
            Naam = o.Naam;
            OrganisatieId = o.OrganisatieId;
            if (o.Contactpersoon != null)
            {
                EmailContactpersoon = o.Contactpersoon.Email;
                NaamContactpersoon = o.Contactpersoon.Naam;
                VoornaamContactpersoon = o.Contactpersoon.Voornaam;
            }
            else
            {
                EmailContactpersoon = "";
                NaamContactpersoon = "";
                VoornaamContactpersoon = "";
            }

        }
        public WerkgeverViewModel(Organisatie o, int analyseId) : this(o)
        {
            AnalyseId = analyseId;
        }
    }
}

