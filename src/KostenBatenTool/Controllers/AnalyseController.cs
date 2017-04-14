
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
            return View(new OverzichtViewModel(GetAnalyse(id)));
        }

        [HttpGet]
        public IActionResult LoonKost(int id)
        {
            Analyse a = GetAnalyse(id);
            LoonKost loonkost = (LoonKost)a.GetBerekening("LoonKost");
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
                    int id = ((LoonKost)analyse.GetBerekening("LoonKost")).Lijnen.Count;
                    analyse.VulVeldIn("LoonKost", id, "functie", model.Functie);
                    analyse.VulVeldIn("LoonKost", id, "uren per week", model.UrenPerWeek);
                    analyse.VulVeldIn("LoonKost", id, "bruto maandloon fulltime", model.BrutoMaandloon);
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
        public IActionResult AanpassingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            AanpassingsKost kost = (AanpassingsKost)a.GetBerekening("AanpassingsKost");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult AanpassingsKost(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((AanpassingsKost)analyse.GetBerekening("AanpassingsKost")).Lijnen.Count;
                    analyse.VulVeldIn("AanpassingsKost", id, "type", model.Type);
                    analyse.VulVeldIn("AanpassingsKost", id, "bedrag", model.Bedrag);
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
                AnalyseId = a.AnalyseId,
                Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
                {
                    Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren")).Value,
                    Veld2 =
                        (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bruto maandloon begeleider")).Value,
                    Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("jaarbedrag")).Value
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AdministratieBegeleidingsKost(DrieDecimalViewModel model, string returnUrl = null)
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
                    int id = ((AdministratieBegeleidingsKost)analyse.GetBerekening("AdministratieBegeleidingsKost")).Lijnen.Count;
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", id, "uren", model.Veld1);
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", id, "bruto maandloon begeleider", model.Veld2);
                    analyse.VulVeldIn("AdministratieBegeleidingsKost", id, "jaarbedrag", model.Veld3);
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

        [HttpGet]
        public IActionResult AndereBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            AndereBesparing kost = (AndereBesparing)a.GetBerekening("AndereBesparing");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type besparing")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult AndereBesparing(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((AndereBesparing)analyse.GetBerekening("AndereBesparing")).Lijnen.Count;
                    analyse.VulVeldIn("AndereBesparing", id, "type besparing", model.Type);
                    analyse.VulVeldIn("AndereBesparing", id, "jaarbedrag", model.Bedrag);
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

        [HttpGet]
        public IActionResult AndereKost(int id)
        {
            Analyse a = GetAnalyse(id);
            AndereKost kost = (AndereKost)a.GetBerekening("AndereKost");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult AndereKost(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((AndereKost)analyse.GetBerekening("AndereKost")).Lijnen.Count;
                    analyse.VulVeldIn("AndereKost", id, "type", model.Type);
                    analyse.VulVeldIn("AndereKost", id, "bedrag", model.Bedrag);
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
        public IActionResult MedewerkerHogerNiveauBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            MedewerkerHogerNiveauBesparing kost = (MedewerkerHogerNiveauBesparing)a.GetBerekening("MedewerkerHogerNiveauBesparing");
            DrieDecimalViewModel model = new DrieDecimalViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
            {
                Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren")).Value,
                Veld2 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value,
                Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("totale loonkost per jaar")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult MedewerkerHogerNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
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
                    int id = ((MedewerkerHogerNiveauBesparing)analyse.GetBerekening("MedewerkerHogerNiveauBesparing")).Lijnen.Count;
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", id, "uren", model.Veld1);
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", id, "bruto maandloon fulltime", model.Veld2);
                    analyse.VulVeldIn("MedewerkerHogerNiveauBesparing", id, "totale loonkost per jaar", model.Veld3);
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

        [HttpGet]
        public IActionResult MedewerkerZelfdeNiveauBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            MedewerkerZelfdeNiveauBesparing kost = (MedewerkerZelfdeNiveauBesparing)a.GetBerekening("MedewerkerZelfdeNiveauBesparing");
            DrieDecimalViewModel model = new DrieDecimalViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new DrieDecimalLijstObjectViewModel
            {
                Veld1 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("uren")).Value,
                Veld2 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bruto maandloon fulltime")).Value,
                Veld3 = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("totale loonkost per jaar")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult MedewerkerZelfdeNiveauBesparing(DrieDecimalViewModel model, string returnUrl = null)
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
                    int id = ((MedewerkerZelfdeNiveauBesparing)analyse.GetBerekening("MedewerkerZelfdeNiveauBesparing")).Lijnen.Count;
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", id, "uren", model.Veld1);
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", id, "bruto maandloon fulltime", model.Veld2);
                    analyse.VulVeldIn("MedewerkerZelfdeNiveauBesparing", id, "totale loonkost per jaar", model.Veld3);
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
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult OpleidingsKost(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((OpleidingsKost)analyse.GetBerekening("OpleidingsKost")).Lijnen.Count;
                    analyse.VulVeldIn("OpleidingsKost", id, "type", model.Type);
                    analyse.VulVeldIn("OpleidingsKost", id, "bedrag", model.Bedrag);
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
        [HttpGet]
        public IActionResult OutsourcingBesparing(int id)
        {
            Analyse a = GetAnalyse(id);
            OutsourcingBesparing kost = (OutsourcingBesparing)a.GetBerekening("OutsourcingBesparing");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("beschrijving")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult OutsourcingBesparing(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((OutsourcingBesparing)analyse.GetBerekening("OutsourcingBesparing")).Lijnen.Count;
                    analyse.VulVeldIn("OutsourcingBesparing", id, "beschrijving", model.Type);
                    analyse.VulVeldIn("OutsourcingBesparing", id, "jaarbedrag", model.Bedrag);
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
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("beschrijving")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("jaarbedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult UitzendkrachtenBesparing(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((UitzendkrachtenBesparing)analyse.GetBerekening("UitzendkrachtenBesparing")).Lijnen.Count;
                    analyse.VulVeldIn("UitzendkrachtenBesparing", id, "beschrijving", model.Type);
                    analyse.VulVeldIn("UitzendkrachtenBesparing", id, "jaarbedrag", model.Bedrag);
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
        [HttpGet]
        public IActionResult VoorbereidingsKost(int id)
        {
            Analyse a = GetAnalyse(id);
            VoorbereidingsKost kost = (VoorbereidingsKost)a.GetBerekening("VoorbereidingsKost");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult VoorbereidingsKost(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((VoorbereidingsKost)analyse.GetBerekening("VoorbereidingsKost")).Lijnen.Count;
                    analyse.VulVeldIn("VoorbereidingsKost", id, "type", model.Type);
                    analyse.VulVeldIn("VoorbereidingsKost", id, "bedrag", model.Bedrag);
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
        [HttpGet]
        public IActionResult WerkkledijKost(int id)
        {
            Analyse a = GetAnalyse(id);
            WerkkledijKost kost = (WerkkledijKost)a.GetBerekening("WerkkledijKost");
            TypeBedragViewModel model = new TypeBedragViewModel(a.AnalyseId);
            model.Lijst = kost.Lijnen.Select(l => new TypeBedragLijstObjectViewModel()
            {
                Type = l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("type")).Value.ToString(),
                Bedrag = (decimal)l.VeldenWaarden.FirstOrDefault(v => v.VeldKey.Equals("bedrag")).Value
            }).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult WerkkledijKost(TypeBedragViewModel model, string returnUrl = null)
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
                    int id = ((WerkkledijKost)analyse.GetBerekening("WerkkledijKost")).Lijnen.Count;
                    analyse.VulVeldIn("WerkkledijKost", id, "type", model.Type);
                    analyse.VulVeldIn("WerkkledijKost", id, "bedrag", model.Bedrag);
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

