using StockPerformanceCalculator.Logic.Calculators;

namespace StockPerformanceCalculator.Logic
{
    public class AvailableBalanceCalculator
    {
        private DepositLedgerCalculator _depositLedgerCalculator;
        private BalanceHoldingCalculator _balanceHoldingCalculator; 


        public AvailableBalanceCalculator(DepositLedgerCalculator depositLedgerCalculator,
            BalanceHoldingCalculator balanceHodingCalculator)
        {
            _depositLedgerCalculator = depositLedgerCalculator;
            _balanceHoldingCalculator = balanceHodingCalculator;
        }

        public decimal GetTotalDeposit()
        {
            var deposits = _depositLedgerCalculator.GetAllDeposit();

            return deposits.Sum(deposit => deposit.Amount);
        }

        internal void AddBalance(decimal toAddBalance, DateTime boughtDate)
        {
            _balanceHoldingCalculator.AddBalance(toAddBalance, boughtDate);
        }

        internal void Calculate()
        {
           _balanceHoldingCalculator.Calculate();
        }

        internal decimal Calculate(DateTime tradingDate)
        {
            _balanceHoldingCalculator.Calculate();

            return _balanceHoldingCalculator.GetCurrentHoldingCash(tradingDate);
        }

        internal void DeductBalance(decimal toSubtractBalance, DateTime boughtDate)
        {
            _balanceHoldingCalculator.DeductBalance(toSubtractBalance, boughtDate);
        }
    }
}

