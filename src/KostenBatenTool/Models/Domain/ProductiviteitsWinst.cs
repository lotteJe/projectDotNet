﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class ProductiviteitsWinst : Berekening
    {
        #region Constructors
        protected ProductiviteitsWinst() { }

        public ProductiviteitsWinst(Analyse analyse)
        {
            Velden.Add(new Veld("jaarbedrag", typeof(decimal),1));
            VoegLijnToe();
            Beschrijving = "Productiviteitswinst";
            Volgorde = 7;
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
            ControleerIndex(index);
            return (decimal)Lijnen[index].VeldenWaarden.First(v => v.VeldKey.Equals("jaarbedrag")).Value;
        }
        #endregion
    }
}
