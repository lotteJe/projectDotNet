﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class OpleidingsKost : Berekening
    {
        #region Constructors
        protected OpleidingsKost() { }
        public OpleidingsKost(Analyse analyse)
        {
            Velden.Add(new Veld("type", typeof(String),1));
            Velden.Add(new Veld("bedrag", typeof(decimal),2));
            //VoegLijnToe();

            Beschrijving = " Extra kosten opleiding";
            Volgorde = 5;
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
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("bedrag")).Value;
        }
        #endregion

    }
}
