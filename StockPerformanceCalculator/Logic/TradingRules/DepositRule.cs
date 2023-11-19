using System;
namespace StockPerformanceCalculator.Logic.TradingRules
{
	public class DepositRule
	{
		public DepositRule()
		{
		}

		public static int GetFirstDepositDate()
		{
			return 1;
		}

        public static int GetSecondDepositDate()
        {
            return 15;
        }

        public static int NumberOfDepositAMonth()
		{
			return 2;
		}

		public static decimal GetDepositAmount()
		{
			return 1000;
		}
	}
}

