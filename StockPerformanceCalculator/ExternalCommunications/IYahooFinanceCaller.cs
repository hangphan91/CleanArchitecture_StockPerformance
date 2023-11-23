using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
	public interface IYahooFinanceCaller
	{
       Task< List<SymbolSummary>> GetStockHistory(string symbol, DateTime startingDate);
        decimal GetCurrentPrice();
    }
}

