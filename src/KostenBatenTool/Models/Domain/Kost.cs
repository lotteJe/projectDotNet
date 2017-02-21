using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public abstract class Kost
    {
        #region Properties

        public decimal Resultaat { get; set; }
        public ICollection<DetailKost> Details { get; set; }
        #endregion

        #region Constructors
        protected Kost()
        {
            Details = new List<DetailKost>();
        } 
        #endregion

        #region Methods

        public void BerekenResultaat()
        {
            Resultaat = Details.Sum(d => d.Bedrag);
        }

        public IEnumerable<DetailKost> GeefDetailKosten()
        {
            return Details;
        }
        #endregion
    }
}
