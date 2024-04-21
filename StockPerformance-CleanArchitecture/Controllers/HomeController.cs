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

    public IActionResult Index(string symbol, bool useDefaultSetting,
        int startYear = 2020, string Name = null)
    {
        var currentSearchDetail = _searchDetailManager.SetInitialView(symbol, startYear, useDefaultSetting, Name);

        //return View(currentSearchDetail);

        return View("./Views/Home/Index.cshtml", currentSearchDetail);
    }

    [HttpGet]
    public IActionResult SaveSearchDetailSetup(SearchDetail searchDetail)
    {
        var currentSearchSetup = _searchDetailManager.GetInitialSearchDetail();
        currentSearchSetup.ActiveSelectedSearchDetails = _searchDetailManager.GetActiveSearchDetails();
        return View(currentSearchSetup);

    }
    [HttpGet]

    public IActionResult SaveCustomizedSearchDetailSetup(SearchDetail searchDetail)
    {
        _searchDetailManager.SaveSearchDetail(searchDetail);
        return View(searchDetail);
    }
    [HttpGet]

    public IActionResult GetAllSearchDetailSetup()
    {
        var toView = _searchDetailManager.GetAllSavedSearchDetails();
        return View(toView);
    }
    public IActionResult Symbol(SearchInitialSetup searchSetup)
    {
        searchSetup = _searchDetailManager.AddSymbol(searchSetup);
        return View(searchSetup);
    }

    public IActionResult AdvanceSearch(AdvanceSearch advanceSearch,
        bool willClearAllSearch)
    {
        var result = _searchDetailManager.AddAdvanceSearch(advanceSearch,
            willClearAllSearch);

        return View(result);

    }

    public IActionResult DefaultAdvanceSearch()
    {
        var result = _searchDetailManager.GetInitialSearchDetail();
        result.ActiveSelectedSearchDetails = _searchDetailManager.GetActiveSearchDetails();
        result.SavedSearchDetails = _searchDetailManager.GetAllSavedSearchDetails();

        return View(new AdvanceSearch { SearchDetail = result });

    }

    public IActionResult HowToUse()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

