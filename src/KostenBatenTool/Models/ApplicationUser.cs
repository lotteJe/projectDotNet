using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KostenBatenTool.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Email { get; set; }
        

        public string NaamOrganisatie { get; set; }

        public string Straat { get; set; }

        public string Huisnummer { get; set; }

        public string Postcode { get; set; }

        public string Gemeente { get; set; }
        //public Organisatie Organisatie { get; set; }
    }
}
