using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class OutsourcingBesparing : Berekening
    {
        #region Constructors
        protected OutsourcingBesparing() { }
        public OutsourcingBesparing(Analyse analyse)
        {
            Velden.Add(new Veld("beschrijving", typeof(string)));
            Velden.Add(new Veld("jaarbedrag", typeof(decimal)));
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
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("jaarbedrag")).Value;
        }
        #endregion
    }
}
