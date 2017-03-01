using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.AnalyseViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KostenBatenTool.Controllers
{
    public class AnalyseController : Controller
    {
        //private readonly IAnalyseRepository _analyseRepository;
        //public AnalyseController(IAnalyseRepository analyseRepository)
        //{
        //    _analyseRepository = analyseRepository;
        //}

        // GET: /<controller>/
        public IActionResult Index()
        {
            //IEnumerable<Analyse> analyses = _analyseRepository.geAll();
            return View( /*analyses*/);
        }
        public IActionResult Nieuw()
        {
            return View();
        }

        public IActionResult PartialWerkgevers()
        {
            return PartialView("_werkgeversPartial");
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
            }
            return View(model);
        }

        public IActionResult Overzicht()
        {
            return View();
        }
    }
}
