using System;
using StockPerformance_CleanArchitecture.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class ProfitSummaryInDollar : ProfitSummary
    {
        public ProfitSummaryInDollar()
        {
            Metric = MetricType.InDollar;
        }

    public override string DisplayProfitSummary()
    {
        return $"{Metric}. {base.DisplayProfitSummary()}";
    }
}
}

