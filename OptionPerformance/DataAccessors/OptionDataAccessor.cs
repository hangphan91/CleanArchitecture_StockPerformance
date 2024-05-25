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

            if (response?.OptionSymbol?.Any() == false || response == null)
                return batchedOptionsData;

            var options = response.OptionSymbol;

            for (int i = 0; i < options.Count; i++)
            {
                var isCall = response.Side[i] == "call";
                var isPut = response.Side[i] == "put";
                var expirationDate = response.Expiration[i];
                var strike = response.Strike[i];
                var openInterest = response.OpenInterest[i];
                var contract = response.OptionSymbol[i];
                var stockVolume = (int)stockPerformanceResult.Volume;

                if (isCall)
                {
                    OptionsData call = PopulateOptionsData(symbol, stockPerformanceResult, stockVolume, expirationDate, strike, contract, openInterest, true, false);

                    if (call != null)
                        batchedOptionsData.OptionsDatas.Add(call);
                }
                else if (isPut)
                {
                    OptionsData put = PopulateOptionsData(symbol, stockPerformanceResult, stockVolume, expirationDate, strike, contract, openInterest, false, true);

                    if (put != null)
                        batchedOptionsData.OptionsDatas.Add(put);
                }
            }

            // map to optionsData and return
            return batchedOptionsData;
        }

        private static OptionsData PopulateOptionsData(string symbol, StockPerformanceResponse? stockPerformanceResult, int? stockVolume,
        int? expiration, decimal? strike, string contractSymbol, int? openInterest, bool isCall, bool isPut)
        {
            if (expiration.HasValue == false)
                return null;

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expiration.Value);
            var expirationDate = dateTimeOffset.DateTime;

            return new OptionsData(symbol)
            {
                StockPrice = stockPerformanceResult?.CurrentPrice ?? 0,
                YearlyReturn = stockPerformanceResult?.ProfitInPercentage ?? 0,
                MonthlyReturn = stockPerformanceResult?.ProfitSummaryPercentage?.AVGMonthlyProfit ?? 0,
                StrikePrice = (decimal)(strike ?? 0),
                ExpirationDate = DateOnly.FromDateTime(expirationDate),
                RiskDesc = "",
                DailyMovingAverage = stockVolume ?? 0,
                OptionName = contractSymbol,
                NumberOfEnterOptions = 1,
                OpenInterest = openInterest ?? 0,
                OptionPremium = (decimal)3.4,
                Delta = (decimal)0.7,
                Gamma = (decimal)0.6,
                Theta = (decimal)0.6,
                Vega = (decimal)0.6,
                Rho = (decimal)0.6,
                IsCall = isCall,
                IsPut = isPut,
            };
        }
    }
}

