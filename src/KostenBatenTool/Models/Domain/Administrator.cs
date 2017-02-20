using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Administrator : Persoon
    {
        #region Constructor

        public Administrator(string naam, string voornaam, string email):base(naam, voornaam, email)
        {

        }

        #endregion
    }
}
