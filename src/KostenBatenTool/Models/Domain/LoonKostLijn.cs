using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKostLijn : Lijn
    {
        public Doelgroep Doelgroep { get; set; }

        public LoonKostLijn(List<Veld> velden) : base (velden)
        {
            
        }

        protected LoonKostLijn()
        {
            
        }
    }
}
