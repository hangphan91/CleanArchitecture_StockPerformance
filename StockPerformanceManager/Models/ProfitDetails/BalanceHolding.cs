using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class BalanceHolding
	{
        public decimal CashAvailable { get; set; }
		public decimal HoldingInPositions { get; set; }

        public BalanceHolding()
		{
		}
	}
}

