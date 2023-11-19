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
            GrowthSpeedByMonths = new List<GrowthSpeedByMonth>();
            GrowthSpeedByYears = new List<GrowthSpeedByYear>();
            ProfitByMonths = new List<ProfitByMonth>();
            ProfitByYears = new List<ProfitByYear>();
            Symbol = "";
            Year = 0;
            CurrentPrice = 0;
        }
    }
}

