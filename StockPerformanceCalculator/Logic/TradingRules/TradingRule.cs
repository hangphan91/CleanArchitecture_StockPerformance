namespace StockPerformanceCalculator.Logic.TradingRules
{
    public class TradingRule
	{
		public static decimal GetPurchaseLimitation()
		{
			return 2000;
		}
		public static bool IsValidForBuyingRule(decimal currentHoldingValue, decimal basicCost)
		{
			// Buy stock on the available date, then continue to buy
			// When Stock Gain 10% overall

			if (currentHoldingValue >= basicCost * (decimal)1.1)
				return true;

			return false;
		}

		public static bool IsValidForSellingRule(decimal currentHoldingValue, decimal basicCost)
        {
			//Sell stock when losing 15% overall
			if (currentHoldingValue * (decimal)1.15 <= basicCost)
				return true;

			return false;
        }

		public static bool IsValidToTradeStockByDate(DateTime date)
		{
			//only buy stock from day 1 to day 10 of each month
			if (date.Day >= 15 && date.Day <= 25)
				return true;

			return false;
		}
    }
}

