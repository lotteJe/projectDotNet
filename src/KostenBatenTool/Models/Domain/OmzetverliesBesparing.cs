using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class OmzetverliesBesparing : Berekening
    {
        #region Constructors

        public OmzetverliesBesparing()
        {
            Velden.Add("jaarbedrag omzetverlies", typeof(decimal));
            Velden.Add("% besparing", typeof(decimal));
            Velden.Add("totaalbesparing", typeof(decimal));
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
            Lijnen[index].First(v => v.Key.Equals("totaalbesparing")).Value = (decimal) Lijnen[index].First(v => v.Key.Equals("jaarbedrag omzetverlies")).Value 
                * (decimal) Lijnen[index].First(v => v.Key.Equals("% besparing")).Value;
            return (decimal)Lijnen[index].First(v => v.Key.Equals("totaalbesparing")).Value;
        }
        #endregion
    }
}

