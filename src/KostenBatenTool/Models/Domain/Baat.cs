using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public abstract class Baat
    {
        #region Properties
        public decimal Resultaat { get; set; }

        #endregion
        #region Methods
        public abstract void BerekenResultaat();
        #endregion
    }
}
