using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
	public class YahooFinanceAPIMapper
	{
		public YahooFinanceAPIMapper()
		{
		}

        internal static List<SymbolSummary> Map()
        {
            //TODO: pass parameter and mapp values
            return new List<SymbolSummary>()
                .OrderByDescending(symbol => symbol.Date)
                .ToList();
        }
    }
}

