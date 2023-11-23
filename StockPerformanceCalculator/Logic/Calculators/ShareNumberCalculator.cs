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
            var shareCount = tradingCash / currentPrice;
            return shareCount;
        }

        internal decimal GetHoldingShare()
        {
            return _stockLedgerCalculator.GetCurrentHoldingShare();
        }
    }
}

