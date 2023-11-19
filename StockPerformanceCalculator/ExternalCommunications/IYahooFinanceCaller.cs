using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
	public interface IYahooFinanceCaller
	{
        List<SymbolSummary> GetStockHistory(string symbol, int year);
        decimal GetCurrentPrice();
    }
}

