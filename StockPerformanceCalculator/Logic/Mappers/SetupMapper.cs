using System;
using EntityDefinitions;
using StockPerformanceCalculator.Models.PerformanceCalculatorSetup;

namespace StockPerformanceCalculator.Logic.Mappers
{
	public class SetupMapper
	{
		public SetupMapper()
		{
		}
        public static InitialPerformanceSetup Map(
           EntityDefinitions.PerformanceSetup performanceSetup,
           List<string> symbols, DepositRule depositRule,
           TradingRule tradingRule)
        {
            return new InitialPerformanceSetup
            {
                EndingYear = performanceSetup.EndingYear,
                StartingYear = performanceSetup.StartingYear,
                Symbols = symbols,
                DepositAmount = depositRule.DepositAmount,
                FirstDepositDate = depositRule.FirstDepositDate,
                SecondDepositDate = depositRule.SecondDepositDate,
                NumberOfDepositDate = depositRule.NumberOfDepositDate,
                BuyPercentageLimitation = tradingRule.BuyPercentageLimitation,
                SellPercentageLimitation = tradingRule.SellPercentageLimitation,
                HigherRangeOfTradingDate = tradingRule.HigherRangeOfTradingDate,
                LowerRangeOfTradingDate = tradingRule.LowerRangeOfTradingDate,
                PurchaseLimitation = tradingRule.PurchaseLimitation,
            };
        }
    }
}

