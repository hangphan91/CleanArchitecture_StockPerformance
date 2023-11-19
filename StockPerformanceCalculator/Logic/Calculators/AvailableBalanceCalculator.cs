using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class AvailableBalanceCalculator
    {
        private List<BalanceHolding> _balanceHoldings;
        private DepositLedgerCalculator _depositLedgerCalculator;
        public AvailableBalanceCalculator(DepositLedgerCalculator depositLedgerCalculator)
        {
            _depositLedgerCalculator = depositLedgerCalculator;
            _balanceHoldings = Calculate();
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
            var currentHolding = _balanceHoldings
                .FirstOrDefault(holding => holding.Date < tradingDate);

            if (currentHolding == null)
                return 0;

            return currentHolding.CashAvailable;
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
    }
}

