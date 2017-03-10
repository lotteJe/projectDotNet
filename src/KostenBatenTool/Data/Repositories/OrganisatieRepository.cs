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
        private readonly ApplicationDbContext _dbContext;

        public OrganisatieRepository(ApplicationDbContext dbContext)
        {
            _organisaties = dbContext.Organisaties;
            _dbContext = dbContext;

        }
        public Organisatie GetBy(string naam)
        {
            return _organisaties.SingleOrDefault(o => o.Naam == naam);
        }

        public IEnumerable<Organisatie> GetAll()
        {
            return _organisaties.ToList();
        }

        public void Add(Organisatie organisatie)
        {
            _organisaties.Add(organisatie);
        }

        public void Delete(Organisatie organisatie)
        {
            _organisaties.Remove(organisatie);
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
