using System.Diagnostics;
using System.Text.Json;
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

        public async Task<IActionResult> StockPerformance(string json)
        {
            SearchDetail? searchDetail = null;

            if (!string.IsNullOrWhiteSpace(json))
                searchDetail = JsonSerializer.Deserialize<SearchDetail>(json);

            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);
            return View(response);
        }

        public async Task<IActionResult> StockPerformanceFromSelectedDetail(SearchDetail searchDetail, bool useDefaultSetting)
        {
            
            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);

            return View("StockPerformance", response);
        }

        public IActionResult StockPerformanceHistory()
        {
            var history = _searchDetailManager.GetStockPerformanceHistory();
            return View(history);
        }

        public async Task<IActionResult> AdvanceSearch(bool searchAll)
        {
            var advancedSearchResult = await _searchDetailManager.PerformAdvanceSearch(searchAll);

            return View(advancedSearchResult);
        }


        public async Task<IActionResult> SeachAll()
        {
            var advancedSearchResult = await _searchDetailManager.PerformAdvanceSearchForAll();

            return View(advancedSearchResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

