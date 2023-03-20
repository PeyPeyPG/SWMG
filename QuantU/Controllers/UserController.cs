using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuantU.Models;
using MongoDB.Driver;

namespace QuantU.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        MongoClient client = new MongoClient("mongodb+srv://SWMG:Shawdowwizardmoneygang@swmg.hzzuvlg.mongodb.net/?retryWrites=true&w=majority");

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    [HttpPost]
    public IActionResult Index(UserInfo user)
    {
        if (!ModelState.IsValid)
            {
                return View("Index");
            }
           try
            {
                user = UserInfo.HashingAlgo(user);
                user = UserInfo.EncryptAlgo(user);
                 if(client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(x => x.username == user.username).ToList().Count == 0) {
                    client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").InsertOne(user);
                    UserFinances uf = new UserFinances(user.username);
                    client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").InsertOne(uf);
                }
                else {
                    throw new Exception();
                }
                TempData["msg"] = "Added!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["msg"] = "Unable to add user; Choose a new username";
                return View("Index");
            }
    }
    public IActionResult UserQuiz()
        {
            return View();
        }
    [HttpPost]
    public IActionResult UserQuiz(UserFinances user)
    {
        if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                TempData["msg"] = "Added!";
                client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").InsertOne(user);
                Console.WriteLine(user);
                return RedirectToAction();        
            }
            catch (Exception ex)
            {

                TempData["msg"] = "Unable to add user";
                return View();
            }
    }
    public IActionResult LogIn()
    {
        return View();
    }
    [HttpPost]
    public IActionResult LogIn(UserInfo user)
    {
        Console.WriteLine(user.username);
        string username = UserInfo.DecryptSingle(user.username);
        string password = UserInfo.HashedSingle(user.password);
        Console.WriteLine(username);
        Console.WriteLine(password);
        FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", username) & Builders<UserInfo>.Filter.Eq("password", password);
        Console.WriteLine("test");
        List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
                if(results.Count != 0) {
                    TempData["loggedin"] = true;
                    TempData["username"] = UserInfo.DecryptSingle(username);
                    Console.WriteLine("works");
                    return RedirectToAction("Index", "Home"); 
                }
        
        return View();
    }

    public IActionResult Paper()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Paper(Portfolio port)
    {
        if (!ModelState.IsValid)
            {
                return View("Paper");
            }
            try
            {
                FilterDefinition<Portfolio> filter = Builders<Portfolio>.Filter.Eq("name", port.name) & Builders<Portfolio>.Filter.Eq("username", port.username);
                List<Portfolio> results = client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").Find(filter).ToList();
                if(results.Count == 0) {
                    client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").InsertOne(port);
                }
                else {
                    UpdateDefinition<Portfolio> updateStock = Builders<Portfolio>.Update.AddToSet<string>("stocks", port.stocks[0]);
                    client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").UpdateOne(filter, updateStock);
                    UpdateDefinition<Portfolio> updateInvest = Builders<Portfolio>.Update.AddToSet<int>("investments", port.investments[0]);
                    client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").UpdateOne(filter, updateInvest);     
                }

                TempData["msg"] = "Added!";
                return RedirectToAction("Paper"); 



            }
            catch (Exception ex)
            {

                TempData["msg"] = "Unable to add stock";
                return View("Paper");
            } 
    }

    


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}