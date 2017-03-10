using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Afdeling
    {
        #region Properties
        public string Naam { get; set; }
        public int AfdelingId { get; set; }
        public int OrganisatieId { get; set; }
        #endregion

        #region Constructors

        public Afdeling(string naam)
        {
            Naam = naam;
        }
        #endregion
    }
}
