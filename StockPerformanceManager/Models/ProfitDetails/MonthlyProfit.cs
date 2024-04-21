using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class MonthlyProfit : Profit
	{
		public int Month { get; set; }
		public int Year { get; set; }
		public MonthlyProfit()
		{
		}

        public override string DisplayProfit()
        {
            var result = $"In {Month}/{Year} ";
            return result + base.DisplayProfit();
        }
    }
}

