using System;
using StockPerformanceCalculator.Models.GrowthSpeeds;

namespace StockPerformanceCalculator.Models
{
    public class StockPerformanceSummary
    { 
        public List<ProfitByMonth> ProfitByMonths { get; set; }
        public List<ProfitByYear> ProfitByYears { get; set; }
        public List<GrowthSpeedByYear> GrowthSpeedByYears { get; set; }
        public List<GrowthSpeedByMonth> GrowthSpeedByMonths { get; set; }
        public string Symbol { get; set; }
        public int Year { get; set; }
        public decimal CurrentPrice { get; set; }

        public StockPerformanceSummary()
        {
        }
    }
}

