using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KostenBatenTool.Data.Repositories;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KostenBatenTool.Data
{
    public class KostenBatenInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public KostenBatenInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {

           _dbContext.Database.EnsureDeleted();

            if (_dbContext.Database.EnsureCreated())
            {
               
                //vul hier je eigen e-mailadres in

                Persoon superadmin = new Administrator("Moens", "Bart", "lottejespers1@gmail.com", true);

                ((Administrator) superadmin).WachtwoordReset = true;

                _dbContext.Personen.Add(superadmin);

                Bericht welkom = new Bericht("Welkom", "Welkom bij Kairos!");
                _dbContext.Berichten.Add(welkom);

                Doelgroep laaggeschoold = new Doelgroep("Jonger dan 25 en laaggeschoold", 2500M,1550M);
                Doelgroep middengeschoold = new Doelgroep("Jonger dan 25 en middengeschoold", 2500M, 1000M);
                Doelgroep tussen55En60 = new Doelgroep("Tussen 55 en 60 jaar", 4466.66M, 1150M);
                Doelgroep boven60 = new Doelgroep("Boven 60 jaar", 4466.66M, 1500M);
                Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
                _dbContext.Doelgroepen.Add(laaggeschoold);
                _dbContext.Doelgroepen.Add(middengeschoold);
                _dbContext.Doelgroepen.Add(tussen55En60);
                _dbContext.Doelgroepen.Add(boven60);
                _dbContext.Doelgroepen.Add(ander);
                
                _dbContext.SaveChanges();

            }
        }

       
        }
    
}
