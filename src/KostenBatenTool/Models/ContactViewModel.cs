using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models
{
    public class ContactViewModel
    {

        [Required(ErrorMessage = "Onderwerp is verplicht")]
        [Display(Name = "Onderwerp")]
        public string Onderwerp { get; set; }

        [Required(ErrorMessage = "Omschrijving is verplicht")]
        [Display(Name = "Omschrijving")]
        public string Omschrijving { get; set; }
        
    }
}

