﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using Microsoft.Net.Http.Headers;

namespace KostenBatenTool.Controllers
{
    public class AnalyseController : Controller
    {

        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;
        private readonly IDoelgroepRepository _doelgroepRepository;
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IOrganisatieRepository _organisatieRepository;
        private readonly ILijnRepository _lijnRepository;
        private readonly IBerekeningRepository _berekeningRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _hostingEnv;

        public AnalyseController(IHostingEnvironment hostingEnv, IDoelgroepRepository doelgroepRepository,
            IAnalyseRepository analyserepository, 
            IOrganisatieRepository organisatieRepository,
            IBerekeningRepository berekeningRepository,
            IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository,
            ILijnRepository lijnRepository,
            UserManager<ApplicationUser> userManager, IEmailService emailService)

        {
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
            _doelgroepRepository = doelgroepRepository;
            _analyseRepository = analyserepository;
            _berekeningRepository = berekeningRepository;
            _organisatieRepository = organisatieRepository;
            _lijnRepository = lijnRepository;
            _userManager = userManager;
            _emailService = emailService;
            _hostingEnv = hostingEnv;
        }

        // GET: /<controller>/
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string zoekCriteria = "Werkgever")
        {
            ViewData["NameSortParm"] = sortOrder == "Naam" ? "naam_desc" : "Naam";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["GemeenteSort"] = sortOrder == "Gemeente" ? "gemeente_desc" : "Gemeente";
            ViewData["AfdelingSort"] = sortOrder == "Afdeling" ? "afdeling_desc" : "Afdeling";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAllAnalysesVanArbeidsBemiddelaar(User.Identity.Name);
            if (!String.IsNullOrEmpty(searchString))
            {
                if (zoekCriteria == null || zoekCriteria.Equals("Werkgever"))
                {
                    a = _arbeidsBemiddelaarRepository.ZoekAnalysesWerkgever(User.Identity.Name, searchString);
                }
                else
                {
                    a = _arbeidsBemiddelaarRepository.ZoekAnalysesGemeente(User.Identity.Name, searchString);
                }
            }
            switch (sortOrder)
            {
                case "naam_desc":
                    a = a.OrderByDescending(s => s.Organisatie.Naam);
                    ViewData["pijlNaam"] = "fa-chevron-down";
                    break;
                case "Naam":
                    a = a.OrderBy(s => s.Organisatie.Naam);
                    ViewData["pijlNaam"] = "fa-chevron-up";
                    break;
                case "date_desc":
                    a = a.OrderByDescending(s => s.AanmaakDatum);
                    ViewData["pijlDatum"] = "fa-chevron-down";
                    break;
                case "Gemeente":
                    a = a.OrderBy(s => s.Organisatie.Gemeente);
                    ViewData["pijlGemeente"] = "fa-chevron-up";
                    break;
                case "gemeente_desc":
                    a = a.OrderByDescending(s => s.Organisatie.Gemeente);
                    ViewData["pijlGemeente"] = "fa-chevron-down";
                    break;
                case "Afdeling":
                    a = a.OrderBy(s => s.Organisatie.Afdeling);
                    ViewData["pijlAfdeling"] = "fa-chevron-up";
                    break;
                case "afdeling_desc":
                    a = a.OrderByDescending(s => s.Organisatie.Afdeling);
                    ViewData["pijlAfdeling"] = "fa-chevron-down";
                    break;
                default:
                    a = a?.OrderBy(s => s.AanmaakDatum);
                    ViewData["pijlDatum"] = "fa-chevron-up";
                    break;
            }
            int pageSize = 10;
            return View(a == null ? null : PaginatedList<Analyse>.CreateAsync(a.AsQueryable(), page ?? 1, pageSize));
        }

        public IActionResult Nieuw()
        {
            IEnumerable<Organisatie> organisaties = _arbeidsBemiddelaarRepository.GetOrganisatiesVanArbeidsBemiddelaar(User.Identity.Name);
            return View(organisaties);


        }

        public IActionResult PartialWerkgevers()
        {
            var user = GetCurrentUserAsync();
            string email = user.Result.Email;
            IEnumerable<Organisatie> organisaties = _arbeidsBemiddelaarRepository.GetOrganisatiesVanArbeidsBemiddelaar(User.Identity.Name);
            return PartialView("_werkgeversPartial", organisaties);
        }

        public IActionResult Werkgever(int id = -1, bool nieuw = false, int analyseId = -1)
        {
            ViewData["nieuw"] = nieuw;
            Organisatie o = _organisatieRepository.GetOrganisatie(id);
            WerkgeverViewModel model = o == null ? new WerkgeverViewModel() : (analyseId < 0 ? new WerkgeverViewModel(o) : new WerkgeverViewModel(o, analyseId));

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
                    o.UrenWerkWeek = Convert.ToDecimal(model.Werkuren);
                    o.PatronaleBijdrage = Convert.ToDecimal(model.Bijdrage) / 100;
                    o.Afdeling = model.Afdeling;
                    if (model.EmailContactpersoon != null)
                    {
                        Contactpersoon contactpersoon = new Contactpersoon(model.NaamContactpersoon,
                            model.VoornaamContactpersoon, model.EmailContactpersoon);
                        o.Contactpersoon = contactpersoon;
                    }
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar a = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarMetAnalyses(email);
                    Analyse analyse = new Analyse(o);
                    a.VoegNieuweAnalyseToe(analyse);
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    return RedirectToAction(nameof(Overzicht), new { analyseId = model.AnalyseId });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            ModelState.AddModelError("EmailReg", "Uw e-mailadres is onjuist, bent u al geregistreerd?");
            return View(model);
        }

        [HttpPost]
        public IActionResult WerkgeverEdit(WerkgeverViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    Organisatie o = _organisatieRepository.GetOrganisatie(model.OrganisatieId);
                    o.Naam = model.Naam;
                    o.Afdeling = model.Afdeling;
                    o.Gemeente = model.Gemeente;
                    o.Straat = model.Straat;
                    o.Huisnummer = model.Huisnummer;
                    o.UrenWerkWeek = Convert.ToDecimal(model.Werkuren);
                    o.PatronaleBijdrage = Convert.ToDecimal(model.Bijdrage) / 100;
                    if (o.Contactpersoon == null && model.EmailContactpersoon != null)
                    {
                        Contactpersoon contactpersoon = new Contactpersoon(model.NaamContactpersoon,
                            model.VoornaamContactpersoon, model.EmailContactpersoon);
                        o.Contactpersoon = contactpersoon;
                    }
                    else if (o.Contactpersoon != null)
                    {
                        o.Contactpersoon.Email = model.EmailContactpersoon;
                        o.Contactpersoon.Naam = model.NaamContactpersoon;
                        o.Contactpersoon.Voornaam = model.VoornaamContactpersoon;
                    }

                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(Overzicht), new { analyseId = model.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            ViewData["nieuw"] = false;
            return View(nameof(Werkgever), model);
        }

        [HttpGet]
        public IActionResult Overzicht(int analyseId)
        {
            return View(new OverzichtViewModel(GetAnalyse(analyseId)));
        }

        [HttpGet]
        public IActionResult LoonKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            LoonKost loonkost = (LoonKost)a.GetBerekening("LoonKost");
            IList<LoonKostLijn> lijnen = _lijnRepository.GetLoonKostLijnen(loonkost.BerekeningId, loonkost.Velden);
            if (lijnId > 0)
            {
                Lijn lijn = _lijnRepository.GetLoonKostLijn(lijnId, loonkost.Velden);
                ViewData["open"] = true;
                LoonkostViewModel vm1 = new LoonkostViewModel(lijn, lijnen, a.AnalyseId);
                vm1.DoelgroepList = _doelgroepRepository.GetAll();
                vm1.VopList = new List<Veld> { new Veld("0%", 0, 1), new Veld("20%", 0.20, 1), new Veld("30%", 0.30, 2), new Veld("40%", 0.40, 3) };
                return View(vm1);

            }
            ViewData["open"] = false;
            LoonkostViewModel vm = new LoonkostViewModel(lijnen, a.AnalyseId);
            vm.DoelgroepList = _doelgroepRepository.GetAll();
            vm.VopList = new List<Veld> { new Veld("0%", 0, 1), new Veld("20%", 0.20, 1), new Veld("30%", 0.30, 2), new Veld("40%", 0.40, 3) };
            return View(vm);
        }


        [HttpPost]
        public IActionResult LoonKost(LoonkostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("LoonKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("LoonKost", model.LijnId, "functie", model.Functie);
                    analyse.VulVeldIn("LoonKost", model.LijnId, "uren per week", Convert.ToDecimal(model.UrenPerWeek));
                    analyse.VulVeldIn("LoonKost", model.LijnId, "bruto maandloon fulltime", Convert.ToDecimal(model.BrutoMaandloon));
                    Doelgroep doelgroep = _doelgroepRepository.GetById(model.DoelgroepId);
                    ((LoonKostLijn)analyse.GetBerekening("LoonKost").Lijnen.FirstOrDefault(l => l.LijnId == model.LijnId)).Doelgroep = doelgroep;
                    analyse.VulVeldIn("LoonKost", model.LijnId, "% Vlaamse ondersteuningspremie", model.VopId);
                    analyse.VulVeldIn("LoonKost", model.LijnId, "aantal maanden IBO", Convert.ToDecimal(model.AantalMaanden));
                    analyse.VulVeldIn("LoonKost", model.LijnId, "totale productiviteitspremie IBO", Convert.ToDecimal(model.Ibo));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De functie werd succesvol toegevoegd.";

                    return RedirectToAction("Loonkost", new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            Berekening loonkost = GetAnalyse(model.AnalyseId).GetBerekening("LoonKost");
            model.Lijnen = _lijnRepository.GetLoonKostLijnen(loonkost.BerekeningId, loonkost.Velden).Select( lijn => new LoonkostLijnViewModel(lijn)).ToList();
            model.DoelgroepList = _doelgroepRepository.GetAll();
            model.VopList = new List<Veld> { new Veld("0%", 0, 1), new Veld("20%", 0.20, 1), new Veld("30%", 0.30, 2), new Veld("40%", 0.40, 3) };
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _analyseRepository.VerwijderAnalyse(id);
            _analyseRepository.SaveChanges();
            TempData["message"] = "De analyse werd succesvol verwijderd.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult AanpassingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AanpassingsKost kost = (AanpassingsKost)a.GetBerekening("AanpassingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult AanpassingsKost(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AanpassingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AanpassingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("AanpassingsKost", model.LijnId, "bedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kost werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(AanpassingsKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("AanpassingsKost").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "type", "bedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AanpassingsSubsidie(int analyseId)
        {
            Analyse a = GetAnalyse(analyseId);
            AanpassingsSubsidie besparing = (AanpassingsSubsidie)a.GetBerekening("AanpassingsSubsidie");
            return View(new EenDecimalViewModel(besparing, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult AanpassingsSubsidie(EenDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    Lijn lijn = ((AanpassingsSubsidie)analyse.GetBerekening("AanpassingsSubsidie")).Lijnen[0];

                    analyse.VulVeldIn("AanpassingsSubsidie", lijn.LijnId, "jaarbedrag", Convert.ToDecimal(model.Jaarbedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                     TempData["message"] = "De subsidie werd succesvol toegevoegd.";
                    return RedirectToAction(nameof(AanpassingsSubsidie), new { analyseId = model.AnalyseId });

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
        public IActionResult AdministratieBegeleidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AdministratieBegeleidingsKost kost =
                (AdministratieBegeleidingsKost)a.GetBerekening("AdministratieBegeleidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
            ViewData["open"] = false;
            return View(new DrieDecimalViewModel(kost, analyseId));
        }

        [HttpPost]
        public IActionResult AdministratieBegeleidingsKost(DrieDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AdministratieBegeleidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "uren", Convert.ToDecimal(model.Veld1));
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "bruto maandloon begeleider", Convert.ToDecimal(model.Veld2));
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "jaarbedrag", Convert.ToDecimal(model.Veld3));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De uren werden succesvol toegevoegd.";

                    return RedirectToAction(nameof(AdministratieBegeleidingsKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("AdministratieBegeleidingsKost").Lijnen.Select(l => new DrieDecimalLijstObjectViewModel(l, "bruto maandloon begeleider", "jaarbedrag")).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult AndereBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AndereBesparing kost = (AndereBesparing)a.GetBerekening("AndereBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult AndereBesparing(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AndereBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AndereBesparing", model.LijnId, "beschrijving", model.Type);
                    analyse.VulVeldIn("AndereBesparing", model.LijnId, "jaarbedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De besparing werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(AndereBesparing), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("AndereBesparing").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "beschrijving", "jaarbedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AndereKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AndereKost kost = (AndereKost)a.GetBerekening("AndereKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult AndereKost(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AndereKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AndereKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("AndereKost", model.LijnId, "bedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kost werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(AndereKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("AndereKost").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "type", "bedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult LogistiekeBesparing(int analyseId)
        {
            Analyse a = GetAnalyse(analyseId);
            LogistiekeBesparing besparing = (LogistiekeBesparing)a.GetBerekening("LogistiekeBesparing");
            return View(new LogistiekeBesparingViewModel(besparing, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult LogistiekeBesparing(LogistiekeBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    Lijn lijn = ((LogistiekeBesparing)analyse.GetBerekening("LogistiekeBesparing")).Lijnen[0];
                    analyse.VulVeldIn("LogistiekeBesparing", lijn.LijnId, "transportkosten jaarbedrag", Convert.ToDecimal(model.Transport));
                    analyse.VulVeldIn("LogistiekeBesparing", lijn.LijnId, "logistieke kosten jaarbedrag",Convert.ToDecimal(model.Logistiek));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kosten werden succesvol toegevoegd.";
                    return RedirectToAction(nameof(LogistiekeBesparing), new { analyseId = model.AnalyseId });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        //[HttpGet]
        //public IActionResult LoonkostSubsidie()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult LoonkostSubsidie(LoonkostSubsidieViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {     // die ene kost toevoegen vanuit de partial

        //            return RedirectToAction(nameof(Overzicht));
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult MedewerkerHogerNiveauBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            MedewerkerHogerNiveauBesparing kost =
                (MedewerkerHogerNiveauBesparing)a.GetBerekening("MedewerkerHogerNiveauBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
            ViewData["open"] = false;
            return View(new DrieDecimalViewModel(kost, analyseId));
        }


        [HttpPost]
        public IActionResult MedewerkerHogerNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("MedewerkerHogerNiveauBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "uren", Convert.ToDecimal(model.Veld1));
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "bruto maandloon fulltime",
                       Convert.ToDecimal(model.Veld2));
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "totale loonkost per jaar", Convert.ToDecimal(model.Veld3));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "Het loon werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(MedewerkerHogerNiveauBesparing), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("MedewerkerHogerNiveauBesparing").Lijnen.Select(l => new DrieDecimalLijstObjectViewModel(l, "bruto maandloon fulltime", "totale loonkost per jaar")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult MedewerkerZelfdeNiveauBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            MedewerkerZelfdeNiveauBesparing kost =
                (MedewerkerZelfdeNiveauBesparing)a.GetBerekening("MedewerkerZelfdeNiveauBesparing");
            if (lijnId > 0)
            {
                ViewData["open"] = true;
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
            ViewData["open"] = false;
            return View(new DrieDecimalViewModel(kost, analyseId));
        }


        [HttpPost]
        public IActionResult MedewerkerZelfdeNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("MedewerkerZelfdeNiveauBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "uren", Convert.ToDecimal(model.Veld1));
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "bruto maandloon fulltime",
                        Convert.ToDecimal(model.Veld2));
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "totale loonkost per jaar", Convert.ToDecimal(model.Veld3));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "Het loon werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(MedewerkerZelfdeNiveauBesparing), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("MedewerkerZelfdeNiveauBesparing").Lijnen.Select(l => new DrieDecimalLijstObjectViewModel(l, "bruto maandloon fulltime", "totale loonkost per jaar")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult OmzetverliesBesparing(int analyseId)
        {
            Analyse a = GetAnalyse(analyseId);
            OmzetverliesBesparing besparing = (OmzetverliesBesparing)a.GetBerekening("OmzetverliesBesparing");
            return View(new OmzetverliesBesparingViewModel(besparing, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult OmzetverliesBesparing(OmzetverliesBesparingViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    Lijn lijn = ((OmzetverliesBesparing)analyse.GetBerekening("OmzetverliesBesparing")).Lijnen[0];
                    analyse.VulVeldIn("OmzetverliesBesparing", lijn.LijnId, "jaarbedrag omzetverlies", Convert.ToDecimal(model.Veld1));
                    analyse.VulVeldIn("OmzetverliesBesparing", lijn.LijnId, "% besparing", Convert.ToDecimal(model.Veld2 )/ 100);
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "Het bedrag werd succesvol toegevoegd.";
                    return RedirectToAction(nameof(OmzetverliesBesparing), new { analyseId = model.AnalyseId });

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
        public IActionResult OpleidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            OpleidingsKost kost = (OpleidingsKost)a.GetBerekening("OpleidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult OpleidingsKost(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("OpleidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("OpleidingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("OpleidingsKost", model.LijnId, "bedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De opleidingskost werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(OpleidingsKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("OpleidingsKost").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "type", "bedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult OutsourcingBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            OutsourcingBesparing kost = (OutsourcingBesparing)a.GetBerekening("OutsourcingBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult OutsourcingBesparing(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("OutsourcingBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("OutsourcingBesparing", model.LijnId, "beschrijving", model.Type);
                    analyse.VulVeldIn("OutsourcingBesparing", model.LijnId, "jaarbedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "Het bedrag werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(OutsourcingBesparing), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("OutsourcingBesparing").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "beschrijving", "jaarbedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult OverurenBesparing(int analyseId)
        {
            Analyse a = GetAnalyse(analyseId);
            OverurenBesparing besparing = (OverurenBesparing)a.GetBerekening("OverurenBesparing");
            return View(new EenDecimalViewModel(besparing, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult OverurenBesparing(EenDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    Lijn lijn = ((OverurenBesparing)analyse.GetBerekening("OverurenBesparing")).Lijnen[0];
                    analyse.VulVeldIn("OverurenBesparing", lijn.LijnId, "jaarbedrag", Convert.ToDecimal(model.Jaarbedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kost werd succesvol toegevoegd.";
                    return RedirectToAction(nameof(OverurenBesparing), new { analyseId = model.AnalyseId });

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
        public IActionResult ProductiviteitsWinst(int analyseId)
        {
            Analyse a = GetAnalyse(analyseId);
            ProductiviteitsWinst besparing = (ProductiviteitsWinst)a.GetBerekening("ProductiviteitsWinst");
            return View(new EenDecimalViewModel(besparing, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult ProductiviteitsWinst(EenDecimalViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    Lijn lijn = ((ProductiviteitsWinst)analyse.GetBerekening("ProductiviteitsWinst")).Lijnen[0];
                    analyse.VulVeldIn("ProductiviteitsWinst", lijn.LijnId, "jaarbedrag", Convert.ToDecimal(model.Jaarbedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "Het bedrag werd succesvol toegevoegd.";
                    return RedirectToAction(nameof(ProductiviteitsWinst), new { analyseId = model.AnalyseId });

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
        public IActionResult UitzendkrachtenBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            UitzendkrachtenBesparing kost = (UitzendkrachtenBesparing)a.GetBerekening("UitzendkrachtenBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult UitzendkrachtenBesparing(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("UitzendkrachtenBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("UitzendkrachtenBesparing", model.LijnId, "beschrijving", model.Type);
                    analyse.VulVeldIn("UitzendkrachtenBesparing", model.LijnId, "jaarbedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De besparing werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(UitzendkrachtenBesparing), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("UitzendkrachtenBesparing").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "beschrijving", "jaarbedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult VoorbereidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            VoorbereidingsKost kost = (VoorbereidingsKost)a.GetBerekening("VoorbereidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult VoorbereidingsKost(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("VoorbereidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("VoorbereidingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("VoorbereidingsKost", model.LijnId, "bedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kost werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(VoorbereidingsKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("VoorbereidingsKost").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "type", "bedrag")).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult WerkkledijKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            WerkkledijKost kost = (WerkkledijKost)a.GetBerekening("WerkkledijKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                ViewData["open"] = true;
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
            ViewData["open"] = false;
            return View(new TypeBedragViewModel(kost, a.AnalyseId));
        }


        [HttpPost]
        public IActionResult WerkkledijKost(TypeBedragViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    Analyse analyse = GetAnalyse(model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("WerkkledijKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("WerkkledijKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("WerkkledijKost", model.LijnId, "bedrag", Convert.ToDecimal(model.Bedrag));
                    _analyseRepository.SerialiseerVelden(analyse);
                    _analyseRepository.SaveChanges();
                    TempData["message"] = "De kost werd succesvol toegevoegd.";

                    return RedirectToAction(nameof(WerkkledijKost), new { analyseId = analyse.AnalyseId });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            ViewData["open"] = true;
            model.Lijst = GetAnalyse(model.AnalyseId).GetBerekening("WerkkledijKost").Lijnen.Select(lijn => new TypeBedragLijstObjectViewModel(lijn, "type", "bedrag")).ToList();
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
                return _arbeidsBemiddelaarRepository.GetLaatsteAnalyseVanArbeidsBemiddelaar(email);
            }
            return _analyseRepository.GetAnalyse(id);
        }


        public IActionResult DeleteLijn(int analyseId, int lijnId, int berekeningId)
        {
            Berekening b = _berekeningRepository.GetBerekening(berekeningId);
            Lijn lijn = b.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
            b.Lijnen.Remove(lijn);
            _lijnRepository.VerwijderLijn(lijn);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction(b.GetType().Name, new { analyseId, lijnId = 0 });
        }

        public IActionResult DeleteLoonKostLijn(int analyseId, int lijnId)
        {
            Lijn lijn = _lijnRepository.GetLoonKostLijn(lijnId);
            _lijnRepository.VerwijderLijn(lijn);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction("LoonKost", new { analyseId, lijnId = 0 });
        }


        [HttpGet]
        public IActionResult EmailResultaat(int id, int analyseId)
        {
            Organisatie o = _organisatieRepository.GetOrganisatie(id);
            Persoon p = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(User.Identity.Name);
            return View(new ResultaatViewModel(o, p.Voornaam, p.Naam, analyseId));
        }

        [HttpPost]
        public async Task<IActionResult> EmailResultaat(ResultaatViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Pdf == null || model.Pdf.Length == 0)
                        throw new Exception("file should not be null");

                    var filename = ContentDispositionHeaderValue.Parse(model.Pdf.ContentDisposition).FileName.Trim('"');
                    var targetDirectory = Path.Combine(_hostingEnv.WebRootPath, string.Format("common\\"));
                    var savePath = Path.Combine(targetDirectory, filename);
                    FileStream stream = new FileStream(savePath, FileMode.Create);
                    model.Pdf.CopyTo(stream);
                    stream.Dispose();
                    Organisatie o = _organisatieRepository.GetOrganisatie(model.OrganisatiId);
                    MimeMessage emailMessage = new MimeMessage();
                    emailMessage.Subject = $"Analyse {o.Naam}";
                    BodyBuilder builder = new BodyBuilder();
                    builder.HtmlBody = @"<div style='background-color:#9CCD0B;color:white;padding-left:7%;padding-right:7%;padding-top:5%;padding-bottom:5%;font-family:Arial, Helvetica;color:#555555;font-size:14px;'>
                        <div style='background-color:rgba(255,255,255, 0.74);padding:7%; border-radius:5px;'> " + model.Bericht + @"</div></div>";
                    builder.Attachments.Add(savePath);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                    }
                    emailMessage.Body = builder.ToMessageBody();
                    await _emailService.SendEmailAsync(User.Identity.Name, model.EmailContactpersoon, emailMessage);
                    TempData["message"] = "De analyse werd succesvol verstuurd.";

                    return RedirectToAction(nameof(Overzicht), model.AnalyseId);
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(model);
        }

        public IActionResult ZetBewerkbaar(int analyseId)
        {
            _analyseRepository.ZetAnalyseBewerkbaar(analyseId);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ZetAfgewerkt(int analyseId)
        {
            _analyseRepository.ZetAnalyseAfgewerkt(analyseId);
            _arbeidsBemiddelaarRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

