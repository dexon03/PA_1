using System.Diagnostics;
using Lab3.Data;
using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Controllers;

public class HomeController : Controller
{
    public ApplicationDbContext _db;
    private BTree _bTree;
    public HomeController(ApplicationDbContext db)
    {
        _db = db;
        _bTree = new BTree(_db, 50);
    }

    public IActionResult Index()
    { 
        return View(_db.NodeValues);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}