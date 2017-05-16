using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class BerekeningRepository : IBerekeningRepository
    {
        private readonly DbSet<Berekening> _berekeningen;
        private readonly ApplicationDbContext _dbContext;

        public BerekeningRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _berekeningen = dbContext.Berekeningen;
        }

        public Berekening GetBerekening(int berekeningId)
        {
            return _dbContext.Berekeningen.Include(b => b.Lijnen).FirstOrDefault(b => b.BerekeningId == berekeningId);
        }
    }
}
