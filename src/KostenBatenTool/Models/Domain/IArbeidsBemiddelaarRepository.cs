using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IArbeidsBemiddelaarRepository
    {

        ArbeidsBemiddelaar GetArbeidsBemiddelaar(string emailadres);
        ArbeidsBemiddelaar GetArbeidsBemiddelaarMetAnalyses(string emailadres);
        void Add(ArbeidsBemiddelaar arbeidsBemiddelaar);
        void Delete(ArbeidsBemiddelaar arbeidsBemiddelaar);
        IEnumerable<Analyse> GetAllAnalysesVanArbeidsBemiddelaar(string email);
        void SaveChanges();
        Analyse GetLaatsteAnalyseVanArbeidsBemiddelaar(string email);
        IEnumerable<Organisatie> GetOrganisatiesVanArbeidsBemiddelaar(string email);
        IEnumerable<Analyse> ZoekAnalysesWerkgever(string email, string searchString);
        IEnumerable<Analyse> ZoekAnalysesGemeente(string email, string searchString);
        List<Analyse> GetAnalysesDashboard(string emailadres);

    }
}
