
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult WerkgeverEdit(WerkgeverViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    Organisatie o = _arbeidsBemiddelaarRepository.GetOrganisatie(email, model.OrganisatieId);
                    o.Naam = model.Naam;
                    o.Afdeling = model.Afdeling;
                    o.Gemeente = model.Gemeente;
                    o.Straat = model.Straat;
                    o.Huisnummer = model.Huisnummer;
                    o.UrenWerkWeek = model.Werkuren;
                    o.PatronaleBijdrage = model.Bijdrage / 100;
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
            return View(new OverzichtViewModel(GetAnalyse(id)));
        }

        [HttpGet]
        public IActionResult LoonKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            LoonKost loonkost = (LoonKost)a.GetBerekening("LoonKost");
            if (lijnId > 0)
            {
                Lijn lijn = loonkost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new LoonkostViewModel(lijn, loonkost, a.AnalyseId));
            }

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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.FirstOrDefault(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("LoonKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("LoonKost", model.LijnId, "functie", model.Functie);
                    analyse.VulVeldIn("LoonKost", model.LijnId, "uren per week", model.UrenPerWeek);
                    analyse.VulVeldIn("LoonKost", model.LijnId, "bruto maandloon fulltime", model.BrutoMaandloon);
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
            _arbeidsBemiddelaarRepository.VerwijderAnalyse(analyse);
            _arbeidsBemiddelaarRepository.SaveChanges();
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
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AanpassingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AanpassingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("AanpassingsKost", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(AanpassingsKost), analyse.AnalyseId);
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
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    analyse.VulVeldIn("AanpassingsSubsidie", 0, "jaarbedrag", model.Jaarbedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
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

        [HttpGet]
        public IActionResult AdministratieBegeleidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AdministratieBegeleidingsKost kost = (AdministratieBegeleidingsKost)a.GetBerekening("AdministratieBegeleidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
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
                   ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.FirstOrDefault(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AdministratieBegeleidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "uren", model.Veld1);
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "bruto maandloon begeleider", model.Veld2);
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", model.LijnId, "jaarbedrag", model.Veld3);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(AdministratieBegeleidingsKost), analyse.AnalyseId);
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
        public IActionResult AndereBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AndereBesparing kost = (AndereBesparing)a.GetBerekening("AndereBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AndereBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AndereBesparing", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("AndereBesparing", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(AndereBesparing), analyse.AnalyseId);
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
        public IActionResult AndereKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            AndereKost kost = (AndereKost)a.GetBerekening("AndereKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("AndereKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("AndereKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("AndereKost", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(AndereKost), analyse.AnalyseId);
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
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    analyse.VulVeldIn("LogistiekeBesparing", 0, "transportkosten jaarbedrag", model.Transport);
                    analyse.VulVeldIn("LogistiekeBesparing", 0, "logistieke kosten jaarbedrag", model.Logistiek);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
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
            MedewerkerHogerNiveauBesparing kost = (MedewerkerHogerNiveauBesparing)a.GetBerekening("MedewerkerHogerNiveauBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.FirstOrDefault(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("MedewerkerHogerNiveauBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "uren", model.Veld1);
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "bruto maandloon begeleider", model.Veld2);
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", model.LijnId, "jaarbedrag", model.Veld3);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(MedewerkerHogerNiveauBesparing), analyse.AnalyseId);
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
        public IActionResult MedewerkerZelfdeNiveauBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            MedewerkerZelfdeNiveauBesparing kost = (MedewerkerZelfdeNiveauBesparing)a.GetBerekening("MedewerkerZelfdeNiveauBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new DrieDecimalViewModel(lijn, kost, analyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.FirstOrDefault(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("MedewerkerZelfdeNiveauBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "uren", model.Veld1);
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "bruto maandloon begeleider", model.Veld2);
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", model.LijnId, "jaarbedrag", model.Veld3);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(MedewerkerZelfdeNiveauBesparing), analyse.AnalyseId);
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
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    analyse.VulVeldIn("OmzetverliesBesparing", 0, "jaarbedrag omzetverlies", model.Veld1);
                    analyse.VulVeldIn("OmzetverliesBesparing", 0, "% besparing", model.Veld2);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
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
        [HttpGet]
        public IActionResult OpleidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            OpleidingsKost kost = (OpleidingsKost)a.GetBerekening("OpleidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("OpleidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("OpleidingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("OpleidingsKost", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(OpleidingsKost), analyse.AnalyseId);
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
        public IActionResult OutsourcingBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            OutsourcingBesparing kost = (OutsourcingBesparing)a.GetBerekening("OutsourcingBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("OutsourcingBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("OutsourcingBesparing", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("OutsourcingBesparing", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(OutsourcingBesparing), analyse.AnalyseId);
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
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    analyse.VulVeldIn("OverurenBesparing", 0, "jaarbedrag", model.Jaarbedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
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
        [HttpGet]
        public IActionResult ProductiviteitsWinst(int id)
        {
            Analyse a = GetAnalyse(id);
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
                    var user = GetCurrentUserAsync();
                    string email = user.Result.Email;
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(email);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    analyse.VulVeldIn("ProductiviteitsWinst", 0, "jaarbedrag", model.Jaarbedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
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
        [HttpGet]
        public IActionResult UitzendkrachtenBesparing(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            UitzendkrachtenBesparing kost = (UitzendkrachtenBesparing)a.GetBerekening("UitzendkrachtenBesparing");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("UitzendkrachtenBesparing").VoegLijnToe();
                    }
                    analyse.VulVeldIn("UitzendkrachtenBesparing", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("UitzendkrachtenBesparing", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(UitzendkrachtenBesparing), analyse.AnalyseId);
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
        public IActionResult VoorbereidingsKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            VoorbereidingsKost kost = (VoorbereidingsKost)a.GetBerekening("VoorbereidingsKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId ==0 )
                    {
                        analyse.GetBerekening("VoorbereidingsKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("VoorbereidingsKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("VoorbereidingsKost", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(VoorbereidingsKost), analyse.AnalyseId);
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
        public IActionResult WerkkledijKost(int analyseId, int lijnId = -1)
        {
            Analyse a = GetAnalyse(analyseId);
            WerkkledijKost kost = (WerkkledijKost)a.GetBerekening("WerkkledijKost");
            if (lijnId > 0)
            {
                Lijn lijn = kost.Lijnen.FirstOrDefault(l => l.LijnId == lijnId);
                return View(new TypeBedragViewModel(lijn, kost, a.AnalyseId));
            }
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
                    ArbeidsBemiddelaar ab = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaarVolledig(User.Identity.Name);
                    Analyse analyse = ab.Analyses.First(a => a.AnalyseId == model.AnalyseId);
                    if (model.LijnId == 0)
                    {
                        analyse.GetBerekening("WerkkledijKost").VoegLijnToe();
                    }
                    analyse.VulVeldIn("WerkkledijKost", model.LijnId, "type", model.Type);
                    analyse.VulVeldIn("WerkkledijKost", model.LijnId, "bedrag", model.Bedrag);
                    _arbeidsBemiddelaarRepository.SerialiseerVelden(analyse);
                    _arbeidsBemiddelaarRepository.SaveChanges();
                    return RedirectToAction(nameof(WerkkledijKost), analyse.AnalyseId);
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

