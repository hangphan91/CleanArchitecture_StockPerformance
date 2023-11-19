namespace StockPerformanceCalculator.Logic
{
    public class ShareNumberCalculator
    {
        private StockLedgerCalculator _stockLedgerCalculator;

        public ShareNumberCalculator(StockLedgerCalculator stockLedgerCalculator)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
        }

        internal int CountShare(decimal currentPrice, decimal tradingCash)
        {
            var shareCount = (int)Math.Floor(tradingCash / currentPrice);
            return shareCount;
        }

        internal int GetHoldingShare()
        {
            return _stockLedgerCalculator.GetCurrentHoldingShare();
        }
    }
}

