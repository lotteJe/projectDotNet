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
        public int AnalyseId { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Gelieve een bestand te selecteren")]
        public IFormFile Pdf { get; set; }

        public ResultaatViewModel()
        {

        }

        public ResultaatViewModel(Organisatie o, string voornaam, string naam, int analyseId)
        {
            EmailContactpersoon = o.Contactpersoon?.Email ?? "";
            OrganisatiId = o.OrganisatieId;
            AnalyseId = analyseId;
            Bericht =
                string.Format("Beste " + $"{o.Contactpersoon?.Voornaam}\n" + "\nIn bijlage vindt u de analyse voor " +
                              $"{o.Naam} te {o.Gemeente}." + " \n \nHartelijke groet \n"+$"{voornaam} {naam}");
        }
    
}
}
