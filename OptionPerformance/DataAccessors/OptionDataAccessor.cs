using System;
using System.Dynamic;
using ExternalCommunicator.ExternalCommunications;
using ExternalCommunicator.ExternalCommunications.Models;
using OptionPerformance.DataAccessors.Models;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Models;
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

            // call stock performance to get stock performance info, price, list of options, greeks
            var stockPerformanceResult = await ManagerHelper.SearchDetailManager.GetStockPerformanceResponse(searchDetail);
            stockPerformanceResult.ProfitSummaryPercentage.SetTotalProfit();

            // call and get optionDa
            var response = await new GetOptionsDataAccessor().GetOptionsData(symbol);

            if (response.OptionChain.Result.First().Options.Any() == false)
                return batchedOptionsData;

            var options = response.OptionChain.Result.First().Options;
            var stockVolume = response.OptionChain.Result.First().Quote?.AverageDailyVolume3Month;

            foreach (var option in options)
            {
                foreach (var stradle in option.Straddles)
                {
                    if (stradle == null)
                        continue;
                    OptionsData call = PopulateOptionsData(symbol, stockPerformanceResult, stockVolume, stradle.Call);

                    if (call != null)
                        batchedOptionsData.OptionsDatas.Add(call);

                    OptionsData put = PopulateOptionsData(symbol, stockPerformanceResult, stockVolume, stradle.Put);

                    if (put != null)
                        batchedOptionsData.OptionsDatas.Add(put);
                }
            }
            // map to optionsData and return
            return batchedOptionsData;
        }

        private static OptionsData PopulateOptionsData(string symbol, StockPerformanceResponse? stockPerformanceResult, int? stockVolume, Call? stradle)
        {
            if (stradle == null || stradle?.Expiration.HasValue == false)
                return null;

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(stradle.Expiration.Value);
            var expirationDate = dateTimeOffset.DateTime;

            return new OptionsData(symbol)
            {
                StockPrice = stockPerformanceResult?.CurrentPrice ?? 0,
                YearlyReturn = stockPerformanceResult?.ProfitInPercentage ?? 0,
                MonthlyReturn = stockPerformanceResult?.ProfitSummaryPercentage?.AVGMonthlyProfit ?? 0,
                StrikePrice = (decimal)(stradle.Strike ?? 0),
                ExpirationDate = DateOnly.FromDateTime(expirationDate),
                RiskDesc = "",
                DailyMovingAverage = stockVolume ?? 0,
                OptionName = stradle.ContractSymbol,
                NumberOfEnterOptions = 1,
                OpenInterest = stradle.OpenInterest ?? 0,
                OptionPremium = (decimal)3.4,
                Delta = (decimal)0.7,
                Gamma = (decimal)0.6,
                Theta = (decimal)0.6,
                Vega = (decimal)0.6,
                Rho = (decimal)0.6
            };
        }

        private static OptionsData PopulateOptionsData(string symbol, StockPerformanceResponse? stockPerformanceResult, int? stockVolume, Put? stradle)
        {
            if (stradle == null)
                return null;

            return new OptionsData(symbol)
            {
                StockPrice = stockPerformanceResult?.CurrentPrice ?? 0,
                YearlyReturn = stockPerformanceResult?.ProfitInPercentage ?? 0,
                MonthlyReturn = stockPerformanceResult?.ProfitSummaryPercentage?.AVGMonthlyProfit ?? 0,
                StrikePrice = (decimal)(stockPerformanceResult?.CurrentPrice ?? 0 + 5),
                ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                RiskDesc = "",
                DailyMovingAverage = stockVolume ?? 0,
                OptionName = stradle.ContractSymbol,
                NumberOfEnterOptions = 1,
                OpenInterest = 600,
                OptionPremium = (decimal)3.4,
                Delta = (decimal)0.7,
                Gamma = (decimal)0.6,
                Theta = (decimal)0.6,
                Vega = (decimal)0.6,
                Rho = (decimal)0.6
            };
        }
    }
}

