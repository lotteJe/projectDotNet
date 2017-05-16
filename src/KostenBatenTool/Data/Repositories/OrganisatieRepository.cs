using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class OrganisatieRepository : IOrganisatieRepository
    {
        private readonly DbSet<Organisatie> _organisaties;
        private readonly ApplicationDbContext _dbContext;

        public OrganisatieRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _organisaties = dbContext.Organisaties;
        }

        public Organisatie GetOrganisatie(int organisatieId)
        {
            return _organisaties.Include(o => o.Contactpersoon).FirstOrDefault(o => o.OrganisatieId == organisatieId);
        }
    }
}
