using System;
using System.Text;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.ProfitDetails
{
    public class ProfitSummary
    {

        public virtual MetricType Metric { get; set; }
        public List<MonthlyProfit> MonthlyProfits { get; set; }
        public List<YearlyProfit> YearlyProfits { get; set; }
        public List<MonthlyGrowthSpeed> MonthlyGrowthSpeeds { get; set; }
        public List<YearlyGrowthSpeed> YearlyGrowthSpeeds { get; set; }

        public decimal? TotalYearlyProfit { get; set; }
        public decimal? TotalMonthlyProfit { get; set; }

        public decimal? AverageMonthlyGrowthSpeed { get; set; }
        public decimal? AverageYearlyGrowthSpeed { get; set; }


        public ProfitSummary(MetricType metricType)
        {
            Metric = metricType;
            MonthlyGrowthSpeeds = new List<MonthlyGrowthSpeed>();
            MonthlyProfits = new List<MonthlyProfit>();
            YearlyGrowthSpeeds = new List<YearlyGrowthSpeed>();
            YearlyProfits = new List<YearlyProfit>();
        }

        public virtual string DisplayProfitSummary()
        {
            SetTotalProfit();
            var result = ProfitSummaryFormatter.FormatResultToDisplay(this);

            return result.ToString();
        }



        public void SetTotalProfit()
        {
            try
            {

                TotalMonthlyProfit = MonthlyProfits.Sum(mProfit => mProfit.Amount);
                TotalYearlyProfit = YearlyProfits.Sum(yProfit => yProfit.Amount);

                AverageMonthlyGrowthSpeed = MonthlyGrowthSpeeds.Average(mGrowth => mGrowth.Rate);
                AverageYearlyGrowthSpeed = YearlyGrowthSpeeds.Average(yGrowth => yGrowth.Rate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
