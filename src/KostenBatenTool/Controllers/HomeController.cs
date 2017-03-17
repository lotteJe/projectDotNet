
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOrganisatieRepository organisatieRepository, IEmailService emailService)
        {
            _organisatieRepository = organisatieRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
        public IActionResult Index()
        {
            return View();
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
