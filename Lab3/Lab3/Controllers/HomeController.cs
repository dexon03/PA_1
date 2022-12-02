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
        var treeList = _bTree.ToList();
        var checkNode = treeList.FirstOrDefault(u => u.NodeValueId == node.NodeValueId);
        if (checkNode != null)
        {
            return View("ErrorAdd","Node with this id already exist");
        }
        this._bTree.BTreeInsert(node);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult SearchNode()
    {
        return View(new NodeValue());
    }
    
    [HttpPost]
    public IActionResult SearchNode(NodeValue node)
    {
        int _countOfComparsion = 0;
        var result = _bTree.BTreeSearch(node.NodeValueId, ref _countOfComparsion);
        if (result != null)
        {
            ViewBag.CountOfComparsion = _countOfComparsion;
            
            return View(result);
        }
        else return View("ErrorAdd","Node isn't found");
    }
    public IActionResult Remove(int id)
    {
        _bTree.DeleteNode(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        int countOfComparsion = 0;
        var node = _bTree.BTreeSearch(id,ref countOfComparsion);
        if (node is null) return NotFound();
        
        return View(node);
    }
    [HttpPost]
    public IActionResult Edit(NodeValue nodeValue)
    {
        _bTree.Edit(nodeValue);
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult SaveToDb()
    {
        _dbContext.NodeValues.RemoveRange(_dbContext.NodeValues);
        _dbContext.NodeValues.AddRange(_bTree.ToList());
        _dbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}