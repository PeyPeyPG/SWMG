using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantU.Models;
using MongoDB.Driver;
using QuantU.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace QuantU.Controllers;

public class HomeController : Controller
{
    MongoClient client = new MongoClient("mongodb+srv://SWMG:Shawdowwizardmoneygang@swmg.hzzuvlg.mongodb.net/?retryWrites=true&w=majority");    
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

    /*
        Method for PaperTrading.cshtml
        Before the page is load the method checks if a logged in user has any porfolios in the database
        If they do the portfolio list is passed to be displayed on the page
    */
    public IActionResult PaperTrading(){
        //gets username from cookies and saves it to userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //encrypts the username
        userId = UserInfo.DecryptSingle((string)userId);
        //creates a filter to look for the matching username in the database
        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
        //Finds the UserFinance document of the user via username filter and saves it to list
        List<UserFinances> results = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
        
        //saves the portfolioList of the user to ViewBag.portfoliolist to be used in the View
        foreach(UserFinances result in results){
            ViewBag.portfolioList = result.portfolioList;
        }
        
        return View();
    }

    /*
        Method to create a new portfolio from the modal on PaperTrading.cshtml
        Once a portfolio name is submitted a new portfolio object is made
        That object is entered into the databased based off of if the username in the cookies
    */
    [HttpPost]
    public IActionResult PaperTrading(Portfolio portfolio){
        //Gets username from cookies and saves it as userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //userId is encrypted
        userId = UserInfo.DecryptSingle(userId);
        //creates a filter to look for the matching username in the database
        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
        //create an update variable that will add the portfolio to the portfolio list of a document
        UpdateDefinition<UserFinances> update = Builders<UserFinances>.Update.AddToSet<Portfolio>("portfolioList", portfolio);
        //finds the UserFinances document with the filter and adds the portfolio to its portfolio list using the update
        client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").UpdateOne(filter, update);
        
        //Finds the UserFinance document of the user via username filter and saves it to list
        List<UserFinances> results = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
        
        //saves the portfolioList of the user to ViewBag.portfoliolist to be used in the View
        foreach(UserFinances result in results){
            ViewBag.portfolioList = result.portfolioList;
        }

        return View();
    }

    public IActionResult Portfolio()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Portfolio(string portfolioName) {
        //Gets username from cookies and saves it as userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //userId is encrypted
        userId = UserInfo.DecryptSingle(userId);
        
        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
        UserFinances userFinance =  client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).FirstOrDefault();
        Portfolio port = userFinance.portfolioList.FirstOrDefault(p => p.name == portfolioName);


        //rain check
        //this is the next step for displaying what stocks you are following
        Console.WriteLine(port.stocks);
        
        
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

