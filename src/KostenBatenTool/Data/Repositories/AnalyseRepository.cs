using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KostenBatenTool.Data.Repositories
{
    public class AnalyseRepository : IAnalyseRepository
    {
        private readonly DbSet<Analyse> _analyses;
        private readonly DbSet<BerekeningVeld> _berekeningVelden;
        private readonly DbSet<Veld> _velden;
        private readonly ApplicationDbContext _dbContext;


        public AnalyseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _analyses = dbContext.Analyses;
            _berekeningVelden = dbContext.BerekeningVelden;
            _velden = dbContext.Velden;
            
        }


        public void Add(Analyse analyse)
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

            _analyses.Add(analyse);
        }

        public Analyse GetBy(int analyseId)
        {
            //conversie van BerekeningVeld
            Analyse analyse = _analyses.Include(a => a.Organisatie).Include("Baten.Velden").Include("Kosten.Velden").FirstOrDefault(a => a.AnalyseId == analyseId);
            foreach (Berekening berekening in analyse.Kosten)
            {
                List<BerekeningVeld> berekenVelden = _berekeningVelden.Where(b => b.BerekeningId == berekening.BerekeningId).ToList();
                List<List<Veld>> lijnen = new List<List<Veld>>();
                for (int i = 0; i  < berekenVelden.Max(b => b.LijnId); i++)
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
                for (int i = 0; i < berekenVelden.Max(b => b.LijnId); i++)
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

        public IEnumerable<Analyse> GetAll()
        {
            return null;
        }

        public void Delete(Analyse analyse)
        {
            
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
