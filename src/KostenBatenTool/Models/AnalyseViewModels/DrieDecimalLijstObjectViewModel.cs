using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class DrieDecimalLijstObjectViewModel
    {
        public int LijnId { get; set; }

        public decimal Veld1 { get; set; }
        public decimal Veld2 { get; set; }
        public decimal Veld3 { get; set; }
        public DrieDecimalLijstObjectViewModel(Lijn lijn, string veld2 , string veld3 )
        {
            LijnId = lijn.LijnId;
            Veld1 = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren")).Value;
            Veld2 = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld2)).Value;
            Veld3 = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(veld3)).Value;
        }
    }
}
