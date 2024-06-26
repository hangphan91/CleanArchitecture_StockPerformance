﻿using ExternalCommunications;
using ExternalCommunications.Models;
using StockPerformanceCalculator.Models;
using SymbolSummary = ExternalCommunications.Models.SymbolSummary;

namespace StockPerformanceCalculatorUnitTests.ExternalCommunications
{
    public class MockYahooFinanceCaller : IYahooFinanceCaller
    {
        private decimal _currentPrice = 0;
        public decimal GetCurrentPrice()
        {
            return _currentPrice;
        }

        public Task<List<SymbolSummary>> GetStockHistory(string symbol, DateTime startingdate)
        {
            return Task.FromResult(GetTestStockSummary());
        }

        private List<SymbolSummary> GetTestStockSummary()
        {
            var summaries = new List<SymbolSummary>();
            for (int i = 0; i < 30; i++)
            {
                summaries.Add(new SymbolSummary
                {
                    ClosingPrice = new Random().NextInt64(30, 60),
                    Date = DateTime.Now.AddMonths(-i)
                });
            }
            summaries = summaries.OrderByDescending(s => s.Date).ToList();
            _currentPrice = summaries.First().ClosingPrice;

            return summaries;
        }
    }
}

