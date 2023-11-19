using System;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class DepositLedgerCalculator
    {
        private List<DepositLedger> _deposits;

        public DepositLedgerCalculator(int year)
        {
            var startingDate = new DateTime(year, 1, 1);
            _deposits = SetUpDepositLeggerFromDate(startingDate);
        }

        internal static List<DepositLedger> SetUpDepositLeggerFromDate(DateTime startingDate)
        {
            var depositLedgers = new List<DepositLedger>();

            var startMonth = startingDate.Month;
            var startYear = startingDate.Year;

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month;

            for (int currentYear = startYear; currentYear <= endYear; currentYear++)
            {
                for (int currentMonth = 1; currentMonth <= 12; currentMonth++)
                {
                    if (startMonth > currentMonth && currentYear == startYear)
                        continue;

                    if (currentYear == endYear && currentMonth == endMonth)
                        return depositLedgers;

                    var depositLedgersToAdd = GetDepositLedgers(currentYear, currentMonth);

                    depositLedgers.AddRange(depositLedgersToAdd);
                }
            }

            return depositLedgers;
        }

        private static List<DepositLedger> GetDepositLedgers(int year, int month)
        {
            var depositDate = new DateTime(year, month, DepositRule.GetFirstDepositDate());
            var firstDeposit = new DepositLedger
            {
                Amount = DepositRule.GetDepositAmount(),
                Date = depositDate,
            };

            depositDate = new DateTime(year, month, DepositRule.GetSecondDepositDate());
            var secondDeposit = new DepositLedger
            {
                Amount = DepositRule.GetDepositAmount(),
                Date = depositDate,
            };
            return new List<DepositLedger> { firstDeposit, secondDeposit };
        }

        internal List<DepositLedger> GetAllDeposit()
        {
            return _deposits;
        }
    }
}

