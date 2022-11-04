using System.Diagnostics;
using Lab3.Data;
using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Controllers;

public class HomeController : Controller
{
    private ApplicationDbContext _db;
    private BTree _bTree;
    public HomeController(ApplicationDbContext db,BTree bTree)
    {
        _db = db;
        _bTree = bTree;
    }

    public string Index()
    {
        if (_db.NodeValues.Count() > 0)
        {
            return "Not empty";
        }

        return "Empty";
        // return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}