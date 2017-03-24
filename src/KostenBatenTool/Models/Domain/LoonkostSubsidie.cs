using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LoonkostSubsidie : Berekening
    {
        #region Properties

        public LoonKost Loonkost { get; set; }
        #endregion

        #region Constructors

        protected LoonkostSubsidie()
        {
            
        }
        public LoonkostSubsidie(LoonKost loonkost)
        {
            Loonkost = loonkost;
            Velden.Add(new Veld("Totale loonkostsubsidie", typeof(decimal)));
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
            if (index != 0)
            {
                throw new ArgumentException("Index moet 0 zijn");
            }

            Lijnen[index].First(v => v.Key.Equals("Totale loonkostsubsidie")).Value = Loonkost.BerekenResultaat() - Loonkost.BerekenTotaleLoonKost();
            return (decimal) Lijnen[index].First(v => v.Key.Equals("Totale loonkostsubsidie")).Value;
        }
        #endregion
    }
}
