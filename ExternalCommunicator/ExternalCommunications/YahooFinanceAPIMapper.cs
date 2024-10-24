using ExternalCommunications.Models;
using Fynance.Result;
using HP.PersonalStocks.Mgr.Helpers;
using OoplesFinance.YahooFinanceAPI.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
    public class YahooFinanceAPIMapper
    {
        public List<SymbolSummary> SymbolSummaries { get; set; }
        public YahooFinanceAPIMapper()
        {
            SymbolSummaries = new List<SymbolSummary>();
        }
        /*
                public List<SymbolSummary> Map(Security? result)
                {
                    if (result == null
                        || result?.PriceHistory == null
                        || !result.PriceHistory.HasValue)
                        return SymbolSummaries;

                    var priceHistory = result.PriceHistory.Value;
                    foreach (var item in priceHistory)
                    {
                        var date = item.Date.ToDateTimeUnspecified();
                        SymbolSummaries.Add(new SymbolSummary
                        {
                            ClosingPrice = (decimal)item.Close,
                            Date = date,
                            Symbol = result.Symbol.Name,
                            Volume = (decimal)item.Volume,
                        });
                    }

                    return SymbolSummaries.OrderByDescending(symbol => symbol.Date)
                        .ToList(); ;
                }
        */
        internal List<SymbolSummary> Map(Response result2, string symbol)
        {
            var summaries = new List<SymbolSummary>();

            if (result2 == null || result2.data == null)
                return summaries;

            foreach (var item in result2.data)
            {
                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = (decimal)item.close,
                    Date = DateTime.Parse(item.date),
                    Symbol = symbol,
                    Volume = (decimal)item.volume,
                });
            }
            return summaries.OrderBy(a => a.Date).ToList();
        }

        internal List<SymbolSummary> Map(List<HistoricalData> result, string symbol)
        {
            var summaries = new List<SymbolSummary>();
            foreach (var item in result)
            {
                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = (decimal)item.Close,
                    Date = item.Date,
                    Symbol = symbol,
                    Volume = (decimal)item.Volume,
                });
            }
            return summaries;
        }

        internal List<SymbolSummary> Map(FyResult result, string symbol)
        {
            var summaries = new List<SymbolSummary>();
            foreach (var item in result.Quotes)
            {
                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = (decimal)item.Close,
                    Date = item.Period,
                    Symbol = symbol,
                    Volume = (decimal)item.Volume,
                });
            }
            return summaries;
        }

        internal List<SymbolSummary> Map(RapiAPiResponse result, string symbol)
        {
            var summaries = new List<SymbolSummary>();
            foreach (var item in result.Results)
            {
                DateTime.TryParse(item.Date, out var date);

                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = (decimal)item.Close,
                    Date = date,
                    Symbol = symbol,
                    Volume = (decimal)item.Volume,
                });
            }
            return summaries.OrderBy(a => a.Date).ToList();
        }

        internal List<SymbolSummary> Map(Root? result, string symbol)
        {
            var summaries = new List<SymbolSummary>();
            foreach (var item in result.Data)
            {
                DateTime myDate = DateTime.UnixEpoch.AddMilliseconds(item.Date);

                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = (decimal)item.Close,
                    Date = myDate,
                    Symbol = symbol,
                    Volume = (decimal)item.Volume,
                });
            }
            return summaries.OrderBy(a => a.Date).ToList();
        }
    }
}

