using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class DepositLedgerCalculator
    {
        private List<DepositLedger> _deposits;
        private DepositRule _depositRule;

        public DepositLedgerCalculator(EntityEngine entityEngine)
        {
            _depositRule = new DepositRule(entityEngine);
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
                Date = startingDate,
            });

            for (int currentYear = startYear; currentYear <= endYear; currentYear++)
            {
                for (int currentMonth = 1; currentMonth <= 12; currentMonth++)
                {
                    if (startMonth > currentMonth && currentYear == startYear)
                        continue;

                    var depositLedgersToAdd = GetDepositLedgers(currentYear, currentMonth);

                    depositLedgers.AddRange(depositLedgersToAdd);

                    if (currentYear == endYear && currentMonth == endMonth)
                        break;

                }
            }
            _deposits = depositLedgers;
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

