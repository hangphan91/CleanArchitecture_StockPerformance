using OptionPerformance.Models.Interfaces;
using OptionPerformance.Models.Interfaces.Logics;
using OptionPerformance.Models.Interfaces.OptionsTypes;

namespace OptionPerformance.Models.OptionsStrategies
{
    public class LongCallStrategy : IStrategy, ICallOptions, ICapitalGainOptions
    {
        private OptionsLegs? _optionsLegs;
        private LongCall _longCall;
        private ExitOptionsSetup _exitOptionsSetup;
        private EnteringOptionsSetup _enteringOptionsSetup;
        public OptionsLegs OptionsLegs => _optionsLegs ?? new OptionsLegs();

        OptionStrikePriceAt _optionStrikePriceAt;

        public ExitOptionsSetup ExitOptionsSetup => _exitOptionsSetup;

        public EnteringOptionsSetup EnteringOptionsSetup => _enteringOptionsSetup;

        public LongCall LongCall => _longCall;

        public LongCallStrategy(LongCall longCall, OptionStrikePriceAt optionStrikePriceAt, int openInterest)
        {
            _longCall = longCall;
            _optionsLegs = new OptionsLegs(longCall);
            _optionStrikePriceAt = optionStrikePriceAt;

            //TODO: may need to break this part to a separate logic
            int numberOfExitingOptions = longCall.OptionsLeg.NumberOfOptions;
            decimal stockPriceToExitOptions = longCall.SelectingStock.StockPrice * (decimal)-.80;
            decimal optionPremiumToExitOptions = longCall.SelectingOptions.BreakEven * 2;
            var numberOfDays = longCall.SelectingOptions.ExpirationDate.ToDateTime(new TimeOnly()).Subtract(DateTime.UtcNow.Date).Days;

            decimal stockPriceToEnterOptions = longCall.SelectingStock.StockPrice;
            decimal optionPremiumToEnterOptions = longCall.SelectingOptions.OptionsPremium;
            int numberOfWeekUntilExpiration = (int)numberOfDays / 7;

            _exitOptionsSetup = new ExitOptionsSetup(numberOfExitingOptions, stockPriceToExitOptions,
             optionPremiumToExitOptions, numberOfWeekUntilExpiration);

            var numberOfWeekToHoldUntilExpiration = (int)numberOfDays / 7;
            _enteringOptionsSetup = new EnteringOptionsSetup(numberOfExitingOptions, stockPriceToEnterOptions,
            optionPremiumToEnterOptions, numberOfWeekToHoldUntilExpiration, _optionStrikePriceAt, openInterest);
        }

        public decimal? ActualReturn(decimal stockPrice)
        {
            return _exitOptionsSetup.IsQualifiedToExit ? (stockPrice - _longCall.SelectingOptions.BreakEven) * _longCall.OptionsLeg.NumberOfOptions : null;
        }

        public decimal CalculateMaxReward()
        {
            return decimal.MaxValue;
        }

        public decimal CalculateMaxRisk()
        {
            return _longCall.SelectingOptions.OptionsPremium * 100;
        }

        public decimal CurrentUnrealizedGain(decimal stockPrice)
        {
            return (stockPrice - _longCall.SelectingOptions.BreakEven) * _longCall.OptionsLeg.NumberOfOptions;
        }

        public decimal MarginCollateral()
        {
            return 0;
        }

        decimal? ICalculatingOptionsReturnBase.CurrentUnrealizedGain(decimal stockPrice)
        {
            return _enteringOptionsSetup.IsQualifiedToEnter ? (stockPrice - _longCall.SelectingOptions.BreakEven) * _longCall.OptionsLeg.NumberOfOptions : null;
        }
    }
}