﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class OverurenBesparing : Kost
    {
        #region Constructors
        public OverurenBesparing()
        {
            Velden.Add("jaarbedrag", typeof(decimal));
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
            ControleerIndex(index);
            return (decimal)Lijnen[index]["jaarbedrag"];
        }
        #endregion
    }
}
