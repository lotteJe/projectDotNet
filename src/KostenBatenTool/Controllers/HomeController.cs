﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using KostenBatenTool.Services;
using Microsoft.AspNetCore.Identity;

namespace KostenBatenTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;
        private readonly IBerichtenRepository _berichtenRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository, IBerichtenRepository berichtenRepository,IEmailService emailService)
        {
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
            _berichtenRepository = berichtenRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
        public IActionResult Index()
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAllAnalyses(email);
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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public IActionResult Berichten()
        {
            List<Bericht> berichten = _berichtenRepository.GeefBerichten();
            return View();
        }
    }
}
