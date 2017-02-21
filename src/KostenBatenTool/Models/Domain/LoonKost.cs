using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public class LoonKost : Kost
    {
        #region Properties

        public decimal TotaleLoonKost { get; set; }
       
        #endregion

        #region Constructors

        public LoonKost(Analyse analyse)
        {
            Details.Add(new LoonKostDetail(analyse));

        }

        #endregion

        #region Methods
        
        public void BerekenTotaleKost()
        {
            TotaleLoonKost = Details.Cast<LoonKostDetail>().Sum(d => d.FunctieKost);
        }

        #endregion
    }
}
