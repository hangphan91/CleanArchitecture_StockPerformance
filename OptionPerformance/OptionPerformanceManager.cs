using System;
using OptionPerformance.DataAccessors;
using OptionPerformance.DataAccessors.Models;
using OptionPerformance.Logic.Factories;
using OptionPerformance.Models.Interfaces.OptionsTypes;
namespace OptionPerformance
{
    public class OptionPerformanceManager
    {
        public OptionPerformanceManager()
        {

        }

        public static List<IStrategy> GetStrategies()
        {
            var result = new List<IStrategy>();
            var batchedOptionsDatas = OptionDataAccessor.GetOptionsData("AAPL");

            foreach (var optionsData in batchedOptionsDatas.OptionsDatas)
            {
                var strategies = OptionsStrategy.GetOptionStrategy
                 (optionsData.OptionName,
                 batchedOptionsDatas.StockSymbol,
                 optionsData.NumberOfEnterOptions,
                 optionsData.DailyMovingAverage,
                 optionsData.StockPrice,
                 optionsData.YearlyReturn,
                 optionsData.MonthlyReturn,
                 optionsData.OpenInterest,
                 optionsData.OptionPremium,
                 optionsData.StrikePrice,
                 optionsData.ExpirationDate,
                 optionsData.RiskDesc,
                 optionsData.Delta,
                 optionsData.Gamma,
                 optionsData.Theta,
                 optionsData.Vega,
                 optionsData.Rho);
                result.AddRange(strategies);
            }

            return result;
        }
    }
}

