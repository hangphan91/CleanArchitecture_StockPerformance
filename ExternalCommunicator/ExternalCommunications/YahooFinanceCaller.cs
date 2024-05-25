using ExternalCommunications;
using ExternalCommunications.Models;
using HP.PersonalStocks.Mgr.Helpers;

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

        public async Task<List<SymbolSummary>> GetStockHistory(string symbol, DateTime startingDate)
        {
            var response = new List<SymbolSummary>();
            var dataAccessor = new GetStockDatAccessor(symbol, startingDate);

            try
            {

                var result = await dataAccessor.GetHistoricalDataRapicAPI();
                response = new YahooFinanceAPIMapper().Map(result, symbol);
            }
            catch (Exception ex0)
            {
                try
                {
                    var result = await dataAccessor.GetHistoricalQuotesInfoAsyncFromYahoo3();
                    response = new YahooFinanceAPIMapper().Map(result, symbol);
                }
                catch (Exception ex)
                {
                    try
                    {
                        var result2 = await dataAccessor.GetHistoricalQuotesInfoAsyncFromMarketStack();
                        response = new YahooFinanceAPIMapper().Map(result2, symbol);

                    }
                    catch (Exception ex2)
                    {/*
                        try
                        {
                            var result2 = await dataAccessor.GetHistoricalQuotesInfoAsyncFromYahoo();
                            response = new YahooFinanceAPIMapper().Map(result2);
                        }
                        catch (Exception ex3)
                        {
                            var result2 = await dataAccessor.GetHistoricalQuotesInfoAsyncFromYahoo2();
                            response = new YahooFinanceAPIMapper().Map(result2, symbol);
                        }*/
                    }
                }
            }

            _currentPrice = response.OrderByDescending(a => a.Date).FirstOrDefault()?.ClosingPrice ?? 0;
            _symbolSummaries = response.Where(a => a.Date >= startingDate).ToList();

            return _symbolSummaries;
        }
    }
}

