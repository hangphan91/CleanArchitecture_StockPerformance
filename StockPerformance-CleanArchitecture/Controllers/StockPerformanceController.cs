using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockPerformance_CleanArchitecture.Models;
using StockPerformanceCalculator.Logic;

namespace StockPerformance_CleanArchitecture.Controllers
{
    public class StockPerformanceController : Controller
	{
        private readonly ILogger<StockPerformanceController> _logger;

        public StockPerformanceController(ILogger<StockPerformanceController> logger)
		{
			_logger = logger;
		}

		public IActionResult StockPerformance(string symbol = "AAPL", int year = 2020)
		{
			var request = new StockPerformanceRequest(symbol, year);

            var performanceMangager = new StockPerformanceManager(symbol, year);
            var summary = performanceMangager.StartStockPerforamanceCalculation();

            var response = new StockPerformanceResponse(symbol, year);
            response = response.Map(summary);

            return View(response);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

