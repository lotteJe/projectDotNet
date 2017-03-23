
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using KostenBatenTool.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Models.AccountViewModels;
using KostenBatenTool.Models.Domain;
using KostenBatenTool.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDB.Bson;

namespace KostenBatenTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IOrganisatieRepository _organisatieRepository;
        private readonly IAnalyseRepository _analyseRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOrganisatieRepository organisatieRepository, IAnalyseRepository analyseRepository, IEmailService emailService)
        {
            _organisatieRepository = organisatieRepository;
            _analyseRepository = analyseRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
        public IActionResult Index()
        {
            //databank testen
            Organisatie hogent = new Organisatie("HoGent", "Arbeidstraat", "14", 9300, "Gent");
            Analyse analyse = new Analyse(hogent);
            analyse.VulVeldIn("LoonKost", 1, "functie", "manager");
            analyse.VulVeldIn("AndereKost",1, "bedrag", 200M);
            analyse.VulVeldIn("LoonKost", 2, "uren per week", 200M);
            analyse.VulVeldIn("ProductiviteitsWinst", 1, "jaarbedrag", 1000M);
            _analyseRepository.Add(analyse);
            _analyseRepository.SaveChanges();
            Analyse a = _analyseRepository.GetAnalyse(1);
            
            return View(a);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                   await _emailService.SendEmailAsync(user.Result.Email,"lotje.j@hotmail.com", "Vraag via contactformulier",
                        model.Omschrijving);
                    TempData["message"] = $"Je bericht werd succesvol verstuurd.";
                    return RedirectToAction(nameof(AnalyseController.Index), "Home");
                }
                
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }



    }
}
