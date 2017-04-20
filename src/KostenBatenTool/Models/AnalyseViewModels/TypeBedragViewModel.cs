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
        public int LijnId { get; set; }
        public int AnalyseId { get; set; }
        public IEnumerable<TypeBedragLijstObjectViewModel> Lijst { get; set; }
        public string Type { get; set; }
        public decimal Bedrag { get; set; }

        public TypeBedragViewModel()
        {

        }

        public TypeBedragViewModel(Berekening berekening, int analyseId) : this()
        {
            AnalyseId = analyseId;
            Lijst = berekening.Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn)).ToList();
            LijnId = 0;
        }

        public TypeBedragViewModel(Lijn lijn, Berekening berekening, int analysId) : this(berekening, analysId)
        {
            LijnId = lijn.LijnId;
            Type = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString();
            Bedrag = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value;
        }

    }
}


