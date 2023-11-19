using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.ExternalCommunications
{
    public class YahooFinanceCaller
	{
		public YahooFinanceCaller()
		{
		}

        internal static List<SymbolSummary> GetStockHistory(string symbol, int year)
        {
            //TODO: make api call
            return YahooFinanceAPIMapper.Map();
        }
    }
}

