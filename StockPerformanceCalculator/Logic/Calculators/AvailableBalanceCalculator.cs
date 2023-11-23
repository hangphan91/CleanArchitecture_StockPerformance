using System;
using StockPerformanceCalculator.Models;
using StockPerformanceCalculator.Models.BalanceHoldings;

namespace StockPerformanceCalculator.Logic
{
    public class AvailableBalanceCalculator
    {
        private List<BalanceHolding> _balanceHoldings;
        private DepositLedgerCalculator _depositLedgerCalculator;
        StockLedgerCalculator _stockLedgerCalculator;

        private Dictionary<string, BalanceHoldingByMonth> BalanceByMonthDictionary { get; set; }
        private Dictionary<string, BalanceHoldingByYear> BalanceByYearDictionary { get; set; }

        public AvailableBalanceCalculator(DepositLedgerCalculator depositLedgerCalculator,
            StockLedgerCalculator stockLedgerCalculator)
        {
            _depositLedgerCalculator = depositLedgerCalculator;
            _balanceHoldings = Calculate();
            _stockLedgerCalculator = stockLedgerCalculator;
            BalanceByMonthDictionary = new Dictionary<string, BalanceHoldingByMonth>();
            BalanceByYearDictionary = new Dictionary<string, BalanceHoldingByYear>();
        }

        public decimal GetTotalDeposit()
        {
            var deposits = _depositLedgerCalculator.GetAllDeposit();

            return deposits.Sum(deposit => deposit.Amount);
        }

        internal List<BalanceHolding> Calculate()
        {
            var holdings = new List<BalanceHolding>();
            var deposits = _depositLedgerCalculator.GetAllDeposit();
            var totalHolding = (decimal)0;

            foreach (var deposit in deposits)
            {
                totalHolding += deposit.Amount;
                var holding = new BalanceHolding
                {
                    CashAvailable = totalHolding,
                    Date = deposit.Date
                };
                holdings.Add(holding);
            }

            return holdings;
        }

        internal decimal Calculate(DateTime tradingDate)
        {
            var currentHoldingCash = _balanceHoldings
                .Where(holding => holding.Date < tradingDate)
                .Sum(h => h.CashAvailable);

            return currentHoldingCash;
        }

        internal void DeductBalance(decimal amount, DateTime appliedDate)
        {
            foreach (var holding in _balanceHoldings)
            {
                if (holding.Date >= appliedDate)
                    holding.CashAvailable -= amount;
            }
        }

        internal void AddBalance(decimal amount, DateTime appliedDate)
        {
            foreach (var holding in _balanceHoldings)
            {
                if (holding.Date >= appliedDate)
                    holding.CashAvailable += amount;
            }
        }

        internal List<BalanceHoldingByMonth> GetBalanceHoldingsByMonth()
        {
            var balanceHoldingsByMonth = _balanceHoldings.Select(holding => new BalanceHoldingByMonth
            {
                Month = holding.Date.Month,
                Date = holding.Date,
                CashAvailable = holding.CashAvailable,
                Year = holding.Date.Year,
            }).ToList();

            return CalculateBalanceByMonths(balanceHoldingsByMonth);
        }

        internal List<BalanceHoldingByYear> GetBalanceHoldingsByYear()
        {
            var balanceHoldingsByYear = _balanceHoldings.Select(holding => new BalanceHoldingByYear
            {
                Date = holding.Date,
                CashAvailable = holding.CashAvailable,
                Year = holding.Date.Year
            }).ToList();

            return CalculateBalanceByYears(balanceHoldingsByYear);
        }

        private List<BalanceHoldingByYear> CalculateBalanceByYears(List<BalanceHoldingByYear> balanceHoldingByYears)
        {
            balanceHoldingByYears.ForEach(profit =>
            {
                var key = profit.Year.ToString();

                if (!BalanceByYearDictionary.ContainsKey(key))
                    BalanceByYearDictionary.Add(key, profit);
                else if(BalanceByYearDictionary[key].CashAvailable < profit.CashAvailable)
                    BalanceByYearDictionary[key].CashAvailable = profit.CashAvailable;
            });

            return BalanceByYearDictionary.Values.ToList();
        }

        private List<BalanceHoldingByMonth> CalculateBalanceByMonths(List<BalanceHoldingByMonth> balanceHoldingByMonths)
        {
            balanceHoldingByMonths.ForEach(profit =>
            {
                var key = profit.Month.ToString() + "/" + profit.Year.ToString();

                if (!BalanceByMonthDictionary.ContainsKey(key))
                    BalanceByMonthDictionary.Add(key, profit);
                else if (BalanceByMonthDictionary[key].CashAvailable < profit.CashAvailable)
                    BalanceByMonthDictionary[key].CashAvailable = profit.CashAvailable;
            });

            return BalanceByMonthDictionary.Values.ToList();
        }
    }
}

