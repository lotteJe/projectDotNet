using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class DrieDecimalViewModel
    {
        public IEnumerable<DrieDecimalLijstObjectViewModel> Lijst { get; set; }
        public decimal Veld1 { get; set; }
        public decimal Veld2 { get; set; }
        public decimal Veld3 { get; set; }

    }
}
