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
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Werkgever(int id = -1)
        {
            Organisatie o = _arbeidsBemiddelaarRepository.GetOrganisatie(User.Identity.Name, id);
            WerkgeverViewModel model = o == null ? new WerkgeverViewModel() : new WerkgeverViewModel(o);
            return View(model);
        }

        [HttpPost]
        public IActionResult Werkgever(WerkgeverViewModel model, string returnUrl = null)
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
            return View(GetAnalyse(id));
        }

        [HttpGet]
        public IActionResult LoonKost(int id)
        {
            Analyse a = GetAnalyse(id);
            LoonKost loonkost = (LoonKost)a.GetBerekening("LoonKost");
            ViewData["Vop"] = new SelectList(new[] { "40 %", "30 %", "20 %", "0 %" });
            return View(new LoonkostViewModel(loonkost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult LoonKost(LoonkostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    int id =((LoonKost)analyse.GetBerekening("LoonKost")).Lijnen.Count;
                    analyse.VulVeldIn("LoonKost", id, "functie", model.Functie);
                    analyse.VulVeldIn("LoonKost", id, "uren per week", model.UrenPerWeek);
                    analyse.VulVeldIn("LoonKost", id, "bruto maandloon fulltime", model.BrutoMaandloon );
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(LoonKost), analyse.AnalyseId);
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
        public IActionResult AanpassingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            AanpassingsKost kost = (AanpassingsKost)a.GetBerekening("AanpassingsKost");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AanpassingsKost(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult AanpassingsSubsidie(int id)
        {
            Analyse a = GetAnalyse(id);
            AanpassingsSubsidie besparing = (AanpassingsSubsidie)a.GetBerekening("AanpassingsSubsidie");
            return View(new EenDecimalViewModel(besparing));
        }


        [HttpPost]
        public async Task<IActionResult> AanpassingsSubsidie(EenDecimalViewModel model, string returnUrl = null)
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
        public IActionResult AdministratieBegeleidingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            AdministratieBegeleidingsKost kost = (AdministratieBegeleidingsKost)a.GetBerekening("AdministratieBegeleidingsKost");
            DrieDecimalViewModel model = new DrieDecimalViewModel
            {
                Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
                {
                    Veld1 = (decimal) l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("uren")).Value,
                    Veld2 =
                        (decimal) l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bruto maandloon begeleider")).Value,
                    Veld3 = (decimal) l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("jaarbedrag")).Value
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratieBegeleidingsKost(DrieDecimalViewModel model, string returnUrl = null)
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
        public IActionResult AndereBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            AndereBesparing kost = (AndereBesparing)a.GetBerekening("AndereBesparing");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type besparing")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AndereBesparing(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult AndereKost(int id)
        {
            Analyse a = GetAnalyse(id);
            AndereKost kost = (AndereKost)a.GetBerekening("AndereKost");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AndereKost(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult LogistiekeBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            LogistiekeBesparing besparing = (LogistiekeBesparing) a.GetBerekening("LogistiekeBesparing");
            return View(new LogistiekeBesparingViewModel(besparing));
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
        public IActionResult MedewerkerHogerNiveauBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            MedewerkerHogerNiveauBesparing kost = (MedewerkerHogerNiveauBesparing)a.GetBerekening("MedewerkerHogerNiveauBesparing");
            DrieDecimalViewModel model = new DrieDecimalViewModel();
            model.Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
            {
                Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("uren")).Value,
                Veld2 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bruto maandloon fulltime")).Value,
                Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("totale loonkost per jaar")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> MedewerkerHogerNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
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
        public IActionResult MedewerkerZelfdeNiveauBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            MedewerkerZelfdeNiveauBesparing kost = (MedewerkerZelfdeNiveauBesparing)a.GetBerekening("MedewerkerZelfdeNiveauBesparing");
            DrieDecimalViewModel model = new DrieDecimalViewModel();
            model.Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
            {
                Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("uren")).Value,
                Veld2 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bruto maandloon fulltime")).Value,
                Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("totale loonkost per jaar")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> MedewerkerZelfdeNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
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
        public IActionResult OmzetverliesBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            OmzetverliesBesparing kost = (OmzetverliesBesparing)a.GetBerekening("OmzetverliesBesparing");
            DrieDecimalViewModel model = new DrieDecimalViewModel();
            model.Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
            {
                Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("jaarbedrag omzetverlies")).Value,
                Veld2 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("% besparing")).Value,
                Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("totaalbesparing")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> OmzetverliesBesparing(DrieDecimalViewModel model, string returnUrl = null)
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
        public IActionResult OpleidingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            OpleidingsKost kost = (OpleidingsKost)a.GetBerekening("OpleidingsKost");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> OpleidingsKost(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult OutsourcingBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            OutsourcingBesparing kost = (OutsourcingBesparing)a.GetBerekening("OutsourcingBesparing");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("beschrijving")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> OutsourcingBesparing(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult OverurenBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            OverurenBesparing besparing = (OverurenBesparing)a.GetBerekening("OverurenBesparing");
            return View(new EenDecimalViewModel(besparing));
        }


        [HttpPost]
        public async Task<IActionResult> OverurenBesparing(EenDecimalViewModel model, string returnUrl = null)
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
        public IActionResult ProductiviteitsWinst(int id)
        {
            Analyse a = GetAnalyse(id);
            ProductiviteitsWinst besparing = (ProductiviteitsWinst)a.GetBerekening("ProductiviteitsWinst");
            return View(new EenDecimalViewModel(besparing));
        }


        [HttpPost]
        public async Task<IActionResult> ProductiviteitsWinst(EenDecimalViewModel model, string returnUrl = null)
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
        public IActionResult UitzendkrachtenBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            UitzendkrachtenBesparing kost = (UitzendkrachtenBesparing)a.GetBerekening("UitzendkrachtenBesparing");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("beschrijving")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UitzendkrachtenBesparing(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult VoorbereidingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            VoorbereidingsKost kost = (VoorbereidingsKost)a.GetBerekening("VoorbereidingsKost");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> VoorbereidingsKost(TypeBedragViewModel model, string returnUrl = null)
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
        public IActionResult WerkkledijKost(int id)
        {
            Analyse a = GetAnalyse(id);
            WerkkledijKost kost = (WerkkledijKost)a.GetBerekening("WerkkledijKost");
            TypeBedragViewModel model = new TypeBedragViewModel();
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("type")).Value.ToString(),
                Bedrag = (decimal) l.VeldenWaarden.FirstOrDefault(v => v.Key.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> WerkkledijKost(TypeBedragViewModel model, string returnUrl = null)
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
            if (id == 0)
            {
                return _arbeidsBemiddelaarRepository.GetLaatsteAnalyse(email);
            }
            return _arbeidsBemiddelaarRepository.GetAnalyse(email, id);
        }
    }

}

