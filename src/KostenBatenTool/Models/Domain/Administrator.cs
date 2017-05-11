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
        public bool SuperAdmin { get; set; }
        public bool WachtwoordReset { get; set; }
        #region Constructor

        public Administrator(string naam, string voornaam, string email, bool superadmin):base(naam, voornaam, email)
        {
            SuperAdmin = superadmin;
        }

        #endregion
    }
}
