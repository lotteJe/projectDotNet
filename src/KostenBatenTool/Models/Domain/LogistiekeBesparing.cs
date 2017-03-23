using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LogistiekeBesparing : Berekening
    {
        #region Constructors 
        public LogistiekeBesparing()
        {
            Velden.Add(new Veld("transportkosten jaarbedrag", typeof(decimal)));
            Velden.Add(new Veld("logistieke kosten jaarbedrag", typeof(decimal)));
            Velden.Add(new Veld("totaalbedrag", typeof(decimal)));
            VoegLijnToe(0);
        }
        #endregion
        #region Methods
        public override decimal BerekenResultaat()
        {
            return BerekenBedragPerLijn(0);
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            Lijnen[index].First(v => v.Key.Equals("totaalbedrag")).Value = (decimal) Lijnen[index].First(v => v.Key.Equals("transportkosten jaarbedrag")).Value + (decimal) Lijnen[index].First(v=> v.Key.Equals("logistieke kosten jaarbedrag")).Value;
            return (decimal) Lijnen[index].First(v => v.Key.Equals("totaalbedrag")).Value;
        }
        #endregion
    }
}
