using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
	public class PriceCalculator
	{
		private decimal _currentPrice;
		public PriceCalculator()
		{
			_currentPrice = 0;
		}
		public decimal GetCurrentPrice()
		{
			return _currentPrice;
		}

		public decimal GetCurrentPrice(List<SymbolSummary> symbolSummaries)
		{
            var currentPrice = symbolSummaries.Select(stock => stock.ClosingPrice).First();
			_currentPrice = currentPrice;

			return currentPrice;
        }
    }
}

