using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private SearchDetailManager _searchDetailManager;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _searchDetailManager = ManagerHelper.SearchDetailManager;
    }

    public IActionResult Index(string symbol, bool useDefaultSetting)
    {
        var currentSearchDetail = _searchDetailManager.SetInitialView(symbol,useDefaultSetting);

        return View(currentSearchDetail);
    }

    public IActionResult Symbol(SearchInitialSetup searchSetup)
    {
        searchSetup = _searchDetailManager.AddSymbol(searchSetup);
        return View(searchSetup);
    }

    public IActionResult AdvanceSearch(AdvanceSearch advanceSearch,
        bool willClearAllSearch, bool willPerformSearch)
    {
       var result = _searchDetailManager.UpdateAdvanceSearch(advanceSearch, willClearAllSearch, willPerformSearch);
       
        return View(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

