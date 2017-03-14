
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using MongoDB.Driver;
using MongoDB.Bson;

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
            //var client = new MongoClient();
            //var database = client.GetDatabase("Kairos");
            //var collection = database.GetCollection<BsonDocument>("organisatie");
            //var document = new BsonDocument
            //{
            //    {"name","MongoDb" },
            //    {"info" , new BsonDocument
            //    {
            //        {"x", 123 },
            //        {"y", 345 }
            //    } }
            //};
            //collection.InsertOne(document);
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
