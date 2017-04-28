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
        protected MedewerkerZelfdeNiveauBesparing() { }
        public MedewerkerZelfdeNiveauBesparing(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add(new Veld("uren", typeof(decimal)));
            Velden.Add(new Veld("bruto maandloon fulltime", typeof(decimal)));
            Velden.Add(new Veld("totale loonkost per jaar", typeof(decimal)));
            //VoegLijnToe();
        }
        #endregion


        #region Methods
        public override decimal BerekenResultaat()
        {
            Resultaat = Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenBedragPerLijn(x)).ToList().Sum();
            return Resultaat;

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
                    Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost per jaar")).Value = ((decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("uren")).Value/
                                                                 Analyse.Organisatie.UrenWerkWeek)
                                                                *(decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value
                                                                *(1 + Analyse.Organisatie.PatronaleBijdrage)* 13.92M;
                }
            

            return (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totale loonkost per jaar")).Value;
        }
        #endregion
    }
}
