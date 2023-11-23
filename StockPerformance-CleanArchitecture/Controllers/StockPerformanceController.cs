using System.Diagnostics;
using EntityPersistence.DataAccessors;
using Microsoft.AspNetCore.Mvc;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.Logic;

namespace StockPerformance_CleanArchitecture.Controllers
{
    public class StockPerformanceController : Controller
    {
        private readonly ILogger<StockPerformanceController> _logger;
        private readonly IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        private readonly SearchDetailManager _searchDetailManager;

        public StockPerformanceController(ILogger<StockPerformanceController> logger)
        {
            _logger = logger;
            _entityDefinitionsAccessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            _searchDetailManager = ManagerHelper.SearchDetailManager;
        }

        public async Task<IActionResult> StockPerformance(SearchDetail searchDetail)
        {
            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}

