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
        public SellReasonType SellReason { get; set; }

        public StockLedgerDetail()
        {

        }

        internal decimal GetCost()
        {
            return BoughtPrice * ShareCount;
        }

        public string GetSellReasonText(SellReasonType sellReasonType)
        {
            switch (sellReasonType)
            {
                case SellReasonType.IsTooVolatile:
                    return "Price Dropped Passed Limitation";
                case SellReasonType.SellingDollarLimitationRule:
                    return "Surpassed $ Selling Limitation Rule";
                case SellReasonType.SellingPercentageRule:
                    return "Surpassed % Selling Limitation Rule";
                default:
                    return "";
            }
        }
    }
}

