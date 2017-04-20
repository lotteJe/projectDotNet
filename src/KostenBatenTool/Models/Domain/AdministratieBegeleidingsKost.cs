using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AdministratieBegeleidingsKost : Berekening
    {

        #region Properties
        public Analyse Analyse { get; set; }
        #endregion

        #region Constructors

        protected AdministratieBegeleidingsKost()
        {
            
        }

        public AdministratieBegeleidingsKost(Analyse analyse)
        {
            Analyse = analyse;
            Velden.Add(new Veld("uren", typeof(decimal)));
            Velden.Add(new Veld("bruto maandloon begeleider", typeof(decimal)));
            Velden.Add(new Veld("jaarbedrag", typeof(decimal)));
            VoegLijnToe();
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
            Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value = 
                (((decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("uren")).Value)/36) 
                * (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bruto maandloon begeleider")).Value
                * (1 + Analyse.Organisatie.PatronaleBijdrage)
                * 12;
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value;
        }
        
        #endregion
    }
}
