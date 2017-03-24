using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IArbeidsBemiddelaarRepository
    {

        ArbeidsBemiddelaar GetBy(string emailadres);
        void Add(ArbeidsBemiddelaar arbeidsBemiddelaar);
        void Delete(ArbeidsBemiddelaar arbeidsBemiddelaar);
        IEnumerable<Analyse> GetAllAnalyses(string email);
        void SaveChanges();
        void VerwijderVelden(Analyse analyse);
        void SerialiseerVelden(Analyse analyse);
        Analyse GetAnalyse(string email, int id);
    }
}
