using System;
namespace StockPerformanceCalculator.Models.BalanceHoldings
{
    public class BalanceHoldingByMonth : BalanceHolding
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public BalanceHoldingByMonth()
        {
        }
    }
}

