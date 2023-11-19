using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
    public class YahooFinanceCaller : IYahooFinanceCaller
    {
        private decimal _currentPrice;
        private List<SymbolSummary> _symbolSummaries;
        public YahooFinanceCaller()
        {
            _currentPrice = 0;
            _symbolSummaries = new List<SymbolSummary>();
        }

        public decimal GetCurrentPrice()
        {
            return _currentPrice;
        }

        public List<SymbolSummary> GetCurrentHistory()
        {
            return _symbolSummaries;
        }

        public List<SymbolSummary> GetStockHistory(string symbol, int year)
        {
            //TODO: make api call

            var result = new List<SymbolSummary>();
            _currentPrice = result.LastOrDefault()?.ClosingPrice ?? 0;
            _symbolSummaries = result;

            return YahooFinanceAPIMapper.Map(result);
        }
    }
}

