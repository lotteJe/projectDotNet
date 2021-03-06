﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        [Display(Name = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Naam van de organisatie is verplicht.")]
        [Display(Name = "Naam organisatie")]
        public string NaamOrganisatie { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [Display(Name = "Straat")]
        public string Straat { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        [Display(Name = "Huisnummer")]
        [RegularExpression(@"\d{1,4}[[:alpha:]]{0,1}", ErrorMessage = "Moet een getal zijn, mag maximum 1 letter bevatten!")]
        public string Huisnummer { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht.")]
        [Display(Name = "Postcode")]
        [RegularExpression(@"[1-9][0-9]{3}", ErrorMessage = "Moet een getal tussen 1000 en 9999 zijn.")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht.")]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }
        }
}
