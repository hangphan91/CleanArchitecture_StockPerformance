using System;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.Models;

namespace StockPerformance_CleanArchitecture.Models
{
    public class StockPerformanceResponse
    {
        public string Symbol { get; set; }
        public int Year { get; set; }
        public ProfitSummaryInDollar ProfitSummaryInDollar { get; set; }
        public ProfitSummaryPercentage ProfitSummaryPercentage { get; set; }
        public string ToDisplay { get; set; }
        public decimal TotalBalanceAfterLoss { get; set; } = 0;
        public decimal TotalDeposit { get; set; } = 0;
        public decimal TotalBalanceHoldingInPosition { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentHoldingShare { get; set; }

        public StockPerformanceResponse()
        {
            ProfitSummaryInDollar = new ProfitSummaryInDollar();
            ProfitSummaryPercentage = new ProfitSummaryPercentage();
        }
        public StockPerformanceResponse(string symbol, int numberOfyear)
        {
            ProfitSummaryPercentage = new ProfitSummaryPercentage();
            ProfitSummaryInDollar = new ProfitSummaryInDollar();
            Symbol = symbol;
            Year = numberOfyear;
            ToDisplay = DisplayStockPerformance();
        }

        public string DisplayStockPerformance()
        {
            var toDisplay = $"Symbol {Symbol}, look back number of years: {Year}.";

            var result = ProfitSummaryInDollar.DisplayProfitSummary();
            toDisplay = toDisplay + " " + result + " " + ProfitSummaryPercentage.DisplayProfitSummary();

            return toDisplay;
        }

        internal StockPerformanceResponse Map(StockPerformanceSummary summary)
        {
            var response = new StockPerformanceResponse
            {
                Year = summary.Year,
                Symbol = summary.Symbol,
                TotalBalanceAfterLoss = summary.TotalBalanceAfterLoss.RoundNumber(),
                TotalDeposit = summary.TotalDeposit.RoundNumber(),
                CurrentPrice = summary.CurrentPrice,
                CurrentHoldingShare = summary.CurrentHoldingShare.RoundNumber(),
                TotalBalanceHoldingInPosition = summary.TotalBalanceHoldingInPosition.RoundNumber(),
                ProfitSummaryInDollar = new ProfitSummaryInDollar
                {
                    MonthlyGrowthSpeeds = new List<MonthlyGrowthSpeed>(),
                    YearlyGrowthSpeeds = new List<YearlyGrowthSpeed>(),
                    MonthlyProfits = new List<MonthlyProfit>(),
                    YearlyProfits = new List<YearlyProfit>(),
                    MonthlyBalanceHoldings = new List<MonthlyBalanceHolding>(),
                    YearlyBalanceHoldings = new List<YearlyBalanceHolding>(),
                },
            };
            GetProfitInDollar(summary, response);
            GetProfitInPercentage(summary, response);

            return response;
        }

        private static void GetProfitInDollar(StockPerformanceSummary summary, StockPerformanceResponse response)
        {
            summary.ProfitByMonths
                .ForEach(profit => response.ProfitSummaryInDollar.MonthlyProfits
                .Add(new MonthlyProfit
                {
                    Amount = profit.Amount.RoundNumber(),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.ProfitByYears
                .ForEach(profit => response.ProfitSummaryInDollar.YearlyProfits
                .Add(new YearlyProfit
                {
                    Amount = profit.Amount.RoundNumber(),
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByMonths
                .ForEach(profit => response.ProfitSummaryInDollar.MonthlyGrowthSpeeds
                .Add(new MonthlyGrowthSpeed
                {
                    Rate = profit.Rate.RoundNumber(),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByYears
                .ForEach(profit => response.ProfitSummaryInDollar.YearlyGrowthSpeeds
                .Add(new YearlyGrowthSpeed
                {
                    Rate = profit.Rate.RoundNumber(),
                    Year = profit.Year,
                }));
        }

        private static void GetProfitInPercentage(StockPerformanceSummary summary,
            StockPerformanceResponse response)
        {
            summary.ProfitByMonths
                .ForEach(profit => response.ProfitSummaryPercentage.MonthlyProfits
                .Add(new MonthlyProfit
                {
                    Amount = GetPercentage(response.TotalDeposit, profit.Amount),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.ProfitByYears
                .ForEach(profit => response.ProfitSummaryPercentage.YearlyProfits
                .Add(new YearlyProfit
                {
                    Amount = GetPercentage(response.TotalDeposit, profit.Amount),
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByMonths
                .ForEach(profit => response.ProfitSummaryPercentage.MonthlyGrowthSpeeds
                .Add(new MonthlyGrowthSpeed
                {
                    Rate = GetPercentage(response.TotalDeposit, profit.Rate),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByYears
                .ForEach(profit => response.ProfitSummaryPercentage.YearlyGrowthSpeeds
                .Add(new YearlyGrowthSpeed
                {
                    Rate = GetPercentage(response.TotalDeposit, profit.Rate),
                    Year = profit.Year,
                }));
        }

        public static decimal GetPercentage(decimal totalDeposit, decimal amount)
        {
            return (amount * 100 / totalDeposit).RoundNumber();
        }
    }
}

