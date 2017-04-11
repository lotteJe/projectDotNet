using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class LoonkostLijnViewModel
    {
        public int LijnId { get; set; }
        public string Functie { get; set; }
        public decimal UrenPerWeek { get; set; }
        public decimal BrutoMaandloon { get; set; }
        public Doelgroep Doelgroep { get; set; }
        public decimal Vop { get; set; }
        public decimal AantalMaanden { get; set; }
        public decimal Ibo { get; set; }

        public LoonkostLijnViewModel(Lijn lijn)
        {
            LijnId = lijn.LijnId;
            Functie = lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("functie")).Value.ToString();
            UrenPerWeek = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("uren per week")).Value;
            BrutoMaandloon =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bruto maandloon fulltime")).Value;
            Doelgroep = (Doelgroep) lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("doelgroep")).Value;
            Vop =
                (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("% Vlaamse ondersteuningspremie")).Value;
            AantalMaanden = (decimal)lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("aantal maanden IBO")).Value;
            Ibo =
               (decimal)
                    lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("totale productiviteitspremie IBO")).Value;
        }
    }
}
