using System;
using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.Logic;
using StockPerformanceCalculatorUnitTests.ExternalCommunications;

namespace StockPerformanceCalculatorUnitTests
{
	public class MockStockPerformanceManager : StockPerformanceManager
    {
		public MockStockPerformanceManager(string symbol, int year, IEntityDefinitionsAccessor accessor)
			: base(symbol, year, accessor)
		{
			_yahooFinanceCaller = new MockYahooFinanceCaller();
			_priceCalculator = new PriceCalculator(_yahooFinanceCaller);
            _stockPerformanceSummaryCalculator =
                new StockPerformanceSummaryCalculator
                (symbol, year, _priceCalculator, _stockLedgerCalculator,
				_depositLedgerCalculator, _availableBalanceCalculator);
        }
	}
}

