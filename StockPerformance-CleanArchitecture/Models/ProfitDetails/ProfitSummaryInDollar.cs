using System;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class ProfitSummaryInDollar : ProfitSummary
    {
        public ProfitSummaryInDollar(MetricType metric): base(metric)
        {
        }

    public override string DisplayProfitSummary()
    {
        return $"{Metric}. {base.DisplayProfitSummary()}";
    }
}
}

