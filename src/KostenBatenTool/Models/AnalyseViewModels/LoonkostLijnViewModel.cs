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
        public string UrenPerWeek { get; set; }
        public decimal BrutoMaandloon { get; set; }
        public string Doelgroep { get; set; }
        public string Vop { get; set; }
        public string AantalMaanden { get; set; }
        public decimal Ibo { get; set; }

        public LoonkostLijnViewModel(Lijn lijn)
        {
            LijnId = lijn.LijnId;
            Functie = lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("functie")).Value.ToString();
            UrenPerWeek = lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("uren per week")).Value.ToString();
            BrutoMaandloon =
                Convert.ToDecimal(lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bruto maandloon fulltime")).Value);
            Doelgroep = lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("doelgroep")).Value.ToString();
            Vop =
                lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("% Vlaamse ondersteuningspremie")).Value.ToString();
            AantalMaanden = lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("aantal maanden IBO")).Value.ToString();
            Ibo =
                Convert.ToDecimal(
                    lijn.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("totale productiviteitspremie IBO")).Value);
        }
    }
}
