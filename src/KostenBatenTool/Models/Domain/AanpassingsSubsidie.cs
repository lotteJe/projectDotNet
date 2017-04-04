using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AanpassingsSubsidie : Berekening
    {
        #region Constructors

        public AanpassingsSubsidie()
        {
            Velden.Add(new Veld("jaarbedrag", typeof(decimal)));
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
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.Key.Equals("jaarbedrag")).Value; 
        }
        #endregion
    }
}
