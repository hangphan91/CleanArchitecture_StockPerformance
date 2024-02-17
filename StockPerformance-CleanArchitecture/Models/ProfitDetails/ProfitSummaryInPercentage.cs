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
    }
}

