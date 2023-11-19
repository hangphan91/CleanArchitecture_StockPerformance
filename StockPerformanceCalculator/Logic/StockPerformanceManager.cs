using StockPerformanceCalculator.ExternalCommunications;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class StockPerformanceManager
    {
        protected StockLedgerCalculator _stockLedgerCalculator;
        private TradeCalculator _tradeCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        protected DepositLedgerCalculator _depositLedgerCalculator;
        private TradeDetailCalculator _tradeDetailCalculator;
        protected StockPerformanceSummaryCalculator _stockPerformanceSummaryCalculator;
        protected PriceCalculator _priceCalculator;
        protected IYahooFinanceCaller _yahooFinanceCaller;

        private string _symbol;
        private int _year;

        public StockPerformanceManager(string symbol, int year)
        {
            _symbol = symbol;
            _year = year;

            _yahooFinanceCaller = new YahooFinanceCaller();
            _depositLedgerCalculator = new DepositLedgerCalculator(year);
            _stockLedgerCalculator = new StockLedgerCalculator();
            _shareNumberCalculator = new ShareNumberCalculator(_stockLedgerCalculator);
            _availableBalanceCalculator = new AvailableBalanceCalculator(_depositLedgerCalculator);
            _tradeCalculator = new TradeCalculator(_stockLedgerCalculator,
                _availableBalanceCalculator, _shareNumberCalculator);
            _priceCalculator = new PriceCalculator(_yahooFinanceCaller);
            _tradeDetailCalculator = new TradeDetailCalculator(_stockLedgerCalculator, _priceCalculator, year);
            _stockPerformanceSummaryCalculator =
                new StockPerformanceSummaryCalculator
                (symbol, year, _priceCalculator, _stockLedgerCalculator, _depositLedgerCalculator);
        }

        public StockPerformanceSummary StartStockPerforamanceCalculation()
        {
            var stockSummaries = _yahooFinanceCaller.GetStockHistory(_symbol, _year);
            ImplementTradingStocks(stockSummaries);

            return CalculateStockPerformance();
        }

        private StockPerformanceSummary CalculateStockPerformance()
        {
            return _stockPerformanceSummaryCalculator.Calculate();
        }

        private void ImplementTradingStocks(List<SymbolSummary> stockSummaries)
        {
            var tradeDetails = _tradeDetailCalculator.GetTradeDetails(stockSummaries);

            tradeDetails.ForEach(_tradeCalculator.ImplementTrade);
        }
    }
}

