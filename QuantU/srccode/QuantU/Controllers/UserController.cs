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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}