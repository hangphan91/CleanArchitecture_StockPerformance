using System;
namespace StockPerformance_CleanArchitecture.Models
{
	public class StockPerformanceHistory
	{
		public List<StockPerformanceResponse> StockPerformanceResponses { get; set; }
		public StockPerformanceHistory()
		{
			StockPerformanceResponses = new List<StockPerformanceResponse>();
		}
	}
}

