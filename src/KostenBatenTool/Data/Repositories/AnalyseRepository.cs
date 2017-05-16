using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class AnalyseRepository : IAnalyseRepository
    {
        private readonly DbSet<Analyse> _analyses;
        private readonly ApplicationDbContext _dbContext;

        public AnalyseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _analyses = _dbContext.Analyses;
        }

        public Analyse GetAnalyse(int analyseId)
        {
            Analyse analyse = _analyses.Include(a => a.Organisatie).ThenInclude(o => o.Contactpersoon)
                .Include(a => a.Baten).ThenInclude(b => b.Velden)
                .Include(a => a.Baten).ThenInclude(b => b.Lijnen).ThenInclude(l => l.VeldenWaarden)
                .Include(a => a.Kosten).ThenInclude(b => b.Velden)
                .Include(a => a.Kosten).ThenInclude(b => b.Lijnen).ThenInclude(l => l.VeldenWaarden)
                .FirstOrDefault(a => a.AnalyseId == analyseId);
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            ((LoonkostSubsidie)
                        analyse.Baten.First(b => b.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie")))
                    .Loonkost =
                (LoonKost)
                analyse.Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonKost"));
            return analyse;
        }

        public void SerialiseerVelden(Analyse analyse)
        {
            analyse.Kosten.ForEach(k => k.Serialiseer());
            analyse.Baten.ForEach(b => b.Serialiseer());
        }

        public void ZetAnalyseAfgewerkt(int analyseId)
        {
            Analyse analyse = _analyses.FirstOrDefault(a => a.AnalyseId == analyseId);
            analyse.Afgewerkt = true;

        }

        public void ZetAnalyseBewerkbaar(int analyseId)
        {
            Analyse analyse = _analyses.FirstOrDefault(a => a.AnalyseId == analyseId);
            analyse.Afgewerkt = false;
        }

        public void VerwijderAnalyse(int analyseId)
        {
            Analyse analyse = _analyses.FirstOrDefault(a => a.AnalyseId == analyseId);
            analyse.Verwijderd = true;
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
