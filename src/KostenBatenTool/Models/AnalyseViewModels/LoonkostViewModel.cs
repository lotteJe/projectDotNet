using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LoonkostViewModel
    {
        public int LijnId { get; set; }
        public string Functie { get; set; }
        public decimal UrenPerWeek { get; set; }
        public decimal BrutoMaandloon { get; set; }
        public Doelgroep Doelgroep { get; set; }
        public decimal Vop { get; set; }
        public decimal AantalMaanden { get; set; }
        public decimal Ibo { get; set; }
        public int AnalyseId { get; set; }
        public IEnumerable<LoonkostLijnViewModel> Lijnen { get; set; }

        public LoonkostViewModel()
        {

        }
        public LoonkostViewModel(LoonKost loonkost, int analyseId) : this()
        {
            AnalyseId = analyseId;
            Lijnen = loonkost.Lijnen.Select(lijn => new LoonkostLijnViewModel(lijn)).ToList();
            LijnId = loonkost.Lijnen.Count;
        }
    }
}
