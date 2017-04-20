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
        [RegularExpression(@"[1-9][0-9]{3}", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }


        [Required(ErrorMessage = "Aantal werkuren is verplicht")]
        [Display(Name = "Aantal werkuren per week")]
        public decimal Werkuren { get; set; }


        [Required(ErrorMessage = "Bijdrage is verplicht")]
        [Display(Name = "Patronale bijdrage")]
        public decimal Bijdrage { get; set; }

        [Display(Name = "Afdeling")]
        public string Afdeling { get; set; }

        public int OrganisatieId { get; set; }
        public WerkgeverViewModel()
        {
            Werkuren = 38;
            Bijdrage = 35;
        }

        public WerkgeverViewModel(Organisatie o)
        {
            Werkuren = o.UrenWerkWeek;
            Bijdrage = o.PatronaleBijdrage;
            Afdeling = o.Afdeling;
            Gemeente = o.Gemeente;
            Postcode = o.Postcode;
            Huisnummer = o.Huisnummer;
            Straat = o.Straat;
            Naam = o.Naam;
            OrganisatieId = o.OrganisatieId;
        }

    }
}

