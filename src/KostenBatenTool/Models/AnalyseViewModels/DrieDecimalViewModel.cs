using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class DrieDecimalViewModel
    {
        public int LijnId { get; set; }
        public int AnalyseId { get; set; }
        public IEnumerable<DrieDecimalLijstObjectViewModel> Lijst { get; set; }
        public decimal Veld1 { get; set; }
        public decimal Veld2 { get; set; }
        public decimal Veld3 { get; set; }
        public int BerekeningId { get; set; }

        public DrieDecimalViewModel()
        {

        }

        public DrieDecimalViewModel(Berekening berekening, int analyseId) : this()
        {
            BerekeningId = berekening.BerekeningId;
            AnalyseId = analyseId;
            string veld2 =
                berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AdministratieBegeleidingsKost")
                    ? "bruto maandloon begeleider"
                    : "bruto maandloon fulltime";
            string veld3 = berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AdministratieBegeleidingsKost")
                    ? "jaarbedrag"
                    : "totale loonkost per jaar";
            Lijst = berekening.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel(l, veld2, veld3)).ToList();
            LijnId = 0;
        }

        public DrieDecimalViewModel(Lijn lijn, Berekening berekening, int analyseId) : this(berekening, analyseId)
        {
            LijnId = lijn.LijnId;
            Veld1 = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren")).Value;
            Veld2 =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AdministratieBegeleidingsKost") ? "bruto maandloon begeleider" : "bruto maandloon fulltime")).Value;
            Veld3 = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals(berekening.GetType().ToString().Equals("KostenBatenTool.Models.Domain.AdministratieBegeleidingsKost") ? "jaarbedrag" : "totale loonkost per jaar")).Value;
        }
    }
}
