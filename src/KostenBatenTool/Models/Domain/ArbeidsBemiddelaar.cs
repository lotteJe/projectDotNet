using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
    public class ArbeidsBemiddelaar : Persoon
    {
        #region Properties
        public Organisatie EigenOrganisatie { get; set; }
        public IList<Analyse> Analyses { get; private set; }
        #endregion
        
        #region Constructors

        public ArbeidsBemiddelaar()
        {
            
        }

        public ArbeidsBemiddelaar(string naam, string voornaam, string email, Organisatie organisatie):base(naam, voornaam, email)
        {

            Analyses = new List<Analyse>();
            EigenOrganisatie = organisatie;
        }
        #endregion

        #region Methods

        public void VoegNieuweAnalyseToe(Analyse analyse)
        {
            Analyses.Add(analyse);
        }
        #endregion

    }
}
