using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AanpassingsKost : Berekening
    {
        #region Constructors
        public AanpassingsKost()
        {
            Velden.Add("type", typeof(String));
            Velden.Add("bedrag", typeof(decimal));
            VoegLijnToe(0);
        }
        #endregion

        #region Methods
        public override decimal BerekenResultaat()
        {
            return Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenBedragPerLijn(x)).ToList().Sum();
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            ControleerVelden(index);
            return (decimal)Lijnen[index]["bedrag"];
        }

        public void ControleerVelden(int index )
        {
            if (Lijnen[index]["bedrag"] == null)
            {
                throw new ArgumentException($"Veld op rij {index} is niet ingevuld!");
            }
        }
        #endregion

    }
}
