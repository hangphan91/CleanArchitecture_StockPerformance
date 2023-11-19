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

            var boughtLedgers = stockLedger.BoughtLedgers;
            GetProfitFromBoughtLedgers(boughtLedgers);

            var soldLedgers = stockLedger.SoldLedgers;
            GetProfitFromSoldLedgers(soldLedgers);

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
            foreach (var boughtLedger in holdingLedgers)
            {
                var amount = currentPrice * boughtLedger.ShareCount;
                ProfitByMonths.Add(new ProfitByMonth
                {
                    Month = boughtLedger.Date.Month,
                    Year = boughtLedger.Date.Year,
                    Amount = amount,
                });

                ProfitByYears.Add(new ProfitByYear
                {
                    Amount = amount,
                    Year = boughtLedger.Date.Year,
                });
            }
        }

        private void GetProfitFromSoldLedgers(List<StockLedgerDetail> soldLedgers)
        {
            foreach (var boughtLedger in soldLedgers)
            {
                var amount = boughtLedger.Price * boughtLedger.ShareCount;
                ProfitByMonths.Add(new ProfitByMonth
                {
                    Month = boughtLedger.Date.Month,
                    Year = boughtLedger.Date.Year,
                    Amount = amount,
                });

                ProfitByYears.Add(new ProfitByYear
                {
                    Amount = amount,
                    Year = boughtLedger.Date.Year,
                });
            }
        }

        private void GetProfitFromBoughtLedgers(List<StockLedgerDetail> boughtLedgers)
        {
            foreach (var boughtLedger in boughtLedgers)
            {
                var amount = boughtLedger.Price * boughtLedger.ShareCount;
                ProfitByMonths.Add(new ProfitByMonth
                {
                    Month = boughtLedger.Date.Month,
                    Year = boughtLedger.Date.Year,
                    Amount = -amount,
                });

                ProfitByYears.Add(new ProfitByYear
                {
                    Amount = -amount,
                    Year = boughtLedger.Date.Year,
                });
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

