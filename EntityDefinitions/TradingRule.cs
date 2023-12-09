using System;
namespace EntityDefinitions
{
	public class TradingRule :IdBase
	{
        public decimal PurchaseLimitation { get; set; }
        public decimal SellPercentageLimitation { get; set; }
        public decimal BuyPercentageLimitation { get; set; }
        public int LowerRangeOfTradingDate { get; set; }
        public int HigherRangeOfTradingDate { get; set; }
        public decimal LossLimitation { get; set; }
        public int NumberOfTradeAMonth { get; set; }
        public decimal SellAllWhenPriceDropAtPercentageSinceLastTrade { get; set; }

        public TradingRule()
		{
		}
	}
}

