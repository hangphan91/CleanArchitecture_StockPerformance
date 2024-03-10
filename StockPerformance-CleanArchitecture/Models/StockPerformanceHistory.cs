using System;
using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Formatters;

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

		public string DisplayPerformanceResult()
		{
			return PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(StockPerformanceResponses);
        }
	}
}

