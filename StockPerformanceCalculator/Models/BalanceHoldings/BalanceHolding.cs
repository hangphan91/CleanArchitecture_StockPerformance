using System;
namespace StockPerformanceCalculator.Models
{
	public class BalanceHolding
	{
		public decimal CashAvailable { get; set; }
		public decimal BalanceInPositions { get; set; }

		public DateTime Date { get; set; }
		public BalanceHolding()
		{
		}
	}
}

