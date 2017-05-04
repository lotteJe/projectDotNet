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

           // _dbContext.Database.EnsureDeleted();

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

                Persoon persoon2 = new Administrator("Bart", "Moens", "bart@hotmail.com");
                _dbContext.Personen.Add(persoon2);
                Bericht welkom = new Bericht("Welkom", "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui.");

                _dbContext.Berichten.Add(welkom);
                Bericht test = new Bericht("Test", "Dit is een testbericht");
                _dbContext.Berichten.Add(test);

                Doelgroep laaggeschoold = new Doelgroep("Laaggeschoold", 2500M,1550M);
                Doelgroep middengeschoold = new Doelgroep("Middengeschoold", 2500M, 1000M);
                Doelgroep tussen55En60 = new Doelgroep("Tussen 55 en 60 jaar", 4466.66M, 1150M);
                Doelgroep boven60 = new Doelgroep("Boven 60 jaar", 4466.66M, 1500M);
                Doelgroep ander = new Doelgroep("Ander", 0M, 0M);
                _dbContext.Doelgroepen.Add(laaggeschoold);
                _dbContext.Doelgroepen.Add(middengeschoold);
                _dbContext.Doelgroepen.Add(tussen55En60);
                _dbContext.Doelgroepen.Add(boven60);
                _dbContext.Doelgroepen.Add(ander);
                
                _dbContext.SaveChanges();

                ////dit gebeurt bij registreren
                //Organisatie vdab = new Organisatie("vdab", "KroonveldLaan", "102", "9200", "Dendermonde");
                //Persoon persoon3 = new ArbeidsBemiddelaar("De Kinder", "Hugo", "hugo@gmail.com", vdab);
                //_dbContext.Personen.Add(persoon3);
                //_dbContext.SaveChanges();



                ////ophalen gebeurt bij overgang naar dashboard
                //ArbeidsBemiddelaarRepository arb = new ArbeidsBemiddelaarRepository(_dbContext);
                //ArbeidsBemiddelaar hugo = arb.GetBy("hugo@gmail.com");


                ////nieuwe analyse toevoegen
                //Organisatie hogent = new Organisatie("HoGent", "Arbeidstraat", "14", "9300", "Aalst");
                //Analyse analyseHogent = new Analyse(hogent);
                //hugo.VoegNieuweAnalyseToe(analyseHogent);
                //Organisatie ugent = new Organisatie("UGent", "Krijgslaan", "114", "9000", "Gent");
                //Analyse analyseUgent = new Analyse(ugent);
                //hugo.VoegNieuweAnalyseToe(analyseUgent);

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
