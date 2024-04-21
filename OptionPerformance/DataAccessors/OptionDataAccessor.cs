using System;
using System.Dynamic;
using OptionPerformance.DataAccessors.Models;
using StockPerformance_CleanArchitecture.Helpers;
namespace OptionPerformance.DataAccessors
{
    public class OptionDataAccessor
    {
        public static async Task<BatchedOptionsData> GetOptionsData(string symbol)
        {
            var batchedOptionsData = new BatchedOptionsData(symbol);
            // call api to get Option info
            var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            var searchDetail = SearchDetailHelper.GetCurrentSearchDetail(accessor);
            searchDetail.Symbol = symbol;
            var stockPerformanceResult = await ManagerHelper.SearchDetailManager.GetStockPerformanceResponse(searchDetail);
            // call stock performance to get stock performance info, price, list of options, greeks
            // call and get optionDa
            stockPerformanceResult.ProfitSummaryPercentage.SetTotalProfit();
            var today = DateTime.UtcNow;
            var volume = stockPerformanceResult?.SymbolSummaries?.OrderBy(a => a.Date)?.Where(a => a.Date > today)?.FirstOrDefault()?.Volume;
            OptionsData item = new OptionsData(symbol)
            {
                StockPrice = stockPerformanceResult?.CurrentPrice ?? 0,
                YearlyReturn = stockPerformanceResult?.ProfitInPercentage ?? 0,
                MonthlyReturn = stockPerformanceResult?.ProfitSummaryPercentage?.AVGMonthlyProfit ?? 0,
                StrikePrice = (decimal)(stockPerformanceResult?.CurrentPrice ?? 0 + 5),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                RiskDesc = "",
                DailyMovingAverage = volume ?? 0,
                OptionName = "",
                NumberOfEnterOptions = 1,
                OpenInterest = 600,
                OptionPremium = (decimal)3.4,
                Delta = (decimal)0.7,
                Gamma = (decimal)0.6,
                Theta = (decimal)0.6,
                Vega = (decimal)0.6,
                Rho = (decimal)0.6
            };
            batchedOptionsData.OptionsDatas.Add(item);
            // map to optionsData and return
            return batchedOptionsData;
        }
    }
}

