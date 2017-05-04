using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class ResultaatViewModel
    {
        [Required(ErrorMessage = "E-mailadres is verplicht")]
        public string EmailContactpersoon { get; set; }

        public string Bericht { get; set; }
        public int OrganisatiId { get; set; }

        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "pdf")]
        public IFormFile Pdf { get; set; }

        public ResultaatViewModel()
        {

        }

        public ResultaatViewModel(Organisatie o, string voornaam, string naam)
        {
            EmailContactpersoon = o.Contactpersoon?.Email ?? "";
            OrganisatiId = o.OrganisatieId;
            Bericht =
                string.Format("Beste " + $"{o.Contactpersoon?.Voornaam}\n" + "\nIn bijlage vind u de analyse voor " +
                              $"{o.Naam} te {o.Gemeente}." + " \n \nHartelijke groet \n"+$"{voornaam} {naam}");
        }
    
}
}
