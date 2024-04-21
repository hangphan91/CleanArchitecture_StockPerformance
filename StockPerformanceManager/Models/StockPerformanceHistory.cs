using System;
using System.Collections.Concurrent;
using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.Storages;

namespace StockPerformance_CleanArchitecture.Models
{
	public class StockPerformanceHistory
	{
		public List<StockPerformanceResponse> StockPerformanceResponses  =>
		StockPerformanceHistoryStorage.StockPerformanceResponses
		?.Select(a => a)
		?.OrderBy(a => a.Symbol)
        ?.ThenBy(a => a.SearchDetail.SettingDate.Year)
		?.ToList();

        public ProfitChart ProfitChart { get; set; }

        public StockPerformanceHistory()
		{
		}

		public string DisplayPerformanceResult()
		{
			return PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(StockPerformanceResponses);
        }
	}
}

