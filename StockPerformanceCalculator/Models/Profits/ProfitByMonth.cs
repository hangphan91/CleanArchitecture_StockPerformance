using System;
namespace StockPerformanceCalculator.Models
{
	public class ProfitByMonth :Profit
	{
        public int Month { get; set; }
        public int Year { get; set; }
        public ProfitByMonth()
		{
		}
	}
}

