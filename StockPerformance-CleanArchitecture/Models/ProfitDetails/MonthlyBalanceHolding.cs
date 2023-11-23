using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class MonthlyBalanceHolding :BalanceHolding
	{
        public int Year { get; set; }
        public int Month { get; set; }
        public MonthlyBalanceHolding()
		{
		}
	}
}

