using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class OmzetverliesBesparing : Berekening
    {
        #region Constructors
        protected OmzetverliesBesparing() { }
        public OmzetverliesBesparing(Analyse analyse)
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
            Resultaat = BerekenBedragPerLijn(0);
            return Resultaat;
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value = (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag omzetverlies")).Value 
                * (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("% besparing")).Value;
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("totaalbesparing")).Value;
        }
        #endregion
    }
}

