using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class DoelgroepRepository : IDoelgroepRepository
    {
        private readonly DbSet<Doelgroep> _doelgroepen;
       

        public DoelgroepRepository(ApplicationDbContext dbContext)
        {
           _doelgroepen = dbContext.Doelgroepen;
        }
        public List<Doelgroep> GetAll()
        {
            return _doelgroepen.ToList();
        }

        public Doelgroep GetBySoort(string soort)
        {
            return _doelgroepen.FirstOrDefault(d => d.Soort.Equals(soort));
        }

        public Doelgroep GetById(int id)
        {
            return _doelgroepen.FirstOrDefault(d => d.DoelgroepId == id);
        }
    }
}
