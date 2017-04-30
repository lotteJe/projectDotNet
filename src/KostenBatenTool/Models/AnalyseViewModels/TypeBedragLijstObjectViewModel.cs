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


        public TypeBedragLijstObjectViewModel(Lijn lijn, string veld1, string veld2)
        {
            LijnId = lijn.LijnId;
            Type = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld1)).Value.ToString();
            Bedrag = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld2)).Value;
        }
    }
}
