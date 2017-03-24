﻿using System;
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
        private readonly ApplicationDbContext _dbContext;

        public ArbeidsBemiddelaarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _arbeidsBemiddelaars = _dbContext.ArbeidsBemiddelaars;
            _berekeningVelden = dbContext.BerekeningVelden;
            _velden = dbContext.Velden;
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

        public IEnumerable<Analyse> GetAllAnalyses(string emailadres)
        {
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
            foreach (Berekening kost in analyse.Kosten)
            {
                foreach (List<Veld> lijn in kost.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {

                        _berekeningVelden.Remove(new BerekeningVeld(kost.BerekeningId, kost.Lijnen.IndexOf(lijn), veld.VeldId));
                        _velden.Remove(veld);
                    }
                }
            }
            foreach (Berekening baat in analyse.Baten)
            {

                foreach (List<Veld> lijn in baat.Lijnen)
                {
                    foreach (Veld veld in lijn)
                    {
                        _berekeningVelden.Remove(new BerekeningVeld(baat.BerekeningId, baat.Lijnen.IndexOf(lijn), veld.VeldId));
                        _velden.Remove(veld);
                    }
                }
            }
        }

        public Analyse GetAnalyse(string email, int id)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie").Include("Baten.Velden").Include("Kosten.Velden").First(a => a.Email.Equals(email)).Analyses.FirstOrDefault(a => a.AnalyseId == id);
            //Deserialiseren
            foreach (Berekening berekening in analyse.Kosten)
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
    }
}
