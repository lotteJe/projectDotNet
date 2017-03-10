
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IOrganisatieRepository _organisatieRepository;
        public HomeController(IOrganisatieRepository organisatieRepository)
        {
            _organisatieRepository = organisatieRepository;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
       


    }
}
