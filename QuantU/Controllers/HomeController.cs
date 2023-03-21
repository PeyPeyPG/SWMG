using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantU.Models;
using MongoDB.Driver;
using QuantU.Controllers;

namespace QuantU.Controllers;

public class HomeController : Controller
{
    MongoClient client = new MongoClient("mongodb+srv://SWMG:Shawdowwizardmoneygang@swmg.hzzuvlg.mongodb.net/?retryWrites=true&w=majority?connect=replicaSet");    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //Method for Search Page mapped to form in Index.cshtml
    public IActionResult Search(){
        return View();
    }
    
    //Takes the string the user inputed in forms and uses it
    [HttpPost]
    public IActionResult Search(String title){
            try
            {
                //takes the full stock title and parses it for just the ticker
                var ticker = title.Split(", ");
                //ViewBag created to transer ticker to Search page
                ViewBag.ticker = ticker[ticker.Length - 1];
                Console.WriteLine(TempData["loggedin"]);
                return View("Search");        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (TempData["loggedin"].Equals(true)){
                    TempData["loggedin"] = true;
                }
                return View("Index");
            }
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult PaperTrading(){
        return View();
    }
    [HttpPost]
    public IActionResult PaperTrading(Portfolio portfolio){
        Console.WriteLine(portfolio.name);
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

