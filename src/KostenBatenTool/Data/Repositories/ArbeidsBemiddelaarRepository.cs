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
            analyse.Kosten.ForEach(k => k.Serialiseer());
            analyse.Baten.ForEach(b => b.Serialiseer());
        }

        public Analyse GetAnalyse(string email, int id)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie").Include("Analyses.Baten.Velden").Include("Analyses.Kosten.Velden").Include("Analyses.Kosten.Lijnen.VeldenDefinitie").Include("Analyses.Kosten.Lijnen.VeldenWaarden").Include("Analyses.Baten.Lijnen.VeldenDefinitie").Include("Analyses.Baten.Lijnen.VeldenWaarden").First(a => a.Email.Equals(email)).Analyses.FirstOrDefault(a => a.AnalyseId == id);
            //Deserialiseren
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            return analyse;
        }

        public Analyse GetLaatsteAnalyse(string email)
        {
            //Ophalen
            Analyse analyse = _arbeidsBemiddelaars.Include("Analyses.Organisatie").Include("Analyses.Baten.Velden").Include("Analyses.Kosten.Velden").Include("Analyses.Kosten.Lijnen.VeldenDefinitie").Include("Analyses.Kosten.Lijnen.VeldenWaarden").Include("Analyses.Baten.Lijnen.VeldenDefinitie").Include("Analyses.Baten.Lijnen.VeldenWaarden").First(a => a.Email.Equals(email)).Analyses.OrderBy(a => a.AanmaakDatum).Last();
            //Deserialiseren
            analyse.Kosten.ForEach(k => k.Deserialiseer());
            analyse.Baten.ForEach(b => b.Deserialiseer());
            return analyse;
        }
        
    }
}



