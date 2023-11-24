using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class ProfitCalculator
    {
        private StockLedgerCalculator _stockLedgerCalculator;

        private List<ProfitByMonth> ProfitByMonths { get; set; }
        private List<ProfitByYear> ProfitByYears { get; set; }

        private Dictionary<string, ProfitByMonth> ProfitByMonthDictionary { get; set; }
        private Dictionary<string, ProfitByYear> ProfitByYearDictionary { get; set; }

        public ProfitCalculator(StockLedgerCalculator stockLedgerCalculator)
        {
            _stockLedgerCalculator = stockLedgerCalculator;

            ProfitByMonths = new List<ProfitByMonth>();
            ProfitByYears = new List<ProfitByYear>();
            ProfitByYearDictionary = new Dictionary<string, ProfitByYear>();
            ProfitByMonthDictionary = new Dictionary<string, ProfitByMonth>();
        }

        internal void Calculate(decimal currentPrice)
        {
            var stockLedger = _stockLedgerCalculator.GetStockLedger();

            var closedPositions = stockLedger.ClosedLedgers;
            GetProfitFromClosedLedgers(closedPositions);

            var holdingLedgers = stockLedger.HoldingLedgers;
            GetProfitFromHoldingLedger(holdingLedgers, currentPrice);

            CalculateProfitByYears();
            CalculateProfitByMonths();
        }

        private void CalculateProfitByMonths()
        {
            ProfitByMonths.ForEach(profit =>
            {
                var key = profit.Year.ToString() + profit.Month.ToString();

                if (!ProfitByMonthDictionary.ContainsKey(key))
                    ProfitByMonthDictionary.Add(key, profit);
                else
                    ProfitByMonthDictionary[key].Amount += profit.Amount;
            });
        }

        private void CalculateProfitByYears()
        {
            ProfitByYears.ForEach(profit =>
            {
                var key = profit.Year.ToString();

                if (!ProfitByYearDictionary.ContainsKey(key))
                    ProfitByYearDictionary.Add(key, profit);
                else
                    ProfitByYearDictionary[key].Amount += profit.Amount;
            });
        }

        private void GetProfitFromHoldingLedger(List<StockLedgerDetail> holdingLedgers, decimal currentPrice)
        {
            foreach (var holdingLedger in holdingLedgers.OrderBy(a => a.BoughtDate))
            {
                var amount = (currentPrice - holdingLedger.BoughtPrice) * holdingLedger.ShareCount;
                ProfitByMonths.Add(new ProfitByMonth
                {
                    Month = holdingLedger.BoughtDate.Month,
                    Year = holdingLedger.BoughtDate.Year,
                    Amount = amount,
                });

                ProfitByYears.Add(new ProfitByYear
                {
                    Amount = amount,
                    Year = holdingLedger.BoughtDate.Year,
                });
            }
        }

        private void GetProfitFromClosedLedgers(List<StockLedgerDetail> closedLedgers)
        {
            foreach (var closedLedger in closedLedgers.OrderBy(a =>a.BoughtDate))
            {
                if (closedLedger.SoldDate.HasValue)
                {
                    var amount = (closedLedger.SoldPrice - closedLedger.BoughtPrice) * closedLedger.ShareCount;
                    ProfitByMonths.Add(new ProfitByMonth
                    {
                        Month = closedLedger.SoldDate?.Month ?? 0,
                        Year = closedLedger.SoldDate?.Year ?? 0,
                        Amount = amount,
                    });

                    ProfitByYears.Add(new ProfitByYear
                    {
                        Amount = amount,
                        Year = closedLedger.SoldDate?.Year ?? 0,
                    });
                }
            }
        }

        internal List<ProfitByYear> GetProfitByYear()
        {
            return ProfitByYearDictionary.Values.ToList();
        }

        internal List<ProfitByMonth> GetProfitByMonth()
        {
            return ProfitByMonthDictionary.Values.ToList();
        }

    }
}

