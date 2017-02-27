using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AndereKost : Kost
    {
        #region Constructors
        public AndereKost()
        {
            Velden.Add("type", typeof(String));
            Velden.Add("bedrag", typeof(decimal));
            VoegLijnToe(0);
        }
        #endregion

        #region Methods
        public override decimal BerekenResultaat()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenKostPerLijn(x)).ToList().Sum();
        }

        public override decimal BerekenKostPerLijn(int index)
        {
            return (decimal)Lijnen[index]["bedrag"];
        }
        #endregion
    }
}
