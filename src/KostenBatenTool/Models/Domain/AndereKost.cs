using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AndereKost : Berekening
    {
        #region Constructors
        protected AndereKost() { }
        public AndereKost(Analyse analyse)
        {
            Velden.Add(new Veld("type", typeof(String)));
            Velden.Add(new Veld("bedrag", typeof(decimal)));
            VoegLijnToe(0);
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
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("bedrag")).Value;
        }
        #endregion
    }
}
