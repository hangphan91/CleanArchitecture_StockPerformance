using System;
namespace StockPerformance_CleanArchitecture.Models
{
	public class StockPerformanceRequest
	{
		public string Symbol { get; set; }
		public int NumberOfYear { get; set; }

		public StockPerformanceRequest(string symbol, int numberOfYear)
		{
			Symbol = symbol;
			NumberOfYear = numberOfYear;
		}
	}
}

