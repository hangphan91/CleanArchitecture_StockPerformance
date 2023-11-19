using System;
using StockPerformanceCalculator.ExternalCommunications;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class PriceCalculator
    {
        private IYahooFinanceCaller _yahooFinanceCaller;
        public PriceCalculator(IYahooFinanceCaller yahooFinanceCaller)
        {
            _yahooFinanceCaller = yahooFinanceCaller;
        }
        public decimal GetCurrentPrice()
        {
            return _yahooFinanceCaller.GetCurrentPrice();
        }
    }
}

