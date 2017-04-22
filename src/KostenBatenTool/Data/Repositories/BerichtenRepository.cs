using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class BerichtenRepository : IBerichtenRepository
    {
        private readonly DbSet<Bericht> _berichten;
        private readonly ApplicationDbContext _dbContext;

        public BerichtenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _berichten = _dbContext.Berichten;
        }

        public List<Bericht> GeefBerichten()
        {
            return _berichten.ToList();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

    }
}
