using System;
using EntityDefinitions;
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

        internal static List<EntityDefinitions.PerformanceByMonth> Map(List<ProfitByMonth> profitByMonths)
        {
            var result = profitByMonths.Select(profitByMonth => new EntityDefinitions.PerformanceByMonth
            {
                Month = profitByMonth.Month,
                Year = profitByMonth.Year,
                Profit = profitByMonth.Amount,
            }).OrderBy(a => a.Year).ThenBy(a =>a.Month).ToList();

            return result;
        }

        internal static List<Deposit> Map(List<DepositLedger> depositLedgers)
        {
            var result = depositLedgers.Select(a => new Deposit
            {
                Amount = a.Amount,
                Date = a.Date,
            }).ToList();
            return result;
        }

        internal static List<Position> Map(StockLedger stockLedger)
        {
            var closed = stockLedger.ClosedLedgers
                .Select(a => Map(a, PositionType.Closed))
                .ToList();

            var holding = stockLedger.HoldingLedgers
                .Select(a => Map(a, PositionType.Holding))
                .ToList();

            var result = new List<Position>();
            result.AddRange(closed);
            result.AddRange(holding);

            return result.OrderBy(a => a.Date).ToList();
        }

        private static Position Map(StockLedgerDetail a, PositionType positionType)
        {
            return new Position
            {
                Date = a.BoughtDate,
                PositionType = positionType,
                Price = a.BoughtPrice,
                ShareCount = a.ShareCount,
            };
        }
    }
}

