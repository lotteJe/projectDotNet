﻿using System;
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
        private readonly IAnalyseRepository _analyseRepository;
        public AnalyseController(IAnalyseRepository analyseRepository, IOrganisatieRepository organisatieRepository)
        {
            _organisatieRepository = organisatieRepository;
            _analyseRepository = analyseRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            Organisatie hogent = new Organisatie("UGent", "Arbeidstraat", "14", 9300, "Dendermonde");
            Analyse analyse = new Analyse(hogent);
            _analyseRepository.Add(analyse);
            _analyseRepository.SaveChanges();
            Analyse analyse2 = _analyseRepository.GetAnalyse(1);
            return View( analyse2);
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
                    _organisatieRepository.Add(o);
                    _organisatieRepository.SaveChanges();
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
