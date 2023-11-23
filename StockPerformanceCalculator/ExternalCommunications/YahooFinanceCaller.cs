using HP.PersonalStocks.Mgr.Helpers;
using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
    public class YahooFinanceCaller : IYahooFinanceCaller
    {
        private Logic.EntityEngine _entityEngine;
        private decimal _currentPrice;
        private List<SymbolSummary> _symbolSummaries;
        public YahooFinanceCaller(Logic.EntityEngine entityEngine)
        {
            _currentPrice = 0;
            _symbolSummaries = new List<SymbolSummary>();
            _entityEngine = entityEngine;
        }

        public decimal GetCurrentPrice()
        {
            return _currentPrice;
        }

        public List<SymbolSummary> GetCurrentHistory()
        {
            return _symbolSummaries;
        }

        public async Task<List<SymbolSummary>> GetStockHistory(string symbol, DateTime startingDate)
        {
            var dataAccessor = new GetStockDatAccessor(symbol, startingDate);
            var result = await dataAccessor.GetHistoricalQuotesInfoAsync();
            var response = new YahooFinanceAPIMapper().Map(result);

            _currentPrice = response.FirstOrDefault()?.ClosingPrice ?? 0;
            _symbolSummaries = response;

            var toInsert = response.Select(result
                => new EntityDefinitions.SymbolSummary
                {
                    ClosingPrice = result.ClosingPrice,
                    Date = result.Date,
                    Symbol = result.Symbol,
                }).ToList();

            _entityEngine.AddSymbolSummaries(toInsert);
            return response;

        }
    }
}

