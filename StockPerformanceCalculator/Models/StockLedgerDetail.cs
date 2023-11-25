using EntityDefinitions;

namespace StockPerformanceCalculator.Models
{
    public class StockLedgerDetail
    {
        public long Id { get; set; } = 0;
        public DateTime BoughtDate { get; set; }
        public decimal ShareCount { get; set; }
        public decimal BoughtPrice { get; set; }
        public decimal? SoldPrice { get; set; }
        public DateTime? SoldDate { get; set; }
        public PositionType PositionType { get; set; }

        public StockLedgerDetail()
        {

        }

        internal decimal GetCost()
        {
            return BoughtPrice * ShareCount;
        }
    }
}

