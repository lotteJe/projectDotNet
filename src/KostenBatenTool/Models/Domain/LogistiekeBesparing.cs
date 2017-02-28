using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LogistiekeBesparing : Kost
    {
        #region Constructors 
        public LogistiekeBesparing()
        {
            Velden.Add("transportkosten jaarbedrag", typeof(decimal));
            Velden.Add("logistieke kosten jaarbedrag", typeof(decimal));
            Velden.Add("totaalbedrag", typeof(decimal));
            VoegLijnToe(0);
        }
        #endregion
        #region Methods
        public override decimal BerekenResultaat()
        {
            return BerekenKostPerLijn(0);
        }

        public override decimal BerekenKostPerLijn(int index)
        {
            ControleerIndex(index);
            Lijnen[index]["totaalbedrag"] = (decimal) Lijnen[index]["transportkosten jaarbedrag"] + (decimal) Lijnen[index]["logistieke kosten jaarbedrag"];
            return (decimal) Lijnen[index]["totaalbedrag"];
        }
        #endregion
    }
}
