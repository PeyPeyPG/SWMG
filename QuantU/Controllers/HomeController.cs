﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantU.Models;
using MongoDB.Driver;
using QuantU.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System;

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
        //gets username from cookies and saves it to userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //encrypts the username
        userId = UserInfo.DecryptSingle(userId);
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
    
    //Takes the string the user inputed in forms and uses it
    [HttpPost]
    public IActionResult Search(String title){
            try
            {
                //takes the full stock title and parses it for just the ticker
                var ticker = title.Split(", ");
                //ViewBag created to transer ticker to Search page
                ViewBag.ticker = ticker[ticker.Length - 1];

                //gets username from cookies and saves it to userId
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                //encrypts the username
                userId = UserInfo.DecryptSingle(userId);
                //creates a filter to look for the matching username in the database
                FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
                //Finds the UserFinance document of the user via username filter and saves it to list
                List<UserFinances> results = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
                
                //saves the portfolioList of the user to ViewBag.portfoliolist to be used in the View
                foreach(UserFinances result in results){
                    ViewBag.portfolioList = result.portfolioList;
                }
                
                return View("Search");        
            }
            catch (Exception ex)
            {
                return View("Index");
            }
    }

    public IActionResult Index()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        //Returns username
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        userId = UserInfo.DecryptSingle(userId);
        //If the user is saved the login in redirects you automatically
        //if (claimUser.Identity.IsAuthenticated){
        //userId is encrypted
        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
        List<UserFinances> results = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
        UserFinances user = new UserFinances();
        foreach(UserFinances result in results){
            user = result;
        }
        ViewBag.user = user;
        ViewBag.user.username = UserInfo.DecryptSingle(userId);
        //Console.WriteLine(ViewBag.user.age);
        return View();
        // }
        // else{
        //     return RedirectToAction("LogIn", "User");
        // } 
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult test()
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
        userId = UserInfo.DecryptSingle(userId);
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

    public IActionResult PortfolioPage(string name){
        Console.WriteLine(name);
        ViewBag.PortfolioName = name;
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //userId is encrypted
        userId = UserInfo.DecryptSingle(userId);
        //creates a filter to look for the matching username in the database
        UserFinances user = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(u => u.username == userId).FirstOrDefault();
        Portfolio p = user.portfolioList.Find(p => p.name == name);
        ViewBag.portfolio = p;
        return View();
    }

    public IActionResult AddStock(string name){
        Console.WriteLine(name);
        ViewBag.PortfolioName = name;

        //gets username from cookies and saves it to userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //encrypts the username
        userId = UserInfo.DecryptSingle(userId);
        //creates a filter to look for the matching username in the database
        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq("username", userId);
        //Finds the UserFinance document of the user via username filter and saves it to list
        List<UserFinances> results = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
        
        //saves the portfolioList of the user to ViewBag.portfoliolist to be used in the View
        foreach(UserFinances result in results){
            ViewBag.portfolioList = result.portfolioList;
        }
        
        return View("Search");
    }


    public IActionResult AddStockToPort(string PortfolioName, string ticker, int shares) {
         var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
         userId = UserInfo.DecryptSingle(userId);
        UserFinances user = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(u => u.username == userId).FirstOrDefault();
        Portfolio p = user.portfolioList.Find(p => p.name == PortfolioName);
        Console.WriteLine(shares + "\n\n\n\n\n");
        if(p.stocks == null) {
            Console.WriteLine("it is null");
            p.stocks = new List<string>();
            p.share = new List<int>();
            p.dates = new List<string>();
             p.stocks.Add(ticker);
            p.share.Add(shares);
            p.dates.Add(DateTime.Now.ToString());
        }
        else {
            Console.WriteLine("it is not null");
            p.stocks.Add(ticker);
            p.share.Add(shares);
             p.dates.Add(DateTime.Now.ToString());
        }

        FilterDefinition<UserFinances> filter = Builders<UserFinances>.Filter.Eq(u => u.username, userId);
        UpdateDefinition<UserFinances> update = Builders<UserFinances>.Update.Set(u => u.portfolioList, user.portfolioList);
        client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").UpdateOne(filter, update);

        userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //userId is encrypted
        userId = UserInfo.DecryptSingle(userId);
        //creates a filter to look for the matching username in the database
        user = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(u => u.username == userId).FirstOrDefault();
        p = user.portfolioList.Find(p => p.name == PortfolioName);
        ViewBag.portfolio = p;

         

        
        ViewBag.PortfolioName = PortfolioName;
            return View("PortfolioPage");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

