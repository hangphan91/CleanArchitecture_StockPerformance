using System.Text;
using StockPerformance_CleanArchitecture.ProfitDetails;
using Utilities;

namespace StockPerformance_CleanArchitecture.Formatters
{
    public static class ProfitSummaryFormatter
	{
        public static StringBuilder FormatResultToDisplay(ProfitSummary profitSummary)
        {
            var result = new StringBuilder(); 
           
            var metric = profitSummary.Metric == Models.ProfitDetails.MetricType.InPercentage ? "%": "$";

            result.AppendLine($"Total profit: ${profitSummary.MAXYearlyProfit}");
            result.AppendLine($"Average monthly growth rate: {profitSummary.AverageMonthlyGrowthSpeed.RoundNumber()} {metric}");
            result.AppendLine($"Arverage yearly growth rate: {profitSummary.AverageYearlyGrowthSpeed.RoundNumber()} {metric}");
            return result;
        }

        
    }
}

