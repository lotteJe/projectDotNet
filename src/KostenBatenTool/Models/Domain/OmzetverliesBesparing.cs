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
            Velden.Add(new Veld("jaarbedrag omzetverlies", typeof(decimal)));
            Velden.Add(new Veld("% besparing", typeof(decimal)));
            Velden.Add(new Veld("totaalbesparing", typeof(decimal)));
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
            Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("totaalbesparing")).Value = (decimal) Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("jaarbedrag omzetverlies")).Value 
                * (decimal) Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("% besparing")).Value;
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("totaalbesparing")).Value;
        }
        #endregion
    }
}

