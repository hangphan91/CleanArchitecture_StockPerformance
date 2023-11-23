using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic.Mappers
{
	public class StockPerformanceSummaryMapper
	{
		public StockPerformanceSummaryMapper()
		{
		}
        public static EntityDefinitions.PerformanceSummary Map(StockPerformanceSummary result)
        {
            return new EntityDefinitions.PerformanceSummary
            {
                Date = DateTime.Now,
                ProfitInDollar = result.ProfitInDollar,
                ProfitInPercentage = result.ProfitInPercentage,
                ProfitCurrency = "$",
                Symbol = result.Symbol,
                TotalBalance = result.TotalBalanceAfterLoss,
                TotalBalanceInPosition = result.TotalBalanceHoldingInPosition,
                TotalDeposit = result.TotalDeposit,
                CurrentPrice = result.CurrentPrice
            };
        }
    }
}

