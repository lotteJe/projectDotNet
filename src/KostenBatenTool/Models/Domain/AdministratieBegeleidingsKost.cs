using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class MedewerkerHogerNiveauBesparing : Berekening
    {

        #region Properties
        public Analyse Analyse { get; set; }
        #endregion

        #region Constructors
        public MedewerkerHogerNiveauBesparing(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add("uren", typeof(decimal));
            Velden.Add("bruto maandloon begeleider", typeof(decimal));
            Velden.Add("jaarbedrag", typeof(decimal));
            VoegLijnToe(0);
        }
        #endregion

        #region Methods
        public override decimal BerekenResultaat()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenBedragPerLijn(x)).ToList().Sum();
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            Lijnen[index]["jaarbedrag"] = (((decimal) Lijnen[index]["uren"])/36) 
                * (decimal) Lijnen[index]["bruto maandloon begeleider"] 
                * (1 + Analyse.Organisatie.PatronaleBijdrage)
                *12;
            return (decimal)Lijnen[index]["jaarbedrag"];
        }

        //public void ControleerVelden(int index)
        //{
        //    if (Lijnen[index]["bruto maandloon begeleider"] == null || Lijnen[index]["uren"] == null)
        //        throw new ArgumentException($"Velden op rij {index} zijn niet ingevuld!");
        //}
        #endregion
    }
}
