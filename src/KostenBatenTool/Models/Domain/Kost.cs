using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models
{
    public abstract class Kost
    {
        #region Properties
        public string KorteOmschrijving { get; set; }
        public string Vraag { get; set; }
        public double Resultaat { get; set; }

        #endregion
        #region Methods
        public abstract double BerekenResultaat(); 
        #endregion
    }
}
