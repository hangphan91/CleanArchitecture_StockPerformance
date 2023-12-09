using System;
using StockPerformanceCalculator.Models.PerformanceCalculatorSetup;

namespace StockPerformanceCalculator.Logic.Mappers
{
	public class SearchDetailMapper
	{
		public SearchDetailMapper()
		{
		}
        public static EntityDefinitions.PerformanceSetup MapPerformanceSetup(
           InitialPerformanceSetup mapped, List<long> symbolIds)
        {
            return new EntityDefinitions.PerformanceSetup
            {
                StartingYear = mapped.StartingYear,
                EndingYear = mapped.EndingYear,
                EndingSymbolId = symbolIds.Max(),
                StartingSymbolId = symbolIds.Min(),
            };
        }

        public static EntityDefinitions.DepositRule MapDepositRule(InitialPerformanceSetup mapped)
        {
            return new EntityDefinitions.DepositRule
            {
                DepositAmount = mapped.DepositAmount,
                FirstDepositDate = mapped.FirstDepositDate,
                NumberOfDepositDate = mapped.NumberOfDepositDate,
                SecondDepositDate = mapped.SecondDepositDate,
                InitialDepositAmount = mapped.InitialDepositAmount,
            };
        }

        public static EntityDefinitions.TradingRule MapTradingRule(InitialPerformanceSetup mapped)
        {
            return new EntityDefinitions.TradingRule
            {
                BuyPercentageLimitation = mapped.BuyPercentageLimitation,
                HigherRangeOfTradingDate = mapped.HigherRangeOfTradingDate,
                LowerRangeOfTradingDate = mapped.LowerRangeOfTradingDate,
                PurchaseLimitation = mapped.PurchaseLimitation,
                SellPercentageLimitation = mapped.SellPercentageLimitation,
                LossLimitation = mapped.LossLimitation,
                NumberOfTradeAMonth = mapped.NumberOfTradeAMonth,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = mapped.SellAllWhenPriceDropAtPercentageSinceLastTrade,
            };
        }
    }
}

