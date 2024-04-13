﻿using System;
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
            return MAXMonthlyProfit > -5 &&
                 MINMonthlyProfit > -5 &&
                 AVGMonthlyProfit > -5 &&
                 MAXYearlyProfit > 0 &&
                 MINYearlyProfit > 0 &&
                 AVGYearlyProfit > 0;
        }

        internal bool IsNeverProfitable()
        {
            return 
                 AVGMonthlyProfit < 0 &&
                 AVGYearlyProfit < 0;
        }
    }
}

