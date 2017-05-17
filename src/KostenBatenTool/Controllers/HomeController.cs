
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using KostenBatenTool.Services;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MimeKit.Text;

namespace KostenBatenTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IBerichtenRepository _berichtenRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository, 
            IAnalyseRepository analyseRepository,
            IBerichtenRepository berichtenRepository, IEmailService emailService)
        {
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
            _berichtenRepository = berichtenRepository;
            _analyseRepository = analyseRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
        public IActionResult Index()
        {
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAnalysesDashboard(User.Identity.Name);
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
                    MimeMessage emailMessage = new MimeMessage();
                    emailMessage.Subject = "Vraag via contactformulier";
                    BodyBuilder builder = new BodyBuilder();
                    builder.HtmlBody = @"<h2>Vraag via contactformulier</h2>
                        <h3>Onderwerp: " + $"{model.Onderwerp}" + @"</h3>
                        <p>" + $"{model.Omschrijving}" + @"</p>
                        <b>Afzender: " + $"{user.Result.Voornaam} {user.Result.Naam} {user.Result.Email}" + @"</b>";
                    emailMessage.Body = builder.ToMessageBody();
                    await _emailService.SendEmailAsync(user.Result.Email, "lotje.j@hotmail.com", emailMessage);
                    TempData["message"] = "Je bericht werd succesvol verstuurd.";
                    return RedirectToAction(nameof(AnalyseController.Index), "Home");
                }

            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> Berichten()
        {
            IEnumerable<Bericht> berichten = _berichtenRepository.GeefBerichten();
            IList<Bericht> berichtenGesorteerd = berichten.OrderByDescending(b => b.AanmaakDatum).ToList();
            ApplicationUser user = await GetCurrentUserAsync();
            user.OngelezenBerichten = false;
            await _userManager.UpdateAsync(user);
            return View(berichtenGesorteerd);
        }

        public IActionResult ZetAfgewerkt(int analyseId)
        {
            _analyseRepository.ZetAnalyseAfgewerkt(analyseId);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
