using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockPerformance_CleanArchitecture.Models;

namespace StockPerformance_CleanArchitecture.Controllers
{
    public class StockPerformanceController : Controller
	{
        private readonly ILogger<StockPerformanceController> _logger;

        public StockPerformanceController(ILogger<StockPerformanceController> logger)
		{
			_logger = logger;
		}

		public IActionResult StockPerformance(string symbol = "AAPL", int numberOfYear = 2)
		{
			var request = new StockPerformanceRequest(symbol, numberOfYear);

            var response = new StockPerformanceResponse(symbol, numberOfYear);

            return View(response);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

