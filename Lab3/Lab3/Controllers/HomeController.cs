using System.Diagnostics;
using Lab3.Data;
using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Controllers;

public class HomeController : Controller
{
    private ApplicationDbContext _dbContext;
    private BTree _bTree;
    public HomeController(ApplicationDbContext db,BTree bTree)
    {
        _dbContext = db;
        _bTree = bTree;

    }

    public IActionResult Index()
    {
        return View(_bTree.ToList());
    }


    public IActionResult Add()
    {
        return View(new NodeValue());
    }

    [HttpPost]
    public IActionResult Add(NodeValue node)
    {
        this._bTree.BTreeInsert(node);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int id)
    {
        _bTree.Delete(_bTree.Root,id);
        return RedirectToAction(nameof(Index));
    }
    
    // [HttpPost]
    // public IActionResult Remove(int id)
    // {
    //     _bTree.Delete(_bTree.Root,id);
    //     return RedirectToAction(nameof(Index));
    // }
    //
    // public IActionResult Search()
    // {
    //     return View();
    // }
    //
    // [HttpPost]
    // public IActionResult Search(int id)
    // {
    //     return RedirectToAction(nameof(Index));
    // }
    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}