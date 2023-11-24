﻿using StockPerformanceCalculator.Models;
using YahooQuotesApi;

namespace StockPerformanceCalculator.ExternalCommunications
{
    public class YahooFinanceAPIMapper
    {
        public List<SymbolSummary> SymbolSummaries { get; set; }
        public YahooFinanceAPIMapper()
        {
            SymbolSummaries = new List<SymbolSummary>();
        }

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
                    Symbol = result.Symbol.Name
                });
            }

            return SymbolSummaries.OrderByDescending(symbol => symbol.Date)
                .ToList(); ;
        }
    }
}
