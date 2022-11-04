using System.Diagnostics;
using Lab3.Data;
using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Controllers;

public class HomeController : Controller
{
    private ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}