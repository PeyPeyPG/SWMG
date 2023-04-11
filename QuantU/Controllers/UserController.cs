using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuantU.Models;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                if (client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(x => x.username == user.username).ToList().Count == 0)
                {
                    client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").InsertOne(user);
                    UserFinances uf = new UserFinances(user.username);
                    client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").InsertOne(uf);
                }
                else
                {
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

                //Aidan code
                TempData["msg"] = "Added!";
                //client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").InsertOne(user);
                Console.WriteLine(user.username);
                // return RedirectToAction();  

                //Peyton code
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine(userId);
                userId = UserInfo.DecryptSingle(userId);
                user.username = userId;
                Console.WriteLine(user.netSalary);
                Console.WriteLine(user.username);
                var filter = Builders<UserFinances>.Filter.Eq("username", user.username);
                var result = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter).ToList();
                if (result.Count > 0)
                {
                    foreach (UserFinances uf in result)
                    {
                        user._id = uf._id;
                        user.portfolioList = uf.portfolioList;
                    }
                }
                filter = Builders<UserFinances>.Filter.Eq(u => u._id, user._id);
                client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").ReplaceOne(filter, user);
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
            //Checks if there is a user saved in the cookies
            ClaimsPrincipal claimUser = HttpContext.User;

            //Returns username
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //If the user is saved the login in redirects you automatically
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /*
            Uses the variables given by the LogIn.cshtml page to parse the database
            If a match is found the user is logged in via cookies and their information is stored for later use
        */
        [HttpPost]
        public async Task<IActionResult> LogIn(UserInfo user)
        {
            //Creates username and password variables of the encrypted and hashed values given by the user in LogIn.cshtml
            string username = UserInfo.DecryptSingle(user.username);
            string password = UserInfo.HashedSingle(user.password);

            //Creates filter to parse data base for username and password
            FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", username) & Builders<UserInfo>.Filter.Eq("password", password);
            //Parsing the database using the filter and saving the results to a list
            List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
            //If the list's length is not zero authenication begins   
            if (results.Count != 0)
            {
                //Username is decrypted for later storage
                username = UserInfo.DecryptSingle(username);
                //New list of claims is made with identifier username to be access later
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, username),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                //Authentication properties (like letting the user stay signed in) are made 
                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    // remember me button
                    IsPersistent = true
                };

                //Assigning the authentication to cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                //redirect to the home page
                return RedirectToAction("Index", "Home");
            }

    
    public async Task<IActionResult> LogOut(){
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("LogIn", "User");
    }

    public IActionResult Account(){
        //Gets username from cookies and saves it as userId
        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //userId is encrypted
        userId = UserInfo.DecryptSingle(userId);
        //creates a filter to look for the matching username in the database
        FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", userId);
        List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
        UserInfo user = new UserInfo();
        foreach(UserInfo result in results){
            user = result;
        }

        public IActionResult Account()
        {
            //Gets username from cookies and saves it as userId
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            //userId is encrypted
            userId = UserInfo.DecryptSingle(userId);
            //creates a filter to look for the matching username in the database
            FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", userId);
            List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
            UserInfo user = new UserInfo();
            foreach (UserInfo result in results)
            {
                user = result;
            }
            user = UserInfo.EncryptAlgo(user);
            ViewBag.user = user;

            FilterDefinition<UserFinances> filter2 = Builders<UserFinances>.Filter.Eq("username", userId);
            List<UserFinances> results2 = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter2).ToList();
            UserFinances uf = new UserFinances();
            foreach (UserFinances result in results2)
            {
                uf = result;
            }
            ViewBag.uf = uf;

            return View();
        }

        public IActionResult changeEmail(string newEmail, string password)
        {


            string encryptPass = UserInfo.HashedSingle(password);
            //Gets username from cookies and saves it as userId
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            //userId is encrypted
            userId = UserInfo.DecryptSingle(userId);
            //creates a filter to look for the matching username in the database
            FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", userId);
            List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
            UserInfo user = new UserInfo();
            foreach (UserInfo result in results)
            {
                user = result;
            }
            //decrypted user
            user = UserInfo.EncryptAlgo(user);
            ViewBag.user = user;

            string changedEmail = UserInfo.DecryptSingle(newEmail);

            FilterDefinition<UserFinances> filter2 = Builders<UserFinances>.Filter.Eq("username", userId);
            List<UserFinances> results2 = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter2).ToList();
            UserFinances uf = new UserFinances();
            foreach (UserFinances result in results2)
            {
                uf = result;
            }
            ViewBag.uf = uf;

            //update

            try
            {
                FilterDefinition<UserInfo> filter4 = filter & Builders<UserInfo>.Filter.Eq("password", encryptPass);
                List<UserInfo> results4 = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter4).ToList();
                if (results4.Count != 0)
                {
                    var update = Builders<UserInfo>.Update.Set("email", changedEmail);
                    client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").UpdateOne(filter, update);
                }
                else
                {
                    throw new Exception();
                }
                TempData["msg"] = "Email updated!";
                return RedirectToAction("Account");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Unable to update email; incorrect password.";
                return RedirectToAction("Account");
            }



            Console.WriteLine(newEmail);
            Console.WriteLine(password);
        }


         public IActionResult changePass(string newPass, string recoveryA)
        {


            string encryptRec = UserInfo.HashedSingle(recoveryA);
            //Gets username from cookies and saves it as userId
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            //userId is encrypted
            userId = UserInfo.DecryptSingle(userId);
            //creates a filter to look for the matching username in the database
            FilterDefinition<UserInfo> filter = Builders<UserInfo>.Filter.Eq("username", userId);
            List<UserInfo> results = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter).ToList();
            UserInfo user = new UserInfo();
            foreach (UserInfo result in results)
            {
                user = result;
            }
            //decrypted user
            user = UserInfo.EncryptAlgo(user);
            ViewBag.user = user;

            string changedPass =  UserInfo.HashedSingle(newPass);

            FilterDefinition<UserFinances> filter2 = Builders<UserFinances>.Filter.Eq("username", userId);
            List<UserFinances> results2 = client.GetDatabase("SWMG").GetCollection<UserFinances>("UserFinances").Find(filter2).ToList();
            UserFinances uf = new UserFinances();
            foreach (UserFinances result in results2)
            {
                uf = result;
            }
            ViewBag.uf = uf;

            //update

            try
            {
                FilterDefinition<UserInfo> filter4 = filter & Builders<UserInfo>.Filter.Eq("recovery answer", encryptRec);
                List<UserInfo> results4 = client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").Find(filter4).ToList();
                if (results4.Count != 0)
                {
                    var update = Builders<UserInfo>.Update.Set("password", changedPass);
                    client.GetDatabase("SWMG").GetCollection<UserInfo>("UserInfo").UpdateOne(filter, update);
                }
                else
                {
                    throw new Exception();
                }
                TempData["msg"] = "Password updated!";
                return RedirectToAction("Account");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Unable to update Password; incorrect recovery answer.";
                return RedirectToAction("Account");
            }



            Console.WriteLine(newPass);
            Console.WriteLine(recoveryA);
        }

        public IActionResult Paper()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult Paper(Portfolio port)
        // {
        //     if (!ModelState.IsValid)
        //         {
        //             return View("Paper");
        //         }
        //         try
        //         {
        //             FilterDefinition<Portfolio> filter = Builders<Portfolio>.Filter.Eq("name", port.name) & Builders<Portfolio>.Filter.Eq("username", port.username);
        //             List<Portfolio> results = client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").Find(filter).ToList();
        //             if(results.Count == 0) {
        //                 client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").InsertOne(port);
        //             }
        //             else {
        //                 UpdateDefinition<Portfolio> updateStock = Builders<Portfolio>.Update.AddToSet<string>("stocks", port.stocks[0]);
        //                 client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").UpdateOne(filter, updateStock);
        //                 UpdateDefinition<Portfolio> updateInvest = Builders<Portfolio>.Update.AddToSet<int>("investments", port.investments[0]);
        //                 client.GetDatabase("SWMG").GetCollection<Portfolio>("Portfolio").UpdateOne(filter, updateInvest);     
        //             }

        //             TempData["msg"] = "Added!";
        //             return RedirectToAction("Paper"); 



        //         }
        //         catch (Exception ex)
        //         {

        //             TempData["msg"] = "Unable to add stock";
        //             return View("Paper");
        //         } 
        // }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}