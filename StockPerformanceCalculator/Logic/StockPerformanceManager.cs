using StockPerformanceCalculator.ExternalCommunications;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class StockPerformanceManager
    {
        private StockLedgerCalculator _stockLedgerCalculator;
        private TradeCalculator _tradeCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        private DepositLedgerCalculator _depositLedgerCalculator;
        private StockPerformanceSummary _stockPerformanceSummary;
        private TradeDetailCalculator _tradeDetailCalculator;
        private StockPerformanceSummaryCalculator _stockPerformanceSummaryCalculator;
        private PriceCalculator _priceCalculator;

        private string _symbol;
        private int _year;

        public StockPerformanceManager(string symbol, int year)
        {
            _symbol = symbol;
            _year = year;

            _depositLedgerCalculator = new DepositLedgerCalculator(year);
            _stockLedgerCalculator = new StockLedgerCalculator();
            _shareNumberCalculator = new ShareNumberCalculator(_stockLedgerCalculator);
            _availableBalanceCalculator = new AvailableBalanceCalculator(_depositLedgerCalculator);
            _tradeCalculator = new TradeCalculator(_stockLedgerCalculator,
                _availableBalanceCalculator, _shareNumberCalculator);
            _priceCalculator = new PriceCalculator();
            _tradeDetailCalculator = new TradeDetailCalculator(_stockLedgerCalculator, _priceCalculator, year);
            _stockPerformanceSummaryCalculator =
                new StockPerformanceSummaryCalculator
                (symbol, year, _priceCalculator, _stockLedgerCalculator, _depositLedgerCalculator);
            _stockPerformanceSummary = new StockPerformanceSummary();
        }

        public StockPerformanceSummary StartStockPerforamanceCalculation()
        {
            var stockSummaries = YahooFinanceCaller.GetStockHistory(_symbol, _year);
            ImplementTradingStocks(stockSummaries);
            CalculateStockPerformance();

            return _stockPerformanceSummary;
        }

        private void CalculateStockPerformance()
        {
           _stockPerformanceSummary = _stockPerformanceSummaryCalculator.Calculate();
        }

        private void ImplementTradingStocks(List<SymbolSummary> stockSummaries)
        {
            var tradeDetails = _tradeDetailCalculator.GetTradeDetails(stockSummaries);

            tradeDetails.ForEach(_tradeCalculator.ImplementTrade);
        }         
    }
}

