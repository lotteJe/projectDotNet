using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Data.Repositories;
using KostenBatenTool.Models;
using Microsoft.AspNetCore.Identity;

namespace KostenBatenTool.Controllers
{
    public class AnalyseController : Controller
    {

        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnalyseController(IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository, UserManager<ApplicationUser> userManager)
        {
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAllAnalyses(email);
            return View(a);
        }

        public IActionResult Nieuw()
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            IEnumerable<Organisatie> organisaties = _arbeidsBemiddelaarRepository.GetOrganisaties(email);
            return View(organisaties);


        }

        public IActionResult PartialWerkgevers()
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            IEnumerable<Organisatie> organisaties = _arbeidsBemiddelaarRepository.GetOrganisaties(email);
            return PartialView("_werkgeversPartial", organisaties);
        }

        public IActionResult Werkgever()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Werkgever(WerkgeverViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Organisatie o = new Organisatie(model.Naam, model.Straat, model.Huisnummer, model.Postcode,
                        model.Gemeente);
                    o.UrenWerkWeek = model.Werkuren;
                    o.PatronaleBijdrage = model.Bijdrage / 100;
                    o.Afdeling = model.Afdeling;
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar a = _arbeidsBemiddelaarRepository.GetBy(email);
                    Analyse analyse = new Analyse(o);
                    a.VoegNieuweAnalyseToe(analyse);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Overzicht(int id)
        {
            return View(GetAnalyse(2));
        }

        [HttpGet]
        public IActionResult LoonKost(int id)
        {
            Analyse a = GetAnalyse(id);
            LoonKost loonkost = (LoonKost)a.GetBerekening("LoonKost");
            return View(loonkost);
        }


        [HttpPost]
        public async Task<IActionResult> LoonKost(LoonkostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            Analyse analyse = _arbeidsBemiddelaarRepository.GetAnalyse(email, id);
            _arbeidsBemiddelaarRepository.GetBy(email).Analyses.Remove(analyse);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult AanpassingsKost()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AanpassingsKost(AanpassingsKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AanpassingsSubsidie()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AanpassingsSubsidie(AanpassingsSubsidieViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AdministratieBegeleidingsKost()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AdministratieBegeleidingsKost(AdministratieBegeleidingsKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AndereBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AndereBesparing(AndereBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try     // die ene kost toevoegen vanuit de partial
                {
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AndereKost()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AndereKost(AndereKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult LogistiekeBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogistiekeBesparing(LogistiekeBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try     // die ene kost toevoegen vanuit de partial
                {
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult LoonkostSubsidie()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoonkostSubsidie(LoonkostSubsidieViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult MedewerkerHogerNiveauBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> MedewerkerHogerNiveauBesparing(MedewerkerHogerNiveauBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult MedewerkerZelfdeNiveauBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> MedewerkerZelfdeNiveauBesparing(MedewerkerZelfdeNiveauBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult OmzetverliesBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> OmzetverliesBesparing(OmzetverliesBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    // die ene kost toevoegen vanuit de partial
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult OpleidingsKost()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> OpleidingsKost(OpleidingsKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult OutsourcingBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> OutsourcingBesparing(OutsourcingBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult OverurenBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> OverurenBesparing(OverurenBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ProductiviteitsWinst()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ProductiviteitsWinst(ProductiviteitsWinstViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UitzendkrachtenBesparing()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UitzendkrachtenBesparing(UitzendkrachtenBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult VoorbereidingsKost()
        {
            // lijst tonen van kosten  die er al zijn
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> VoorbereidingsKost(VoorbereidingsKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    // die ene kost toevoegen vanuit de partial
                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult WerkkledijKost()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> WerkkledijKost(WerkkledijKostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {     // die ene kost toevoegen vanuit de partial

                    return RedirectToAction(nameof(Overzicht));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private Analyse GetAnalyse(int id)
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            return _arbeidsBemiddelaarRepository.GetAnalyse(email, id);
        }
    }

}
