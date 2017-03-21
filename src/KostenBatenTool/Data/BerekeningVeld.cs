using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Data
{
    public class BerekeningVeld
    {
        #region Properties
        public int BerekeningVeldId { get; set; }
        public int BerekeningId { get; set;}
        public int LijnId { get; set; }
        public int VeldId { get; set; }
        #endregion

        #region Constructors

        protected BerekeningVeld()
        {
            
        }
        public BerekeningVeld(int berekeningId, int lijnId, int veldId)
        {
            BerekeningId = berekeningId;
            LijnId = lijnId;
            VeldId = veldId;
        }
        #endregion
    }
}
