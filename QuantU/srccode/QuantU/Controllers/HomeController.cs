using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantU.Models;
using MongoDB.Driver;
//using AppData;
//using DataPartition;

namespace QuantU.Controllers;

public class HomeController : Controller
{
    MongoClient client = new MongoClient("mongodb+srv://SWMG:Shawdowwizardmoneygang@swmg.hzzuvlg.mongodb.net/?retryWrites=true&w=majority?connect=replicaSet");
    //var UserInfoCollection = client.GetDatabase("SWMG").GetCollection<User>("UserInfo");
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AddUser(){
        return View();
    }
    [HttpPost]
    public IActionResult AddUser(User user)
    {
        if (!ModelState.IsValid)
            {
                return View("Index");
            }
            try
            {
                TempData["msg"] = "Added!";
                client.GetDatabase("SWMG").GetCollection<User>("UserInfo").InsertOne(user);
                Console.WriteLine(user);
                return RedirectToAction("Index");        
            }
            catch (Exception ex)
            {

                TempData["msg"] = ex;
                Console.WriteLine(user.username + " " + user.email + " " + user.password + " " + user.recoveryA + " " + user.recoveryQ);
                return View("Index");
            }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

