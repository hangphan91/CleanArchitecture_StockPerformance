using System;
namespace StockPerformanceCalculator.Models
{
    public class StockLedgerDetail
    {
        public long Id { get; set; } = 0;
        public DateTime Date { get; set; }
        public int ShareCount { get; set; }
        public decimal Price { get; set; }

        public StockLedgerDetail()
        {

        }

        internal decimal GetCost()
        {
            return Price * ShareCount;
        }
    }
}

