using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            LijnId = 0;

        }
        public LoonkostViewModel(Lijn  lijn, LoonKost loonkost, int analyseId) : this(loonkost,analyseId)
        {
            LijnId = lijn.LijnId;
            Functie = lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("functie")).Value.ToString();
            UrenPerWeek = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren per week")).Value;
            BrutoMaandloon =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value;
            Doelgroep = (Doelgroep)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("doelgroep")).Value;
            Vop =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("% Vlaamse ondersteuningspremie")).Value;
            AantalMaanden = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("aantal maanden IBO")).Value;
            Ibo =
               (decimal)
                    lijn.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("totale productiviteitspremie IBO")).Value;
        }
    }
}
