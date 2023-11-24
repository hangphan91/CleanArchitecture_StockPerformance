namespace StockPerformanceCalculator.Logic.Models
{
    public class TradeDetail
    {
        public DateTime TradingDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal AboutToTradePrice { get; set; }
        public decimal PriceAverage { get; set; }

        public TradeDetail()
        {
        }
    }
}

