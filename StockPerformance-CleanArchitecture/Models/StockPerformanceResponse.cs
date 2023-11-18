using System;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models
{
    public class StockPerformanceResponse
    {
        public string Symbol { get; set; }
        public int NumberOfYear { get; set; }
        public ProfitSummaryInDollar ProfitSummaryInDollar { get; set; }
        public ProfitSummaryPercentage ProfitSummaryPercentage { get; set; }


        public StockPerformanceResponse(string symbol, int numberOfyear)
        {
            ProfitSummaryPercentage = new ProfitSummaryPercentage(MetricType.InPercentage);
            ProfitSummaryInDollar = new ProfitSummaryInDollar(MetricType.InDollar);
            Symbol = symbol;
            NumberOfYear = numberOfyear;
        }

        public string DisplayStockPerformance()
        {
            var toDisplay = $"Symbol {Symbol}, look back number of years: {NumberOfYear}.";

            var result = ProfitSummaryInDollar.DisplayProfitSummary();
            toDisplay =  toDisplay + " " + result + " " + ProfitSummaryPercentage.DisplayProfitSummary();

            return toDisplay;
        }
    }
}

