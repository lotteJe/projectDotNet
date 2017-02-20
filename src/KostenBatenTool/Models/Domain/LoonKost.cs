using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models
{
    public class LoonKost : Kost
    {
        #region Properties

        public double TotaleLoonKost { get; set; }
        

        #endregion
        public override double BerekenResultaat()
        {
            throw new NotImplementedException();
        }
    }
}
