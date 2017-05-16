using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class ArbeidsBemiddelaarRepository : IArbeidsBemiddelaarRepository
    {
        private readonly DbSet<ArbeidsBemiddelaar> _arbeidsBemiddelaars;
        private readonly ApplicationDbContext _dbContext;

        public ArbeidsBemiddelaarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _arbeidsBemiddelaars = _dbContext.ArbeidsBemiddelaars;
        }

        public ArbeidsBemiddelaar GetArbeidsBemiddelaar(string emailadres)
        {
           return  _arbeidsBemiddelaars.Include(a => a.EigenOrganisatie).FirstOrDefault(a => a.Email.Equals(emailadres));
        }

        public ArbeidsBemiddelaar GetArbeidsBemiddelaarMetAnalyses(string emailadres)
        {
            return _arbeidsBemiddelaars.Include(a => a.EigenOrganisatie).Include(a => a.Analyses).FirstOrDefault(a => a.Email.Equals(emailadres));

        }

        public void Add(ArbeidsBemiddelaar arbeidsBemiddelaar)
        {
            _arbeidsBemiddelaars.Add(arbeidsBemiddelaar);
        }

        public void Delete(ArbeidsBemiddelaar arbeidsBemiddelaar)
        {
            _arbeidsBemiddelaars.Remove(arbeidsBemiddelaar);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        
        public IEnumerable<Organisatie> GetOrganisatiesVanArbeidsBemiddelaar(string emailadres)
        {
            if (!_arbeidsBemiddelaars.Include(a => a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Any())
            {
                return null;
            }
            return
                _arbeidsBemiddelaars.Include(a => a.Analyses).ThenInclude(an => an.Organisatie).First(a => a.Email.Equals(emailadres)).Analyses.Select(a => a.Organisatie).Distinct().ToList();
        }
        public IEnumerable<Analyse> GetAllAnalysesVanArbeidsBemiddelaar(string emailadres)
        {

            if (!_arbeidsBemiddelaars.Include(a => a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Any())
            {
                return null;
            }
            return _arbeidsBemiddelaars.Include(a => a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Where(a => !a.Verwijderd);
        }

        public IEnumerable<Analyse> ZoekAnalysesWerkgever(string emailadres, string searchString)
        {
            return _arbeidsBemiddelaars.Include(a => a.Analyses).ThenInclude(an => an.Organisatie).FirstOrDefault(a => a.Email.Equals(emailadres)).Analyses.Where(s => s.Organisatie.Naam.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) && !s.Verwijderd);
        }

        public IEnumerable<Analyse> ZoekAnalysesGemeente(string emailadres, string searchString)
        {
            return _arbeidsBemiddelaars.Include(a => a.Analyses).ThenInclude(an => an.Organisatie).FirstOrDefault(a => a.Email.Equals(emailadres)).Analyses.Where(s => s.Organisatie.Gemeente.ToLowerInvariant().Contains(searchString.ToLowerInvariant()) && !s.Verwijderd);
        }
        
        public Analyse GetLaatsteAnalyseVanArbeidsBemiddelaar(string email)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars
                .Include(a => a.Analyses).ThenInclude(an => an.Organisatie)
                .Include(a => a.Analyses).ThenInclude(an => an.Baten).ThenInclude(b => b.Velden)
                .Include(a => a.Analyses).ThenInclude(an => an.Kosten).ThenInclude(b => b.Velden)
                .Include(a => a.Analyses).ThenInclude(an => an.Baten).ThenInclude(b => b.Lijnen).ThenInclude(b => b.VeldenWaarden)
                .Include(a => a.Analyses).ThenInclude(an => an.Kosten).ThenInclude(b => b.Lijnen).ThenInclude(b => b.VeldenWaarden)
                .First(a => a.Email.Equals(email)).Analyses.OrderBy(a => a.AanmaakDatum).Last();
            //Deserialiseren
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            ((LoonkostSubsidie)analyse.Baten.First(b => b.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie"))).Loonkost = (LoonKost)analyse.Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonKost"));
            return analyse;
        }

        public List<Analyse> GetAnalysesDashboard(string emailadres)
        {
            if (!_arbeidsBemiddelaars.Include(a => a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Any())
            {
                return null;
            }
            return _arbeidsBemiddelaars.Include(a => a.Analyses).ThenInclude(an => an.Organisatie)
                .First(a => a.Email.Equals(emailadres)).Analyses.Where(a => !a.Afgewerkt && !a.Verwijderd).ToList();
        }
    }
}



