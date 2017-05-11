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

        public ArbeidsBemiddelaar GetBy(string emailadres)
        {
            return _arbeidsBemiddelaars.Include(a => a.EigenOrganisatie).Include(a => a.Analyses).FirstOrDefault(a => a.Email.Equals(emailadres));
        }

        public ArbeidsBemiddelaar GetArbeidsBemiddelaarVolledig(string email)
        {
            ArbeidsBemiddelaar ab = _arbeidsBemiddelaars.Include("Analyses.Organisatie.Contactpersoon").Include("Analyses.Baten.Velden").Include("Analyses.Kosten.Velden").Include("Analyses.Kosten.Lijnen.VeldenWaarden").Include("Analyses.Baten.Lijnen.VeldenWaarden").First(a => a.Email.Equals(email));
            foreach (Analyse analyse in ab.Analyses)
            {
                analyse.Kosten.ForEach(k => k.Deserialiseer());
                analyse.Baten.ForEach(b => b.Deserialiseer());
                ((LoonkostSubsidie)analyse.Baten.First(b => b.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie"))).Loonkost = (LoonKost)analyse.Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonKost"));
            }
            return ab;
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

        public Organisatie GetOrganisatie(string emailadres, int id)
        {
            return _arbeidsBemiddelaars.Include("Analyses.Organisatie.Contactpersoon").First(a => a.Email.Equals(emailadres)).Analyses.Select(a => a.Organisatie).FirstOrDefault(o => o.OrganisatieId == id);
        }

        public IEnumerable<Organisatie> GetOrganisaties(string emailadres)
        {
            if (!_arbeidsBemiddelaars.Include(a =>a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Any())
            {
                return null;
            } 
            return
                _arbeidsBemiddelaars.Include("Analyses.Organisatie").First(a => a.Email.Equals(emailadres)).Analyses.Select(a => a.Organisatie).ToList();
        }


        public IEnumerable<Analyse> GetAllAnalyses(string emailadres)
        {
            if (!_arbeidsBemiddelaars.Include(a => a.Analyses).First(a => a.Email.Equals(emailadres)).Analyses.Any())
            {
                return null;
            }
            return _arbeidsBemiddelaars.Include("Analyses.Organisatie").First(a => a.Email.Equals(emailadres)).Analyses;
        }

        public void SerialiseerVelden(Analyse analyse)
        {
            analyse.Kosten.ForEach(k => k.Serialiseer());
            analyse.Baten.ForEach(b => b.Serialiseer());
        }

        public Analyse GetAnalyse(string email, int id)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie")
                .Include("Analyses.Kosten.Velden")
                .Include("Analyses.Baten.Velden")
                .Include("Analyses.Kosten.Lijnen.VeldenWaarden")
                .Include("Analyses.Baten.Lijnen.VeldenWaarden")
                .First(a => a.Email.Equals(email)).Analyses.FirstOrDefault(a => a.AnalyseId == id);
            //Deserialiseren
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            ((LoonkostSubsidie)analyse.Baten.First(b => b.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie"))).Loonkost = (LoonKost)analyse.Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonKost"));
            return analyse;
        }

        public Analyse GetLaatsteAnalyse(string email)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie").Include("Analyses.Baten.Velden").Include("Analyses.Kosten.Velden").Include("Analyses.Kosten.Lijnen.VeldenWaarden").Include("Analyses.Baten.Lijnen.VeldenWaarden").First(a => a.Email.Equals(email)).Analyses.OrderBy(a => a.AanmaakDatum).Last();
            //Deserialiseren
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            ((LoonkostSubsidie)analyse.Baten.First(b => b.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie"))).Loonkost = (LoonKost)analyse.Kosten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonKost"));
            return analyse;
        }

        public void VerwijderAnalyse(Analyse analyse)
        {
            _dbContext.Analyses.Remove(analyse);
        }

        public void VerwijderLijn(Lijn lijn)
        {
            _dbContext.Lijnen.Remove(lijn);
        }
        
        public LoonKostLijn GetLoonKostLijn(int lijnId, List<Veld> velden)
        {
            LoonKostLijn lijn = _dbContext.Lijnen.Include(l => l.VeldenWaarden).OfType<LoonKostLijn>().Include(l => l.Doelgroep).FirstOrDefault(l => l.LijnId == lijnId);
            lijn.Deserialiseer(velden);
            return lijn;
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



