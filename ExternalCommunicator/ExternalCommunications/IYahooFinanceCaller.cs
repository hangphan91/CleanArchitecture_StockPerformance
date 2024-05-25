using System;
using ExternalCommunications.Models;

namespace ExternalCommunications
{
    public interface IYahooFinanceCaller
    {
        Task<List<SymbolSummary>> GetStockHistory(string symbol, DateTime startingDate);
        decimal GetCurrentPrice();
    }
}

