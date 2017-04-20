using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class TypeBedragLijstObjectViewModel
    {
        public int LijnId { get; set; }
        public string Type { get; set; }
        public decimal Bedrag { get; set; }


        public TypeBedragLijstObjectViewModel(Lijn lijn)
        {
            LijnId = lijn.LijnId;
            Type = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString();
            Bedrag = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value;
        }
    }
}
