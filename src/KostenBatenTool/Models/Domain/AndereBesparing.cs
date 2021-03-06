﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class AndereBesparing : Berekening
    {
        #region Constructors
        protected AndereBesparing() { }

        public AndereBesparing(Analyse analyse)
        {
            Velden.Add(new Veld("beschrijving", typeof(string),1));
            Velden.Add(new Veld("jaarbedrag", typeof(decimal),2));
            //VoegLijnToe();
            Beschrijving = "Andere besparingen";
            Volgorde = 11;
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
            return (decimal) Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value;
        }
        #endregion
    }
}
