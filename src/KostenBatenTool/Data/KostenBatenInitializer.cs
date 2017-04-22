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
                //await InitializeUsers();


                //Organisatie colruyt = new Organisatie("colruyt","eiklaan","23",1600,"SPL");
                //Organisatie delhaize = new Organisatie("delhaize", "beukenlaan", "3", 9000, "Gent");
                //Organisatie[] organisaties = new Organisatie[] {colruyt,delhaize};
                //_dbContext.Organisaties.AddRange(organisaties);
                //_dbContext.SaveChanges();

                Persoon persoon1 = new Contactpersoon("jespers","lotte","lotte@hotmail.com");
                _dbContext.Personen.Add(persoon1);
                Bericht welkom = new Bericht("Welkom", "Welkom bij Kairos!");
                _dbContext.Berichten.Add(welkom);
                _dbContext.SaveChanges();

                ////dit gebeurt bij registreren
                //Organisatie vdab = new Organisatie("vdab", "KroonveldLaan", "102", 9200, "Dendermonde");
                //Persoon sharonRegistreer = new ArbeidsBemiddelaar("Van Hove", "Sharon", "sharonvanhove1@gmail.com", vdab);
                //_dbContext.Personen.Add(sharonRegistreer);
                //_dbContext.SaveChanges();



                ////ophalen gebeurt bij overgang naar dashboard
                //ArbeidsBemiddelaarRepository arb = new ArbeidsBemiddelaarRepository(_dbContext);
                //ArbeidsBemiddelaar sharon = arb.GetBy("sharonvanhove1@gmail.com");
               

                ////nieuwe analyse toevoegen
                //Organisatie hogent = new Organisatie("HoGent", "Arbeidstraat", "14", 9300, "Aalst");
                //Analyse analyseHogent = new Analyse(hogent);
                //sharon.VoegNieuweAnalyseToe(analyseHogent);
                //Organisatie ugent = new Organisatie("UGent", "Krijgslaan", "114", 9000, "Gent");
                //Analyse analyseUgent = new Analyse(ugent);
                //sharon.VoegNieuweAnalyseToe(analyseUgent);

                //foreach(Analyse analyse in sharon.Analyses)
                //    arb.SerialiseerVelden(analyse);
                //arb.SaveChanges();
               
                
                //analyses ophalen
                //Analyse analyseHogent = a.GetBy(1);
                //analyseHogent.VulVeldIn("LoonKost", 0, "functie", "manager");
                //analyseHogent.VulVeldIn("LoonKost", 0, "bruto maandloon fulltime", 1000M);
                //analyseHogent.VulVeldIn("LoonKost", 0, "uren per week", 40.0M);
                //analyseHogent.VulVeldIn("LoonKost", 1, "bruto maandloon fulltime", 1200M);
                //analyseHogent.VulVeldIn("LoonKost", 1, "uren per week", 40.0M);
                //analyseHogent.VulVeldIn("AndereKost",1, "bedrag", 200M);
                //analyseHogent.VulVeldIn("ProductiviteitsWinst", 1, "jaarbedrag", 1000M);

                //Analyse analyseUgent = a.GetBy(2);
                //analyseUgent.VulVeldIn("LoonKost", 0, "functie", "docent");
                //analyseUgent.VulVeldIn("LoonKost", 0, "bruto maandloon fulltime", 2000M);
                //analyseUgent.VulVeldIn("LoonKost", 0, "uren per week", 40.0M);
                //analyseUgent.VulVeldIn("LoonKost", 1, "bruto maandloon fulltime", 2200M);
                //analyseUgent.VulVeldIn("LoonKost", 1, "uren per week", 40.0M);
                //analyseUgent.VulVeldIn("AndereKost", 1, "bedrag", 100M);
                //analyseUgent.VulVeldIn("ProductiviteitsWinst", 1, "jaarbedrag", 1000M);
               
                //a.SaveChanges();
                


            }
        }

        private async Task InitializeUsers()
        {
            string email = "test@test.be";
            ApplicationUser user = new ApplicationUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user, "P@ssword1");

            string email1 = "lotte@test.be";
            string naam = "Lotte Jespers";
            ApplicationUser user1 = new ApplicationUser { UserName = email1, Email = email1 };
            await _userManager.CreateAsync(user1, "test");

        }

        }
    
}
