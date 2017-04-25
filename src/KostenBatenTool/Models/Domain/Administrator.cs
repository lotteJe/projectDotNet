using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;

namespace KostenBatenTool.Models.Domain
{
    public class Administrator : Persoon
    {
        public string Paswoord { get; set; }
        #region Constructor

        public Administrator(string naam, string voornaam, string email):base(naam, voornaam, email)
        {
            
        }

        #endregion
    }
}
