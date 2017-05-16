using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IAnalyseRepository
    {
        Analyse GetAnalyse(int analyseId);
        void SerialiseerVelden(Analyse analyse);
        void ZetAnalyseAfgewerkt(int analyseId);
        void ZetAnalyseBewerkbaar(int analyseId);
        void VerwijderAnalyse(int analyseId);
        void SaveChanges();
    }
}
