﻿using System;
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
            Velden.Add(new Veld("Totale loonkostsubsidie", typeof(decimal),1));
            VoegLijnToe();

            Beschrijving = "Totale loonkostsubsidies (VOP, IBO en doelgroepvermindering";
            Volgorde = 1;
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
            if (index != 0)
            {
                throw new ArgumentException("Index moet 0 zijn");
            }

            Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("Totale loonkostsubsidie")).Value = Loonkost.BerekenResultaat() - Loonkost.BerekenTotaleLoonKost();
            return (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("Totale loonkostsubsidie")).Value;
        }
        #endregion
    }
}
