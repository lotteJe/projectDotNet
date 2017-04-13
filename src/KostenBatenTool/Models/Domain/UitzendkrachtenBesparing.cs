﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class UitzendkrachtenBesparing : Berekening
    {
        #region Constructors
        protected UitzendkrachtenBesparing() { }
        public UitzendkrachtenBesparing(Analyse analyse)
        {
            Velden.Add(new Veld("beschrijving", typeof(string)));
            Velden.Add(new Veld("jaarbedrag", typeof(decimal)));
            VoegLijnToe(0);
        }
        #endregion

        #region Methods
        public override decimal BerekenResultaat()
        {
            Resultaat = Enumerable.Range(0, Lijnen.Count).ToList().Select(x => BerekenBedragPerLijn(x)).ToList().Sum();
            return Resultaat;
        }

        public override decimal BerekenBedragPerLijn(int index)
        {
            ControleerIndex(index);
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value;
        }
        #endregion
    }
}
