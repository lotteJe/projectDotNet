using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class OverzichtViewModel
    {
        public decimal Kost1 { get; set; }
        public decimal Kost2 { get; set; }
        public decimal Kost3 { get; set; }
        public decimal Kost4 { get; set; }
        public decimal Kost5 { get; set; }
        public decimal Kost6 { get; set; }
        public decimal Kost7 { get; set; }
        public decimal Baat1 { get; set; }
        public decimal Baat2 { get; set; }
        public decimal Baat3 { get; set; }
        public decimal Baat4 { get; set; }
        public decimal Baat5 { get; set; }
        public decimal Baat6 { get; set; }
        public decimal Baat7 { get; set; }
        public decimal Baat8 { get; set; }
        public decimal Baat9 { get; set; }
        public decimal Baat10 { get; set; }
        public decimal Baat11 { get; set; }
        public int AnalyseId { get; set; }
        public Organisatie Organisatie { get; set; }
        public decimal SubTotaalKosten { get; set; }
        public decimal SubTotaalBaten { get; set; }
        public decimal Resultaat { get; set; }

        public OverzichtViewModel()
        {
            
        }

        public OverzichtViewModel(Analyse analyse)
        {
            AnalyseId = analyse.AnalyseId;
            Organisatie = analyse.Organisatie;
            SubTotaalBaten = analyse.BatenResultaat;
            SubTotaalKosten = analyse.KostenResultaat;
            Resultaat = analyse.Resultaat;
            Kost1 = analyse.Kosten[0].Resultaat;
            Kost2 = analyse.Kosten[1].Resultaat;
            Kost3 = analyse.Kosten[2].Resultaat;
            Kost4 = analyse.Kosten[3].Resultaat;
            Kost5 = analyse.Kosten[4].Resultaat;
            Kost6 = analyse.Kosten[5].Resultaat;
            Kost7 = analyse.Kosten[6].Resultaat;
            Baat1 = analyse.Baten[0].Resultaat;
            Baat2 = analyse.Baten[1].Resultaat;
            Baat2 = analyse.Baten[2].Resultaat;
            Baat4 = analyse.Baten[3].Resultaat;
            Baat5 = analyse.Baten[4].Resultaat;
            Baat6 = analyse.Baten[5].Resultaat;
            Baat7 = analyse.Baten[6].Resultaat;
            Baat8 = analyse.Baten[7].Resultaat;
            Baat9 = analyse.Baten[8].Resultaat;
            Baat10 = analyse.Baten[9].Resultaat;
            Baat11 = analyse.Baten[10].Resultaat;


        }





    }
}
