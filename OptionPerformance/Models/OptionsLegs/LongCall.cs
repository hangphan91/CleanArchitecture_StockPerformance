using OptionPerformance.Models.Interfaces;

namespace OptionPerformance.Models.OptionsStrategies
{
    public class LongCall : IOptionsLeg
    {
        private static string _description = "Buying a call. Benefits: Capped risk, uncapped reward, better leverage then stock purchase.";
        private OptionsLeg _optionLeg;
        private SelectingOptions _selectingOptions;
        private SelectingStock _selectingStock;
        private OptionsGreek _optionsGreek;
        public LongCall(
        string optionName, string stockSymbol, int numberOfEnterOptions,
        int dailyMovingAverage, decimal stockPrice, decimal yearlyReturn,
        decimal monthlyReturn, int openInterest, decimal optionPremium,
        decimal strikePrice, DateOnly expirationDate, string riskDesc,
        decimal delta, decimal gamma, decimal theta, decimal vega, decimal rho)
        {
            _optionLeg = new OptionsLeg(_description, TrendingDirection.Up, optionName, stockSymbol, numberOfEnterOptions);
            _selectingOptions = new SelectingOptions(openInterest, optionPremium, strikePrice, expirationDate);
            _selectingStock = new SelectingStock(dailyMovingAverage, stockPrice, yearlyReturn, monthlyReturn);
            _optionsGreek = new OptionsGreek(riskDesc, delta, gamma, theta, vega, rho);
        }

        public OptionsLeg OptionsLeg { get => _optionLeg; }
        public SelectingStock SelectingStock { get => _selectingStock; }
        public SelectingOptions SelectingOptions { get => _selectingOptions; }
        public OptionsGreek OptionsGreek { get => _optionsGreek; }
    }
}