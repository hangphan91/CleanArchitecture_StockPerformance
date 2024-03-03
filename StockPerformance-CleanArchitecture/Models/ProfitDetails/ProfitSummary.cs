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
        public List<MonthlyBalanceHolding> MonthlyBalanceHoldings { get; set; }
        public List<YearlyBalanceHolding> YearlyBalanceHoldings { get; set; }
        public List<MonthlyGrowthSpeed> MonthlyGrowthSpeeds { get; set; }
        public List<YearlyGrowthSpeed> YearlyGrowthSpeeds { get; set; }

        public decimal? MAXYearlyProfit { get; set; }
        public decimal? MAXMonthlyProfit { get; set; }
        public decimal? MINYearlyProfit { get; set; }
        public decimal? MINMonthlyProfit { get; set; }

        public decimal? AverageMonthlyGrowthSpeed { get; set; }
        public decimal? AverageYearlyGrowthSpeed { get; set; }


        public ProfitSummary()
        {
            MonthlyGrowthSpeeds = new List<MonthlyGrowthSpeed>();
            MonthlyProfits = new List<MonthlyProfit>();
            MonthlyBalanceHoldings = new List<MonthlyBalanceHolding>();
            YearlyGrowthSpeeds = new List<YearlyGrowthSpeed>();
            YearlyProfits = new List<YearlyProfit>();
            YearlyBalanceHoldings = new List<YearlyBalanceHolding>();
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
                MAXMonthlyProfit = MonthlyProfits?.Max(mProfit => mProfit.Amount);
                MAXYearlyProfit = YearlyProfits?.Max(yProfit => yProfit.Amount);

                MINMonthlyProfit = MonthlyProfits?.Min(mProfit => mProfit.Amount);
                MINYearlyProfit = YearlyProfits?.Min(yProfit => yProfit.Amount);

                AverageMonthlyGrowthSpeed = MonthlyGrowthSpeeds?.Average(mGrowth => mGrowth.Rate);
                AverageYearlyGrowthSpeed = YearlyGrowthSpeeds?.Average(yGrowth => yGrowth.Rate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
