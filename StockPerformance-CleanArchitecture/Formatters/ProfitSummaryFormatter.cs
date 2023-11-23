using System.Text;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Formatters
{
    public static class ProfitSummaryFormatter
	{
        public static StringBuilder FormatResultToDisplay(ProfitSummary profitSummary)
        {
            var result = new StringBuilder();           

            profitSummary.MonthlyProfits.ForEach(mProfit => result.AppendLine(mProfit.DisplayProfit()));
            profitSummary.YearlyProfits.ForEach(yProfit => result.AppendLine(yProfit.DisplayProfit()));

            profitSummary.MonthlyGrowthSpeeds.ForEach(mGrowthSpeed => result.AppendLine(mGrowthSpeed.DisplayGrowthSpeed()));
            profitSummary.YearlyGrowthSpeeds.ForEach(yGrowthSpeed => result.AppendLine(yGrowthSpeed.DisplayGrowthSpeed()));


            result.AppendLine($"Total monthly profit: {profitSummary.TotalMonthlyProfit}");
            result.AppendLine($"Total yearly profit: {profitSummary.TotalYearlyProfit}");

            result.AppendLine($"Average monthly growth rate: {profitSummary.AverageMonthlyGrowthSpeed}");
            result.AppendLine($"Arverage yearly growth rate: {profitSummary.AverageYearlyGrowthSpeed}");
            return result;
        }

        public static decimal RoundNumber(this decimal number)
        {
            return Math.Round(number, 2);
        }
    }
}

