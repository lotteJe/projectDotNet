using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class TypeBedragViewModel
    {
        public int AnalyseId { get; set; }
        public IEnumerable<TypeBedragLijstObjectViewModel> Lijst { get; set; }
        public string Type { get; set; }

        [RegularExpression("[1-9][0-9]*([,][0-9]+)?", ErrorMessage = "Moet een positief getal zijn.")]
        public decimal Bedrag { get; set; }

        public TypeBedragViewModel()
        {
            
        }

        public TypeBedragViewModel(int id) : this()
        {
            AnalyseId = id;
        }

       }
}
