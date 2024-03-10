namespace StockPerformanceCalculator.Logic
{
    public class ShareNumberCalculator
    {
        private StockLedgerCalculator _stockLedgerCalculator;

        public ShareNumberCalculator(StockLedgerCalculator stockLedgerCalculator)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
        }

        internal decimal CountShare(decimal currentPrice, decimal tradingCash)
        {
            if (currentPrice == 0)
                return (decimal)0;

            var shareCount = tradingCash / currentPrice;
            return shareCount;
        }

        internal decimal GetHoldingShare()
        {
            return _stockLedgerCalculator.GetCurrentHoldingShare();
        }
    }
}

