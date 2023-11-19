using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Interfaces
{
    public interface ISellStock
	{
		void Sell(StockLedgerDetail stockLedgerDetail);
	}
}

