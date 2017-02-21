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
        public ArbeidsBemiddelaar(string naam, string voornaam, string email, Organisatie organisatie):base(naam, voornaam, email)
        {
            Analyses = new List<Analyse>();
            EigenOrganisatie = organisatie;
        }
        #endregion

        #region Methods

        public void MaakNieuweAnalyse(Organisatie organisatie)
        {
            Analyse nieuweAnalyse = new Analyse(organisatie);
            Analyses.Add(nieuweAnalyse);
        }

        public IEnumerable<Organisatie> GeefAlleOrganisaties()
        {
            return Analyses.Select(a => a.Organisatie);
        }

        public IEnumerable<Analyse> SorteerAnalysesOpOrganisatie()
        {
            return Analyses.OrderBy(a => a.Organisatie.Naam);
        }

        public IEnumerable<Analyse> ZoekInAnalyes(String zoekterm)
        {
            IEnumerable<Analyse> analyses = Analyses.Where(a => a.Organisatie.Naam.Contains(zoekterm)
            || a.Organisatie.Gemeente.Contains(zoekterm)).ToList();

            if (!analyses.Any())
                throw new InvalidOperationException("Geen resultaten gevonden.");
            return Analyses;
        }

        #endregion

    }
}
