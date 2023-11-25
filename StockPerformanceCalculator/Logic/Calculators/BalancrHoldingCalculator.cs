using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic.Calculators
{
    public class BalanceHoldingCalculator
    {
        private List<BalanceHolding> _balanceHoldings;
        DepositLedgerCalculator _depositLedgerCalculator;

        public BalanceHoldingCalculator(
            DepositLedgerCalculator depositLedgercalculator)
        {
            _balanceHoldings = new List<BalanceHolding>();
            _depositLedgerCalculator = depositLedgercalculator;
        }

        public decimal GetCurrentHoldingCash(DateTime tradingDate)
        {
            return _balanceHoldings
                .Where(holding => holding.Date < tradingDate)
                .OrderByDescending(a => a.Date)
                .Select(h => h.CashAvailable)
                .First();
        }

        internal List<BalanceHolding> Calculate()
        {
            if (_balanceHoldings.Any())
                return _balanceHoldings;

            var holdings = new List<BalanceHolding>();
            var deposits = _depositLedgerCalculator.GetAllDeposit();
            var totalHolding = (decimal)0;

            foreach (var deposit in deposits.OrderBy(a => a.Date))
            {
                totalHolding += deposit.Amount;
                var holding = new BalanceHolding
                {
                    CashAvailable = totalHolding,
                    Date = deposit.Date
                };
                holdings.Add(holding);
            }

            _balanceHoldings = holdings;
            return _balanceHoldings;
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

