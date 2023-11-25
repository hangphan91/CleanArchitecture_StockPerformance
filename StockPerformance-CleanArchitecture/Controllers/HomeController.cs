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

    public IActionResult Index()
    {
        var currentSearchDetail = _searchDetailManager.GetCurrentSearchDetail();
        return View(currentSearchDetail);
    }

    public IActionResult Symbol(SearchInitialSetup searchSetup)
    {
        var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
        var detail = SearchDetailHelper.GetCurrentSearchDetail(accessor);

        if (!string.IsNullOrWhiteSpace(searchSetup?.AddSymbol))
        {
            searchSetup.AddingSymbols.Add(searchSetup.AddSymbol);
            _searchDetailManager.AddNewSymbols(searchSetup, detail.SearchSetup.Symbols);
            searchSetup.StartingYear = detail.SearchSetup.StartingYear;
            searchSetup.EndingYear = detail.SearchSetup.EndingYear;
            detail.SearchSetup = searchSetup;
        }
        else
            searchSetup = detail.SearchSetup;
        SearchDetailHelper.SetCurrentSearchDetail(detail);
        return View(searchSetup);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

