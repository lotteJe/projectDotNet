using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class ArbeidsBemiddelaarRepository : IArbeidsBemiddelaarRepository
    {
        private readonly DbSet<ArbeidsBemiddelaar> _arbeidsBemiddelaars;
        private readonly DbSet<BerekeningVeld> _berekeningVelden;
        private readonly DbSet<Veld> _velden;
        private readonly DbSet<Analyse> _analyses;
        private readonly ApplicationDbContext _dbContext;

        public ArbeidsBemiddelaarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _arbeidsBemiddelaars = _dbContext.ArbeidsBemiddelaars;
            _berekeningVelden = dbContext.BerekeningVelden;
            _velden = dbContext.Velden;
            _analyses = dbContext.Analyses;
        }

        public ArbeidsBemiddelaar GetBy(string emailadres)
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

        public Organisatie GetOrganisatie(string emailadres, int id)
        {
            return _arbeidsBemiddelaars.Include("Analyses.Organisatie").First(a => a.Email.Equals(emailadres)).Analyses.Select(a => a.Organisatie).First(o => o.OrganisatieId == id);
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
            foreach (Berekening kost in analyse.Kosten)
            {
                //conversie van velden
                kost.Serialiseer();
                //conversie naar BerekeningVeld
                foreach (List<Veld> lijn in kost.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {
                        _berekeningVelden.Add(new BerekeningVeld(kost.BerekeningId, kost.Lijnen.IndexOf(lijn), veld.VeldId));
                        _velden.Add(veld);
                    }
                }
            }
            foreach (Berekening baat in analyse.Baten)
            {
                //conversie van velden
                baat.Serialiseer();
                //conversie naar BerekeningVeld
                foreach (List<Veld> lijn in baat.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {
                        _berekeningVelden.Add(new BerekeningVeld(baat.BerekeningId, baat.Lijnen.IndexOf(lijn), veld.VeldId));
                        _velden.Add(veld);
                    }
                }
            }
        }

        public void VerwijderVelden(Analyse analyse)
        {
            foreach (Berekening baat in analyse.Baten)
            {

                foreach (List<Veld> lijn in baat.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {
                        _berekeningVelden.Remove(_berekeningVelden.First(b => b.BerekeningId == baat.BerekeningId && b.LijnId == baat.Lijnen.IndexOf(lijn) && b.VeldId == veld.VeldId));
                        _velden.Remove(veld);
                    }
                }
               
            }
            foreach (Berekening kost in analyse.Kosten)
            {
                foreach (List<Veld> lijn in kost.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {

                        _berekeningVelden.Remove(_berekeningVelden.First(b => b.BerekeningId == kost.BerekeningId && b.LijnId == kost.Lijnen.IndexOf(lijn) && b.VeldId == veld.VeldId));
                        _velden.Remove(veld);
                    }
                }
                
            }
            //fout op loonkostsubsidie verwijderen
            _dbContext.Berekeningen.Remove(analyse.Baten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie")));
            analyse.Baten.Remove(analyse.Baten.First(k => k.GetType() == Type.GetType("KostenBatenTool.Models.Domain.LoonkostSubsidie")));
            _dbContext.SaveChanges();
            _analyses.Remove(analyse);
        }

        public Analyse GetAnalyse(string email, int id)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie").Include("Analyses.Baten.Velden").Include("Analyses.Kosten.Velden").First(a => a.Email.Equals(email)).Analyses.FirstOrDefault(a => a.AnalyseId == id);
            //Deserialiseren
            foreach (Berekening berekening in analyse.Kosten)
            {
                List<BerekeningVeld> berekenVelden = _berekeningVelden.Where(b => b.BerekeningId == berekening.BerekeningId).ToList();
                List<List<Veld>> lijnen = new List<List<Veld>>();
                for (int i = 0; i <= berekenVelden.Max(b => b.LijnId); i++)
                {
                    List<BerekeningVeld> berekeningVeldLijn = berekenVelden.Where(b => b.LijnId == i).ToList();
                    List<Veld> lijn = new List<Veld>();
                    foreach (BerekeningVeld berekeningVeld in berekeningVeldLijn)
                    {
                        Veld veld = _velden.FirstOrDefault(v => v.VeldId == berekeningVeld.VeldId);
                        lijn.Add(veld);
                    }
                    lijnen.Add(lijn);
                }
                berekening.Lijnen = lijnen;
                //Deserialiseren
                berekening.Deserialiseer();
            }
            foreach (Berekening berekening in analyse.Baten)
            {
                List<BerekeningVeld> berekenVelden = _berekeningVelden.Where(b => b.BerekeningId == berekening.BerekeningId).ToList();
                List<List<Veld>> lijnen = new List<List<Veld>>();
                for (int i = 0; i <= berekenVelden.Max(b => b.LijnId); i++)
                {
                    List<BerekeningVeld> berekeningVeldLijn = berekenVelden.Where(b => b.LijnId == i).ToList();
                    List<Veld> lijn = new List<Veld>();
                    foreach (BerekeningVeld berekeningVeld in berekeningVeldLijn)
                    {
                        Veld veld = _velden.FirstOrDefault(v => v.VeldId == berekeningVeld.VeldId);
                        lijn.Add(veld);
                    }
                    lijnen.Add(lijn);
                }
                berekening.Lijnen = lijnen;
                //Deserialiseren
                berekening.Deserialiseer();
            }
            return analyse;
        }
    }
}



