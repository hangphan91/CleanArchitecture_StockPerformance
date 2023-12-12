using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.Models;

namespace StockPerformance_CleanArchitecture.Models
{
    public class StockPerformanceResponse
    {
        public string Symbol { get; set; }
        public DateDetail StartDate { get; set; }
        public ProfitSummaryInDollar ProfitSummaryInDollar { get; set; }
        public ProfitSummaryPercentage ProfitSummaryPercentage { get; set; }
        public List<DepositLedger> DepositLedgers { get; set; }
        public List<StockLedgerDetail> StockLedgerDetails { get; set; }
        public List<SymbolSummary> SymbolSummaries { get; set; }
        public string ToDisplay { get; set; }
        public decimal TotalBalanceAfterLoss { get; set; } = 0;
        public decimal TotalDeposit { get; set; } = 0;
        public decimal TotalBalanceHoldingInPosition { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentHoldingShare { get; set; }
        public decimal ProfitInDollar { get; set; }
        public decimal ProfitInPercentage { get; set; }
        public SearchDetail SearchDetail { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public ProfitChart ProfitChart { get; set; }

        public StockPerformanceResponse()
        {
            ProfitSummaryInDollar = new ProfitSummaryInDollar();
            ProfitSummaryPercentage = new ProfitSummaryPercentage();
            DepositLedgers = new List<DepositLedger>();
            StockLedgerDetails = new List<StockLedgerDetail>();
            SearchDetail = new SearchDetail();
            SymbolSummaries = new List<SymbolSummary>();

        }
        public StockPerformanceResponse(string symbol, DateDetail startDate)
        {
            ProfitSummaryPercentage = new ProfitSummaryPercentage();
            ProfitSummaryInDollar = new ProfitSummaryInDollar();
            SymbolSummaries = new List<SymbolSummary>();
            Symbol = symbol;
            StartDate = startDate;
        }

        public string DisplayStockPerformance()
        {

            var toDisplay = $"The performance report for CPRT, reflecting on the period from {StartDate.ToString()}" +
                $" indicates a total deposit of ${TotalDeposit} and " +
                $"a total holding in position of ${TotalBalanceHoldingInPosition}." +
                $" The deposit rule involved an initial one-time deposit of ${SearchDetail.DepositRule.InitialDepositAmount} " +
                $"and a monthly repeated deposit of ${SearchDetail.DepositRule.DepositAmount}," +
                $" made on the 1st and 16th of each month." +
                $" The trading rule specified a purchase limitation of {SearchDetail.TradingRule.PurchaseLimitation} per trade," +
                $" with conditions to sell when the total investment reached {SearchDetail.TradingRule.SellPercentageLimitation}%," +
                $" when the overall loss amounted to ${SearchDetail.TradingRule.LossLimitation}, or when the price dropped " +
                $"by {SearchDetail.TradingRule.SellAllWhenPriceDropAtPercentageSinceLastTrade}% " +
                $"since the last visit. It also included a buy condition" +
                $" when the overall gain reached {SearchDetail.TradingRule.BuyPercentageLimitation}%. The trading was limited" +
                $" to between the {SearchDetail.TradingRule.LowerRangeOfTradingDate} and " +
                $"{SearchDetail.TradingRule.HigherRangeOfTradingDate} day of the month, " +
                $"with a maximum of {SearchDetail.TradingRule.NumberOfTradeAMonth} " +
                $"trades per month. The current stock price is ${CurrentPrice}, and the " +
                $"current holding share is {CurrentHoldingShare}.";

            return toDisplay;
        }

        internal StockPerformanceResponse Map(StockPerformanceSummary summary)
        {
            var response = new StockPerformanceResponse
            {
                StartDate = summary.StartDate,
                Symbol = summary.Symbol,
                TotalBalanceAfterLoss = summary.TotalBalanceAfterLoss.RoundNumber(),
                TotalDeposit = summary.TotalDeposit.RoundNumber(),
                CurrentPrice = summary.CurrentPrice.RoundNumber(),
                CurrentHoldingShare = summary.CurrentHoldingShare.RoundNumber(),
                ProfitInDollar = summary.ProfitInDollar.RoundNumber(),
                ProfitInPercentage = summary.ProfitInPercentage.RoundNumber(),
                TotalBalanceHoldingInPosition = summary.TotalBalanceHoldingInPosition.RoundNumber(),
                StockLedgerDetails = Map(summary.StockLedger),
                DepositLedgers = summary.DepositLedgers,
                SymbolSummaries = new List<SymbolSummary>(),
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

        private List<StockLedgerDetail> Map(StockLedger stockLedger)
        {
            return stockLedger.All.Select(a => new StockLedgerDetail
            {
                BoughtDate = a.BoughtDate,
                PositionType = a.PositionType,
                BoughtPrice = a.BoughtPrice.RoundNumber(),
                ShareCount= a.ShareCount.RoundNumber(),
                SoldPrice = a.SoldPrice?.RoundNumber(),
                SoldDate = a.SoldDate,
                SellReason = a.SellReason,
            }).OrderBy(a =>a.BoughtDate).ToList();
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

