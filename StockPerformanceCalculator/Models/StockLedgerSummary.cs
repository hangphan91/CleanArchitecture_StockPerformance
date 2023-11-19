using System;
namespace StockPerformanceCalculator.Models
{
	public class StockLedgerSummary
	{
		public decimal CurrentProfit { get; set; }
		public List<StockLedger> StockLedgers { get; set; }
		public Profit Profit { get; set; }
		public BalanceHolding BalanceHolding { get; set; }
		public List<DepositLedger> DepositLedgers { get; set; }

		public StockLedgerSummary()
		{
		}
	}
}

