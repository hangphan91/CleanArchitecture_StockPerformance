using System.Text;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Formatters
{
    public static class ProfitSummaryFormatter
	{
        public static StringBuilder FormatResultToDisplay(ProfitSummary profitSummary)
        {
            var result = new StringBuilder(); 
           
            var metric = profitSummary.Metric == Models.ProfitDetails.MetricType.InPercentage ? "%": "$";

            result.AppendLine($"Total profit: ${profitSummary.TotalYearlyProfit}");
            result.AppendLine($"Average monthly growth rate: {profitSummary.AverageMonthlyGrowthSpeed.RoundNumber()} {metric}");
            result.AppendLine($"Arverage yearly growth rate: {profitSummary.AverageYearlyGrowthSpeed.RoundNumber()} {metric}");
            return result;
        }

        public static decimal RoundNumber(this decimal? number)
        {
            if (number == null)
                return 0;

            return Math.Round(number.Value, 2);
        }


        public static decimal RoundNumber(this decimal number)
        {
            return Math.Round(number, 2);
        }
    }
}

