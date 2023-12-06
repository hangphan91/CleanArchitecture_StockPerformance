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
            var response = new List<SymbolSummary>();
            var dataAccessor = new GetStockDatAccessor(symbol, startingDate);

            try
            {
                var result = await dataAccessor.GetHistoricalQuotesInfoAsyncFromYahoo3();
                response = new YahooFinanceAPIMapper().Map(result, symbol);
            }
            catch (Exception ex)
            {
                var result2 = await dataAccessor.GetHistoricalQuotesInfoAsyncFromMarketStack();
                response = new YahooFinanceAPIMapper().Map(result2, symbol);
            }

            _currentPrice = response.OrderByDescending(a =>a.Date).FirstOrDefault()?.ClosingPrice ?? 0;
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

