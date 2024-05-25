using OptionPerformance.Models.Interfaces;
using OptionPerformance.Models.Interfaces.OptionsTypes;
using OptionPerformance.Models.OptionsStrategies;

namespace OptionPerformance.Logic.Factories
{
    public class OptionsStrategy
    {
        public static List<IStrategy> GetOptionStrategy(
            string optionName, string stockSymbol, int numberOfEnterOptions,
            int dailyMovingAverage, decimal stockPrice, decimal yearlyReturn,
            decimal monthlyReturn, int openInterest, decimal optionPremium,
            decimal strikePrice, DateOnly expirationDate, string riskDesc,
            decimal delta, decimal gamma, decimal theta, decimal vega, decimal rho,
            bool isCall, bool isPut, OptionStrikePriceAt optionStrikePriceAt)
        {
            var strategies = new List<IStrategy>();
            if (isCall)
            {
                var longCall = new LongCall(optionName, stockSymbol, numberOfEnterOptions, dailyMovingAverage,
                stockPrice, yearlyReturn, monthlyReturn, openInterest, optionPremium, strikePrice, expirationDate,
                riskDesc, delta, gamma, theta, vega, rho);

                if (optionStrikePriceAt == OptionStrikePriceAt.InTheMoney)
                    strategies.Add(new LongCallStrategy(longCall, optionStrikePriceAt, openInterest));
            }

            return strategies;
        }
    }
}