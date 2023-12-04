using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class DepositLedgerCalculator
    {
        private List<DepositLedger> _deposits;
        private DepositRule _depositRule;

        public DepositLedgerCalculator(DepositRule depositRule)
        {
            _depositRule = depositRule;
        }

        internal List<DepositLedger> SetUpDepositLeggerFromDate(DateTime startingDate)
        {
            var depositLedgers = new List<DepositLedger>();

            var startMonth = startingDate.Month;
            var startYear = startingDate.Year;

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month;

            depositLedgers.Add(new DepositLedger
            {
                Amount = _depositRule.GetInitialDepositAmount(),
                Date = new DateTime(startYear, startMonth, 1),
            });
            var dates = new List<DateOnly>();

            for (DateTime date = startingDate; date <= DateTime.Now; date = date.AddMonths(1))
            {
                dates.Add(new DateOnly(date.Year, date.Month, date.Day));
            }

            foreach (var date in dates)
            {
                var depositLedgersToAdd = GetDepositLedgers(date.Year, date.Month);
                depositLedgers.AddRange(depositLedgersToAdd);
            }

            _deposits = depositLedgers.OrderBy(a => a.Date).ToList();
            return depositLedgers;
        }

        private List<DepositLedger> GetDepositLedgers(int year, int month)
        {
            var depositDate = new DateTime(year, month, _depositRule.GetFirstDepositDate());
            var amount = _depositRule.GetDepositAmount();
            var firstDeposit = new DepositLedger
            {
                Amount = amount,
                Date = depositDate,
            };

            depositDate = new DateTime(year, month, _depositRule.GetSecondDepositDate());
            var secondDeposit = new DepositLedger
            {
                Amount = amount,
                Date = depositDate,
            };
            return new List<DepositLedger> { firstDeposit, secondDeposit };
        }

        internal List<DepositLedger> GetAllDeposit()
        {
            return _deposits;
        }

        internal List<DepositLedger> GetDepositByMonth(int month, int year)
        {
            return _deposits.Where(d => d.Date.Month == month
            && d.Date.Year == year).ToList();
        }
    }
}

