namespace StockPerformanceCalculator.Logic.Models
{
    public class TradeDetail
    {
        public DateTime TradingDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentHoldingValue { get; set; }
        public decimal BasicCost { get; set; }

        public TradeDetail()
        {
        }
    }
}

