using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class LoonkostSubsidie : Kost
    {
        #region Properties

        public LoonKost Loonkost { get; set; }
        #endregion

        #region Constructors

        public LoonkostSubsidie(LoonKost loonkost)
        {
            Loonkost = loonkost;
            Velden.Add("Totale loonkostsubsidie", typeof(decimal));
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
            if (index != 0)
            {
                throw new ArgumentException("Index moet 0 zijn");
            }

            Lijnen[index]["Totale loonkostsubsidie"] = Loonkost.BerekenResultaat() - Loonkost.BerekenTotaleLoonKost();
            return (decimal) Lijnen[index]["Totale loonkostsubsidie"];
        }
        #endregion
    }
}
