namespace OptionPerformance.Models.Interfaces
{
    public interface ISelectingStock
    {
        SelectingStock SelectingStock { get; }
    }

    public class SelectingStock
    {
        public SelectingStock(int dailyMovingAVG, decimal stockPrice, decimal yearlyReturn, decimal monthlyReturn)
        {
            DailyMovingAverage = dailyMovingAVG;
            StockPrice = stockPrice;
            YearlyReturn = yearlyReturn;
            MonthlyReturn = monthlyReturn;
        }
        public int DailyMovingAverage { get; }
        public decimal StockPrice { get; }
        public decimal YearlyReturn { get; }
        public decimal MonthlyReturn { get; }
    }
}