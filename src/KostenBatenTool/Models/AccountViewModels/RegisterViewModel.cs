using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "Naam")]

        public string Naam { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Naam organisatie")]
        public string NaamOrganisatie { get; set; }

        [Required]
        [Display(Name = "Straat")]
        public string Straat { get; set; }

        [Required]
        [Display(Name = "Huisnummer")]
        [RegularExpression(@"[1-9][0-9]*[a-zA-Z]", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public string Huisnummer { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        [RegularExpression(@"[1-9][0-9]{3}", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public int Postcode { get; set; }

        [Required]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }

        
    }
}
