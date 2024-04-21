using Exporter;
using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.Models;
using Utilities;

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

        public string DisplayPerformanceResultTable()
        {
            return PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(new List<StockPerformanceResponse> { this});
        }

        public string DisplayStockPerformanceSetting()
        {
            return PerformanceResultFormatter.GetSettingTableHTML(this.SearchDetail);
        }

        public string GetAllHTMLs()
        {
            return PerformanceResultFormatter.GetAllHTMLs(this);
        }

        public string GetSaveFilePerformanceResultTable()
        {
            return PerformanceResultFormatter.ExportDataTableToExcelFormatAndGetFile(new List<StockPerformanceResponse> { this });
        }

        public string GetSaveFilePerformanceResultTableForAll()
        {
            return PerformanceResultFormatter.ExportDataTableToExcelFormatAndGetFile(this);
        }

        public string GetSaveFileStockPerformanceSetting()
        {
            return PerformanceResultFormatter.ExportDataTableToExcelFormatAndGetFile(this.SearchDetail);
        }

        public string Conclusion()
        {
            var text = ProfitInPercentage > 0 ? "increase." : "decrease.";
            var dateText = $"{SearchDetail.SettingDate} to {SearchDetail.SearchSetup.EndingYear}";
            var overallPerformance = $"The profit over the period from " + dateText +
          $" was ${ProfitInDollar}, representing a {ProfitInPercentage}% {text}";

            return overallPerformance;
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
                ShareCount = a.ShareCount.RoundNumber(),
                SoldPrice = a.SoldPrice?.RoundNumber(),
                SoldDate = a.SoldDate,
                SellReason = a.SellReason,
            }).OrderBy(a => a.BoughtDate).ToList();
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
                    Amount = GetPercentage(GetTotalDepositUptoDate(response, profit), profit.Amount),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.ProfitByYears
                .ForEach(profit => response.ProfitSummaryPercentage.YearlyProfits
                .Add(new YearlyProfit
                {
                    Amount = GetPercentage(GetTotalDepositUptoDate(response, profit), profit.Amount),
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByMonths
                .ForEach(profit => response.ProfitSummaryPercentage.MonthlyGrowthSpeeds
                .Add(new MonthlyGrowthSpeed
                {
                    Rate = GetPercentage(GetTotalDepositUptoDate(response, profit), profit.Rate),
                    Month = profit.Month,
                    Year = profit.Year,
                }));

            summary.GrowthSpeedByYears
                .ForEach(profit => response.ProfitSummaryPercentage.YearlyGrowthSpeeds
                .Add(new YearlyGrowthSpeed
                {
                    Rate = GetPercentage(GetTotalDepositUptoDate(response, profit), profit.Rate),
                    Year = profit.Year,
                }));
        }

        private static decimal GetTotalDepositUptoDate(StockPerformanceResponse response, StockPerformanceCalculator.Models.GrowthSpeeds.GrowthSpeedByMonth profit)
        {
            return response.DepositLedgers
                                .Where(a => a.Date < (new DateOnly(profit.Year, profit.Month, 1)).ToDateTime(default).AddMonths(1).AddDays(-1))
                                .Sum(a => a.Amount);
        }

        private static decimal GetTotalDepositUptoDate(StockPerformanceResponse response, ProfitByMonth profit)
        {
            return response.DepositLedgers
                                .Where(a => a.Date < (new DateOnly(profit.Year, profit.Month, 1)).ToDateTime(default).AddMonths(1).AddDays(-1))
                                .Sum(a => a.Amount);
        }

        private static decimal GetTotalDepositUptoDate(StockPerformanceResponse response, ProfitByYear profit)
        {
            return response.DepositLedgers
                                .Where(a => a.Date < (new DateOnly(profit.Year, 12, 1)).ToDateTime(default).AddMonths(1).AddDays(-1))
                                .Sum(a => a.Amount);
        }

        private static decimal GetTotalDepositUptoDate(StockPerformanceResponse response,
            StockPerformanceCalculator.Models.GrowthSpeeds.GrowthSpeedByYear profit)
        {
            return response.DepositLedgers
                                .Where(a => a.Date < (new DateOnly(profit.Year, 12, 1)).ToDateTime(default).AddMonths(1).AddDays(-1))
                                .Sum(a => a.Amount);
        }

        public static decimal GetPercentage(decimal totalDeposit, decimal amount)
        {
            if (totalDeposit == 0)
                return 0;

            return (amount * 100 / totalDeposit).RoundNumber();
        }
    }
}

