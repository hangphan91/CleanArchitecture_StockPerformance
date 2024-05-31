using System;
using System.Collections.Concurrent;
using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.Storages;

namespace StockPerformance_CleanArchitecture.Models
{
	public class StockPerformanceHistory
	{
		public List<StockPerformanceResponse> StockPerformanceResponses { get; set; }

		public ProfitChart ProfitChart { get; set; }

		public StockPerformanceHistory()
		{
			StockPerformanceResponses = new List<StockPerformanceResponse>();
		}

		public string DisplayPerformanceResult(List<StockPerformanceResponse> stockPerformanceResponses)
		{
			StockPerformanceResponses = stockPerformanceResponses;
			return PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(StockPerformanceResponses);
		}
	}
}

