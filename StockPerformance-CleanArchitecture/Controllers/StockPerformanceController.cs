using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Controllers
{
    public class StockPerformanceController : Controller
    {
        private readonly ILogger<StockPerformanceController> _logger;
        private readonly SearchDetailManager _searchDetailManager;

        public StockPerformanceController(ILogger<StockPerformanceController> logger)
        {
            _logger = logger;
            _searchDetailManager = ManagerHelper.SearchDetailManager;
        }

        public async Task<IActionResult> StockPerformance(SearchDetail searchDetail)
        {
            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);
            return View(response);
        }

        public IActionResult StockPerformanceHistory()
        {
            var history = _searchDetailManager.GetStockPerformanceHistory();
            return View(history);
        }

        public async Task<IActionResult> AdvanceSearch()
        {
            var advancedSearchResult = await _searchDetailManager.PerformAdvanceSearch();

            return View(advancedSearchResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

