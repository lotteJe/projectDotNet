using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class LijnRepository : ILijnRepository
    {
        private readonly DbSet<Lijn> _lijnen;
        private readonly DbSet<LoonKostLijn> _loonkostlijnen;
        private readonly ApplicationDbContext _dbContext;

        public LijnRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _lijnen = dbContext.Lijnen;
            _loonkostlijnen = dbContext.Loonkostlijnen;
        }

        public void VerwijderLijn(Lijn lijn)
        {
            _lijnen.Remove(lijn);
        }

        public LoonKostLijn GetLoonKostLijn(int lijnId, List<Veld> velden)
        {
            LoonKostLijn lijn = _lijnen.Include(l => l.VeldenWaarden).OfType<LoonKostLijn>().Include(l => l.Doelgroep).FirstOrDefault(l => l.LijnId == lijnId);
            lijn.Deserialiseer(velden);
            return lijn;
        }

        public LoonKostLijn GetLoonKostLijn(int lijnId)
        {
           return _lijnen.Include(l => l.VeldenWaarden).OfType<LoonKostLijn>().FirstOrDefault(l => l.LijnId == lijnId);

        }
        public IList<LoonKostLijn> GetLoonKostLijnen(int berekeningId, List<Veld> velden)
        {
            IEnumerable<int> lijnIds =
                _dbContext.Berekeningen.Include(b => b.Lijnen)
                    .FirstOrDefault(b => b.BerekeningId == berekeningId)
                    .Lijnen.Select(l => l.LijnId);
            List<LoonKostLijn> lijnen = _dbContext.Lijnen.Include(l => l.VeldenWaarden).OfType<LoonKostLijn>().Include(l => l.Doelgroep).Where(l => lijnIds.Contains(l.LijnId)).ToList();
            lijnen.ForEach(l => l.Deserialiseer(velden));
            return lijnen;
        }


    }
}
