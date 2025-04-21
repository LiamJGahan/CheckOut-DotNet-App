using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CheckOut.Models;
using Microsoft.EntityFrameworkCore;
using CheckOut.Data;

namespace CheckOut.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CheckOutContext _context;

    // Logger is not being used but is being left in for now
    public HomeController(ILogger<HomeController> logger, CheckOutContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId != null)
        {
            var checklists = _context.Checklists
                .Include(checklist => checklist.ToDos)
                .Where(checklist => checklist.UserId == userId)
                .Where(checklist => !checklist.IsArchived)
                .ToList();

            return View(checklists);
        }

        return View(new List<Checklist>());
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

    [HttpGet("GetUsername")]
    public IActionResult SessionTest()
    {
        string? user = HttpContext.Session.GetString("Username");
        return Content(user);
    }
}
