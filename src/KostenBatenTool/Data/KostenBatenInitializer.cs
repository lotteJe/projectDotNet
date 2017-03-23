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
        private readonly IAnalyseRepository _analyseRepository;

        public KostenBatenInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _analyseRepository = new AnalyseRepository(dbContext);
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {

                await InitializeUsers();

                Organisatie colruyt = new Organisatie("colruyt","eiklaan","23",1600,"SPL");
                Organisatie vdab = new Organisatie("vdab", "esdoornlaan", "41", 1600, "SPL");
                Organisatie delhaize = new Organisatie("delhaize", "beukenlaan", "3", 9000, "Gent");
                Organisatie[] organisaties = new Organisatie[] {colruyt, vdab,delhaize};
                _dbContext.Organisaties.AddRange(organisaties);
                _dbContext.SaveChanges();

                Persoon persoon1 = new Contactpersoon("jespers","lotte","lotte@hotmail.com");
                _dbContext.Personen.Add(persoon1);
                _dbContext.SaveChanges();

                
                Organisatie hogent = new Organisatie("HoGent", "Arbeidstraat", "14", 9300, "Aalst");
                Analyse analyseHogent = new Analyse(hogent);
                //analyse.VulVeldIn("LoonKost", 1, "functie", "manager");
                //analyse.VulVeldIn("AndereKost",1, "bedrag", 200M);
                //analyse.VulVeldIn("LoonKost", 2, "uren per week", 200M);
                //analyse.VulVeldIn("ProductiviteitsWinst", 1, "jaarbedrag", 1000M);
                Organisatie ugent = new Organisatie("UGent", "Krijgslaan", "114", 9000, "Gent");
                Analyse analyseUgent = new Analyse(ugent);
                AnalyseRepository a = new AnalyseRepository(_dbContext);
                a.Add(analyseHogent);
                a.Add(analyseUgent);
                a.SaveChanges();
                


            }
        }

        private async Task InitializeUsers()
        {
            string email = "test@test.be";
            ApplicationUser user = new ApplicationUser {UserName = email, Email = email};
            await _userManager.CreateAsync(user, "P@ssword1");

            string email1 = "lotte@test.be";
            string naam = "Lotte Jespers";
            ApplicationUser user1 = new ApplicationUser { UserName = email1, Email = email1 };
            await _userManager.CreateAsync(user1, "test");

        }

        }
    
}
