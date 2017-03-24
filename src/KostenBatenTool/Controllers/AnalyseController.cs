using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using KostenBatenTool.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Data.Repositories;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KostenBatenTool.Controllers
{
    public class AnalyseController : Controller
    {
        private readonly IOrganisatieRepository _organisatieRepository;
        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;
        public AnalyseController(IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository, IOrganisatieRepository organisatieRepository)
        {
            _organisatieRepository = organisatieRepository;
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Analyse> a = _arbeidsBemiddelaarRepository.GetAllAnalyses("sharonvanhove1@gmail.com");
            return View( a);
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
                    Organisatie o = new Organisatie(model.Naam, model.Straat, model.Huisnummer, model.Postcode, model.Gemeente);
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

        public IActionResult Overzicht()
        {
            return View();
        }

        [HttpGet]
        public IActionResult K1()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> K1(LoonkostViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                   // LoonKost kost = new LoonKost();
                   // kost.Lijnen = model.Functie;

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
