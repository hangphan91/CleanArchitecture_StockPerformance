using System;
namespace StockPerformance_CleanArchitecture.Models.Settings
{
	public class TradingRule
	{
        public decimal PurchaseLimitation { get; set; }
        public decimal SellPercentageLimitation { get; set; }
        public decimal BuyPercentageLimitation { get; set; }
        public int LowerRangeOfTradingDate { get; set; }
        public int HigherRangeOfTradingDate { get; set; }
        public decimal LossLimitation { get; set; }
        public TradingRule()
		{
		}
	}
}

