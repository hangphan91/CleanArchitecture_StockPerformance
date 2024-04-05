using System;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class ProfitSummaryPercentage : ProfitSummary
    {
        public ProfitSummaryPercentage()
        {
            Metric = MetricType.InPercentage;
        }
        public override string DisplayProfitSummary()
        {
            return $"{base.DisplayProfitSummary()}";
        }

        internal bool IsProfitable()
        {
            return MAXMonthlyProfit > 0 &&
                 MAXYearlyProfit > 0 &&
                 MINMonthlyProfit > 0 &&
                 MINYearlyProfit > 0 &&
                 AVGMonthlyProfit > 0 &&
                 AVGYearlyProfit > 0;
        }
    }
}

