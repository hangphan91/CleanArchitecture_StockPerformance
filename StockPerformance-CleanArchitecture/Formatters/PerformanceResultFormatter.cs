using System;
using StockPerformance_CleanArchitecture.Models;
using Utilities;

namespace StockPerformance_CleanArchitecture.Formatters
{
	public class PerformanceResultFormatter
	{
		public PerformanceResultFormatter()
		{
		}
        public static string GetTableMessage(List<StockPerformanceResponse> list)
        {
            var tableStartHtml = $"<table  border=\"1\" class=\"table table-bordered table-responsive table-hover\"> ";

            var tableColumnsMessage =
                            $" <tr> " +
                            $"  <th>Symbol</th> " +
                            $"  <th>Start Date $</th> " +
                            $"  <th>End Date $</th> " +
                            $"  <th>Max Yearly Profit %</th> " +
                            $"  <th>Max Monthly Profit %</th> " +
                            $"  <th>Min Yearly Profit %</th> " +
                            $"  <th>Min Monthly Profit %</th> " +
                            $"  <th>Average Yearly Profit %</th> " +
                            $"  <th>Average Monthly Profit %</th> " +
                            $"  <th>Total Profit %</th> " +
                            $" </tr> ";
            var tableRowsMessage = "";
            foreach (var item in list)
            {
                if (item == null)
                    continue;

                item.ProfitSummaryPercentage?.SetTotalProfit();
                var basedURL = "https://stockperformance.azurewebsites.net/";

                var link = $"{basedURL}?symbol={item.Symbol}&startYear={item.SearchDetail?.SettingDate.Year}";

                var yearlyAVGGrowth = item.ProfitSummaryPercentage?.AVGYearlyProfit;
                var monthlyAVGGrowth = item.ProfitSummaryPercentage?.AVGMonthlyProfit;
                var yearlyMinGrowth = item.ProfitSummaryPercentage?.MINYearlyProfit;
                var monthlyMinGrowth = item.ProfitSummaryPercentage?.MINMonthlyProfit;
                var yearlyMaxGrowth = item.ProfitSummaryPercentage?.MAXYearlyProfit;
                var monthlyMaxGrowth = item.ProfitSummaryPercentage?.MAXMonthlyProfit;

                tableRowsMessage += $"  <tr> ";
                tableRowsMessage += $"  <th> <a href={link}>{item.Symbol}</a></th>";
                tableRowsMessage += $"  <th>{item.SearchDetail?.SettingDate}</th> ";
                tableRowsMessage += $"  <th>{item.SearchDetail?.SearchSetup?.EndingYear}</th> ";
                tableRowsMessage += $"  <th>{yearlyMaxGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyMaxGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{yearlyMinGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyMinGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{yearlyAVGGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyAVGGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{item.ProfitInPercentage.RoundNumber()}</th> ";
                tableRowsMessage += $" </tr> ";
            }

            var tableMessage = tableStartHtml + tableColumnsMessage + tableRowsMessage + $"</table>";
            return tableMessage;
        }

    }
}

