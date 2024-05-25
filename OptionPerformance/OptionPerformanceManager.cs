using System;
using OptionPerformance.DataAccessors;
using OptionPerformance.DataAccessors.Models;
using OptionPerformance.Logic.Factories;
using OptionPerformance.Models.Interfaces;
using OptionPerformance.Models.Interfaces.OptionsTypes;
namespace OptionPerformance
{
    public class OptionPerformanceManager
    {
        public OptionPerformanceManager()
        {

        }

        public static async Task<List<IStrategy>> GetStrategies()
        {
            var result = new List<IStrategy>();
            var batchedOptionsDatas = await OptionDataAccessor.GetOptionsData("AAPL");

            foreach (var optionsData in batchedOptionsDatas.OptionsDatas)
            {
                var optionStrikePriceAt = OptionStrikePriceAt.AtTheMoney;

                if (optionsData.StockPrice > optionsData.StrikePrice)
                    optionStrikePriceAt = OptionStrikePriceAt.InTheMoney;
                else if (optionsData.StockPrice < optionsData.StockPrice)
                    optionStrikePriceAt = OptionStrikePriceAt.OutOfMoney;

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
                 optionsData.Rho,
                 optionsData.IsCall,
                 optionsData.IsPut,
                 optionStrikePriceAt);
                result.AddRange(strategies);
            }

            return result;
        }

        public static async Task<List<IOptionsLeg>> EnterOptions(List<IStrategy> strategies)
        {
            var addingOptions = new List<IOptionsLeg>();
            foreach (var strategy in strategies)
            {
                if (strategy.EnteringOptionsSetup.IsQualifiedToEnter)
                {
                    addingOptions.AddRange(strategy.OptionsLegs.OptionsLegList.ToList());
                }
            }

            return addingOptions;
        }


        public static async Task<List<IOptionsLeg>> ExitOptions(List<IStrategy> strategies)
        {
            var addingOptions = new List<IOptionsLeg>();
            foreach (var strategy in strategies)
            {
                if (strategy.ExitOptionsSetup.IsQualifiedToExit)
                {
                    addingOptions.AddRange(strategy.OptionsLegs.OptionsLegList.ToList());
                }
            }

            return addingOptions;
        }
    }
}

