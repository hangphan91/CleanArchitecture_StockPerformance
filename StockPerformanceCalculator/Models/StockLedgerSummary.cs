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
			StockLedgers = new List<StockLedger>();
			BalanceHolding = new BalanceHolding();
			DepositLedgers = new List<DepositLedger>();
			Profit = new Profit();
		}
	}
}

