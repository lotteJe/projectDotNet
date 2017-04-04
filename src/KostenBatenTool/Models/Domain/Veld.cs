using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Veld
    {
        #region Properties
        public int VeldId { get; set; }
        public string Key { get; set; }
        public Object Value { get; set; }
        public string InternalValue { get; set; }
        #endregion

        #region Constructors

        protected Veld()
        {
            
        }
        public Veld(string key, Object value)
        {
            Key = key;
            Value = value;
        }
        #endregion

       
    }
}
