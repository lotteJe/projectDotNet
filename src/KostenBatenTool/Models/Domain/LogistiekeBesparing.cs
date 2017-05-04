using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LogistiekeBesparing : Berekening
    {
        #region Constructors 
        protected LogistiekeBesparing() { }

        public LogistiekeBesparing(Analyse analyse)
        {
            Velden.Add(new Veld("transportkosten jaarbedrag", typeof(decimal),1));
            Velden.Add(new Veld("logistieke kosten jaarbedrag", typeof(decimal),2));
            Velden.Add(new Veld("totaalbedrag", typeof(decimal),3));
            VoegLijnToe();
            Beschrijving = "Logistieke besparing";
            Volgorde = 10;
        }
        #endregion
        #region Methods
        public override decimal BerekenResultaat()
        {
            Resultaat = BerekenBedragPerLijn(0);
            return Resultaat;
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbedrag")).Value = (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("transportkosten jaarbedrag")).Value + (decimal) Lijnen[index].VeldenWaarden.First(v=> v.VeldKey.Equals("logistieke kosten jaarbedrag")).Value;
            return (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbedrag")).Value;
        }
        #endregion
    }
}
