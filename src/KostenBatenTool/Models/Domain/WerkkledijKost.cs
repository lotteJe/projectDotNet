﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class WerkkledijKost : Berekening
    {
        #region Constructors
        public WerkkledijKost()
        {
            Velden.Add(new Veld("type", typeof(String)));
            Velden.Add(new Veld("bedrag", typeof(decimal)));
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
            return (decimal)Lijnen[index].First(v => v.Key.Equals("bedrag")).Value;
        } 
        #endregion
    }
}
