using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LoonkostViewModel
    {
        public string Functie { get; set; }
        public string UrenPerWeek { get; set; }
        public decimal BrutoMaandloon { get; set; }
        public string Doelgroep { get; set; }
        public string Vop { get; set; }
        public string AantalMaanden { get; set; }
        public decimal Ibo{ get; set; }
    }
}
