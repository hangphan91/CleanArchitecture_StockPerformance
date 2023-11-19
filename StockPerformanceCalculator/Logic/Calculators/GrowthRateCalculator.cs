using System;
using StockPerformanceCalculator.Interfaces;
using StockPerformanceCalculator.Models;
using StockPerformanceCalculator.Models.GrowthSpeeds;

namespace StockPerformanceCalculator.Logic
{
    public class GrowthRateCalculator : IGrowthSpeed
    {
        public List<GrowthSpeedByYear> GrowthSpeedByYears { get; set; }
        public List<GrowthSpeedByMonth> GrowthSpeedByMonths { get; set; }
        public GrowthRateCalculator()
        {
            GrowthSpeedByYears = new List<GrowthSpeedByYear>();
            GrowthSpeedByMonths = new List<GrowthSpeedByMonth>();
        }

        internal void Calculate(List<ProfitByYear> profitByYears, List<ProfitByMonth> profitByMonths)
        {
            decimal previousProfitByYear = 0;
            foreach (var byYear in profitByYears)
            {
                GrowthSpeedByYears.Add(new GrowthSpeedByYear
                {
                    Rate = byYear.Amount - previousProfitByYear,
                    Year = byYear.Year,
                });
                previousProfitByYear = byYear.Amount;
            }

            decimal previousProfitByMonth = 0;
            foreach (var byMonth in profitByMonths)
            {
                GrowthSpeedByYears.Add(new GrowthSpeedByYear
                {
                    Rate = byMonth.Amount - previousProfitByMonth,
                    Year = byMonth.Year,
                });
                previousProfitByMonth = byMonth.Amount;
            }
        }

        internal List<GrowthSpeedByMonth> GetGrowthRateByMonth()
        {
            return GrowthSpeedByMonths;
        }

        internal List<GrowthSpeedByYear> GetGrowthRateByYear()
        {
            return GrowthSpeedByYears;
        }

        public void GetGrowthRate()
        {
            throw new NotImplementedException();
        }
    }
}

