using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic.Calculators
{
    public class HoldingPositionCalculator
	{
        StockLedgerCalculator _stockLedgerCalculator;

        public HoldingPositionCalculator(
            StockLedgerCalculator stockLedgerCalculator
            )
		{
            _stockLedgerCalculator = stockLedgerCalculator;
		}

        internal void Calculate(StockPerformanceSummary summary)
        {;
                     
        }
    }
}

