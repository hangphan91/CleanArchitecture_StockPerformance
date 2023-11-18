using System;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class ProfitSummaryPercentage : ProfitSummary
    {
        public ProfitSummaryPercentage(MetricType metric): base(metric)
        {
        }
        public override string DisplayProfitSummary()
        {
            return $"{Metric}. {base.DisplayProfitSummary()}";
        }
    }
}

