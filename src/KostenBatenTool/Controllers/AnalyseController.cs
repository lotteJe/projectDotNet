using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Data.Repositories;

namespace KostenBatenTool.Controllers
{
    public class AnalyseController : Controller
    {
        private readonly IOrganisatieRepository _organisatieRepository;
        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;

        public AnalyseController(IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository,
            IOrganisatieRepository organisatieRepository)
        {
            _organisatieRepository = organisatieRepository;
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAllAnalyses("sharonvanhove1@gmail.com");
            return View(a);
        }

        public IActionResult Nieuw()
        {
            IEnumerable<Organisatie> organisaties = _organisatieRepository.GetAll();
            return View(organisaties);
        }

        public IActionResult PartialWerkgevers()
        {
            IEnumerable<Organisatie> organisaties = _organisatieRepository.GetAll();
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
                    ArbeidsBemiddelaar a = _arbeidsBemiddelaarRepository.GetBy("sharonvanhove1@gmail.com");
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
        public IActionResult Overzicht()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoonKost()
        {
            // alle kosten tonen in tabel die er al zijn
            

            return View();
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
            Analyse analyse = _arbeidsBemiddelaarRepository.GetAnalyse("sharonvanhove1@gmail.com", id);

            _arbeidsBemiddelaarRepository.GetBy("sharonvanhove1@gmail.com").Analyses.Remove(analyse);
            _arbeidsBemiddelaarRepository.VerwijderVelden(analyse);
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
    }

}
