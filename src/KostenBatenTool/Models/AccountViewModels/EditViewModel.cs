using System.ComponentModel.DataAnnotations;
using KostenBatenTool.Models.Domain;
using KostenBatenTool.Models.ManageViewModels;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht")]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

       [Required(ErrorMessage = "E-mailadres is verplicht")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Naam van de organisatie is verplicht")]
        [Display(Name = "Naam organisatie")]
        public string NaamOrganisatie { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Straat")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Display(Name = "Huisnummer")]
        [RegularExpression(@"[1-9][0-9]*[a-zA-Z]?", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public string Huisnummer { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode")]
        [RegularExpression(@"[1-9][0-9]{3}", ErrorMessage = "Moet een getal tussen 1000 en 9999 zijn!")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }




        public EditViewModel()
        {

        }

        public EditViewModel(ArbeidsBemiddelaar a) : this()
        {
            Naam = a.Naam;
            Voornaam = a.Voornaam;
            Email = a.Email;
            NaamOrganisatie = a.EigenOrganisatie.Naam;
            Straat = a.EigenOrganisatie.Straat;
            Huisnummer = a.EigenOrganisatie.Huisnummer;
            Postcode = a.EigenOrganisatie.Postcode;
            Gemeente = a.EigenOrganisatie.Gemeente;
        }
    }
}

