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
                TempData["msg"] = "Added!";
                client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").InsertOne(user);
                Console.WriteLine(user);
                return RedirectToAction("Index");        
            }
            catch (Exception ex)
            {

                TempData["msg"] = "Unable to add user";
                return View("Index");
            }
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
                if(client.GetDatabase("SWMG").GetCollection<Portfolio>("UserFinances").Find(x => x.username == port.username && x.name == port.name).ToList().Count == 0) {
                    client.GetDatabase("SWMG").GetCollection<Portfolio>("UserFinances").InsertOne(port);
                }
                else {
                    //client.GetDatabase("SWMG").GetCollection<Portfolio>("UserFinances").updateOne();
                }
                Console.WriteLine(port);
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