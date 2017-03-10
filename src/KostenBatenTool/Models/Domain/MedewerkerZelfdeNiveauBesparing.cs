using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class MedewerkerZelfdeNiveauBesparing : Berekening
    {
        #region Properties
        public Analyse Analyse { get; set; }
        #endregion

        #region Constructors

        public MedewerkerZelfdeNiveauBesparing(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add("uren", typeof(decimal));
            Velden.Add("bruto maandloon fulltime", typeof(decimal));
            Velden.Add("totale loonkost per jaar", typeof(decimal));
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
            

                if (Analyse.Organisatie.UrenWerkWeek == 0)
                {
                    throw new ArgumentException("Uren werkweek van de organisatie mag niet 0 zijn!");
                }
                else
                {
                    Lijnen[index]["totale loonkost per jaar"] = ((decimal) Lijnen[index]["uren"]/
                                                                 Analyse.Organisatie.UrenWerkWeek)
                                                                *(decimal) Lijnen[index]["bruto maandloon fulltime"]
                                                                *(1 + Analyse.Organisatie.PatronaleBijdrage)* 13.92M;
                }
            

            return (decimal) Lijnen[index]["totale loonkost per jaar"];
        }
        #endregion
    }
}
