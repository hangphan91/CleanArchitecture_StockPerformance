using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
	public class YahooFinanceAPIMapper
	{
		public YahooFinanceAPIMapper()
		{
		}

        public static List<SymbolSummary> Map(List<SymbolSummary> summaries)
        {
            //TODO: pass parameter and mapp values
            return summaries
                .OrderByDescending(symbol => symbol.Date)
                .ToList();
        }
    }
}

