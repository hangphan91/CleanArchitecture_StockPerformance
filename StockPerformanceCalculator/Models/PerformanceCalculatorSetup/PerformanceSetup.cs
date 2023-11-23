using System;
namespace StockPerformanceCalculator.Models.PerformanceCalculatorSetup
{
    public class PerformanceSetup
	{
        public int StartingYear { get; set; }
        public int EndingYear { get; set; }
        public string Symbol { get; set; }
        public PerformanceSetup()
		{
		}
	}
}

