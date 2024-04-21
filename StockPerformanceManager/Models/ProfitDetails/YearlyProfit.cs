using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class YearlyProfit : Profit
    {
        public int Year { get; set; }
        public YearlyProfit()
        {
        }

        public override string DisplayProfit()
        {
            var result = $"In {Year} ";
            return result + base.DisplayProfit();
        }
    }
}

