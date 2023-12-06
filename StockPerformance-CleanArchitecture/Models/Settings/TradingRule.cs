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
        public int NumberOfTradeAMonth { get; set; }
        public TradingRule()
        {
        }
        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine("Trading rule as:");
            str.AppendLine($"Each trade's purchase limitation is ${PurchaseLimitation}.");
            str.AppendLine($"Sell when we are at {SellPercentageLimitation}% of total investment.");
            str.AppendLine($"Buy when we are gaining {BuyPercentageLimitation}% overall.");
            str.AppendLine($"We will trade between day {LowerRangeOfTradingDate} and {HigherRangeOfTradingDate}.");
            str.AppendLine($"We will trade {NumberOfTradeAMonth} times monthly.");
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
                && tradingRule.LossLimitation == LossLimitation
                && tradingRule.NumberOfTradeAMonth == NumberOfTradeAMonth;
        }
    }
}

