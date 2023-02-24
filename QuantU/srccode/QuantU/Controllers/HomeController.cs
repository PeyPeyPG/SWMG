using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuantU.Models;
using AppData;
using DataPartition;

namespace QuantU.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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

    public async Task<IActionResult> Search(string searchString){
        if (searchString == VALID){
            var data = DataPartition.pullAPIData("APIURL" + searchString);
            var stock = AppData(data);
            return View();
        }
    }
{

}

