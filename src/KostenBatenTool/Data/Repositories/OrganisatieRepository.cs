using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class OrganisatieRepository : IOrganisatieRepository
    {
        private readonly DbSet<Organisatie> _organisaties;

        public OrganisatieRepository(ApplicationDbContext dbContext)
        {
            _organisaties = dbContext.Organisaties;

        }
        public Organisatie GetBy(string naam)
        {
            return _organisaties.SingleOrDefault(o => o.Naam == naam);
        }

        public IEnumerable<Organisatie> GetAll()
        {
            return _organisaties.ToList();
        }
    }
}
