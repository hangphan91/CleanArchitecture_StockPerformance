using System;
using System.Text;

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
        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"Purchase limitation: {PurchaseLimitation}," +
                $" sell percentage limitation : {SellPercentageLimitation}" +
                $" buy percentage limitation: {BuyPercentageLimitation}" +
                $" lower range of trading date: {LowerRangeOfTradingDate}" +
                $" higher range of trading date: {HigherRangeOfTradingDate}");
            return str.ToString();
        }

        internal bool IsSame(TradingRule tradingRule)
        {
            if (tradingRule == null)
                return false;

            return tradingRule.PurchaseLimitation == PurchaseLimitation
                && tradingRule.SellPercentageLimitation == SellPercentageLimitation
                && tradingRule.BuyPercentageLimitation == BuyPercentageLimitation
                && tradingRule.LowerRangeOfTradingDate == LowerRangeOfTradingDate
                && tradingRule.HigherRangeOfTradingDate == HigherRangeOfTradingDate
                && tradingRule.LossLimitation == LossLimitation;
        }
    }
}

