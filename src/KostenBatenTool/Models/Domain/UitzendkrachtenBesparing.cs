﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class UitzendkrachtenBesparing : Berekening
    {
        #region Constructors

        public UitzendkrachtenBesparing()
        {
            Velden.Add("beschrijving", typeof(string));
            Velden.Add("jaarbedrag", typeof(decimal));
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
            return (decimal)Lijnen[index]["jaarbedrag"];
        }
        #endregion
    }
}