using System;
using System.Data;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.Models;
using Utilities;

namespace StockPerformance_CleanArchitecture.Formatters
{
    public class DataTableFormatter
    {
        public DataTableFormatter()
        {
        }

        public static DataTable GetMonthlyProfitDataTable(List<MonthlyProfit> profits, string unit)
        {
            var table = new DataTable($"Monthly Profit ({unit})");

            table.Columns.Add("Month", typeof(int));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add($"Profit {unit}", typeof(decimal));

            foreach (var item in profits)
            {
                table.Rows.Add(
                    item.Month, item.Year, item.Amount);
            }
            return table;
        }

        public static DataTable GetYearlyProfitDataTable(List<YearlyProfit> profits, string unit)
        {
            var table = new DataTable($"Yearly Profits ({unit})");

            table.Columns.Add("Year", typeof(int));
            table.Columns.Add($"Profit ({unit})", typeof(decimal));

            foreach (var item in profits)
            {
                table.Rows.Add(item.Year, item.Amount);
            }
            return table;
        }

        public static DataTable GetStockLedgerDataTable(List<StockLedgerDetail> profits)
        {
            var table = new DataTable("Stock Ledgers");

            table.Columns.Add("Bought Date", typeof(DateTime));
            table.Columns.Add("Sold Date", typeof(DateTime));
            table.Columns.Add("Share Count", typeof(decimal));
            table.Columns.Add("Bought Price", typeof(decimal));
            table.Columns.Add("Sold Price", typeof(decimal));
            table.Columns.Add("Position Type", typeof(string));
            table.Columns.Add("Sell Reason", typeof(string));

            foreach (var item in profits)
            {
                table.Rows.Add(item.BoughtDate,
                    item.SoldDate, item.ShareCount,
                    item.BoughtPrice, item.SoldPrice,
                    item.PositionType, item.SellReason);
            }
            return table;
        }

        public static DataTable GetDepositLedgerDataTable(List<DepositLedger> profits)
        {
            var table = new DataTable("Deposit Ledgers");

            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Amount $", typeof(decimal));

            foreach (var item in profits)
            {
                table.Rows.Add(item.Date, item.Amount);
            }
            return table;
        }

        public static DataTable GetSettingDataTable(SearchDetail item)
        {
            DataTable table = new DataTable("Trading rules");
            table.Columns.Add("One time deposit amount", typeof(decimal));
            table.Columns.Add("Repetitive deposit amount", typeof(decimal));
            table.Columns.Add("First date of deposit", typeof(int));

            table.Columns.Add("Last date of deposit", typeof(int));
            table.Columns.Add("Buy more when holding % of the total investment", typeof(decimal));
            table.Columns.Add("Sell all when holding % of the total investment", typeof(decimal));

            table.Columns.Add("Maximum amount to trade at once", typeof(decimal));
            table.Columns.Add("Lost limitation $", typeof(decimal));
            table.Columns.Add("Lost limitation %", typeof(decimal));

            table.Columns.Add("Number of trade monthly", typeof(int));
            table.Columns.Add("We will trade from date of a month", typeof(int));
            table.Columns.Add("We will trade to date of a month", typeof(int));

            table.Rows.Add(
                item.DepositRule.InitialDepositAmount,
                item.DepositRule.DepositAmount,
                item.DepositRule.FirstDepositDate,
                item.DepositRule.SecondDepositDate,
                item.TradingRule.BuyPercentageLimitation,
                item.TradingRule.SellPercentageLimitation,
                item.TradingRule.PurchaseLimitation,
                item.TradingRule.LossLimitation,
                item.TradingRule.SellAllWhenPriceDropAtPercentageSinceLastTrade,
                item.TradingRule.NumberOfTradeAMonth,
                item.TradingRule.LowerRangeOfTradingDate,
                item.TradingRule.HigherRangeOfTradingDate
                );
            return table;
        }


        public static DataTable GetStockPerformanceReponseDataTable(List<StockPerformanceResponse> list)
        {
            DataTable table = new DataTable("Performance Result");
            table.Columns.Add("Symbol", typeof(string));
            table.Columns.Add("Current Price $", typeof(decimal));

            table.Columns.Add("Start Date", typeof(Models.Settings.SettingDate));
            table.Columns.Add("End Date", typeof(DateOnly));

            table.Columns.Add("Max Yearly Profit %", typeof(decimal));
            table.Columns.Add("Min Yearly Profit %", typeof(decimal));
            table.Columns.Add("Average Yearly Profit %", typeof(decimal));

            table.Columns.Add("Max Monthly Profit %", typeof(decimal));
            table.Columns.Add("Min Monthly Profit %", typeof(decimal));
            table.Columns.Add("Average Monthly Profit %", typeof(decimal));

            table.Columns.Add("Total Profit %", typeof(decimal));

            foreach (var item in list)
            {
                if (item == null)
                    continue;

                item.ProfitSummaryPercentage?.SetTotalProfit();
                var basedURL = "https://stockperformance.azurewebsites.net/";

                var link = $"{basedURL}?symbol={item.Symbol}&startYear={item.SearchDetail?.SettingDate.Year}" +
                    $"&Name={item.SearchDetail?.Name}";

                var yearlyMaxGrowth = item.ProfitSummaryPercentage?.MAXYearlyProfit;
                var yearlyMinGrowth = item.ProfitSummaryPercentage?.MINYearlyProfit;
                var yearlyAVGGrowth = item.ProfitSummaryPercentage?.AVGYearlyProfit;
                var monthlyMaxGrowth = item.ProfitSummaryPercentage?.MAXMonthlyProfit;
                var monthlyMinGrowth = item.ProfitSummaryPercentage?.MINMonthlyProfit;
                var monthlyAVGGrowth = item.ProfitSummaryPercentage?.AVGMonthlyProfit;

                table.Rows.Add($"<a href={link}>{item.Symbol}</a>",
                    item.CurrentPrice.RoundNumber(),
                    item.SearchDetail?.SettingDate,
                    item.SearchDetail?.SearchSetup?.EndingYear,
                    yearlyMaxGrowth.RoundNumber(),
                    yearlyMinGrowth.RoundNumber(),
                    yearlyAVGGrowth.RoundNumber(),
                    monthlyMaxGrowth.RoundNumber(),
                    monthlyMinGrowth.RoundNumber(),
                    monthlyAVGGrowth.RoundNumber(),
                    item.ProfitInPercentage.RoundNumber()
                    );
            }

            return table;
        }

    }
}

