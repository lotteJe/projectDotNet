using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View(/*analyses*/);
        }
    }
}
