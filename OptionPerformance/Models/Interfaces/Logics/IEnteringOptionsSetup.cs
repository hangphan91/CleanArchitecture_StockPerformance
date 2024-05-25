namespace OptionPerformance.Models.Interfaces.Logics
{
    public interface IEnteringOptionsSetup
    {
        EnteringOptionsSetup EnteringOptionsSetup { get; }
    }

    public class EnteringOptionsSetup
    {
        public EnteringOptionsSetup(int numberOfEnteringOptions, decimal stockPriceToEnterOptions,
         decimal optionPremiumToEnterOptions, int numberOfWeekToHoldUntilExpiration,
         OptionStrikePriceAt optionStrikePriceAt, int openInterest)
        {
            NumberOfEnterOptions = numberOfEnteringOptions;
            StockPriceToEnterOptions = stockPriceToEnterOptions;
            OptionPremiumToEnterOptions = optionPremiumToEnterOptions;
            NumberOfWeekToHoldUntilExpiration = numberOfWeekToHoldUntilExpiration;
            OptionStrikePriceAt = optionStrikePriceAt;
            OpenInterest = openInterest;
            IsQualifiedToEnter = ExecuteEnter(optionStrikePriceAt);
        }

        public int OpenInterest { get; set; }
        public int NumberOfEnterOptions { get; set; }
        public decimal StockPriceToEnterOptions { get; set; }
        public decimal OptionPremiumToEnterOptions { get; set; }
        public int NumberOfWeekToHoldUntilExpiration { get; set; }
        public bool IsQualifiedToEnter { get; }
        public OptionStrikePriceAt OptionStrikePriceAt { get; }
        public bool ExecuteEnter(OptionStrikePriceAt optionStrikePriceAt)
        {
            return NumberOfEnterOptions >= 0
                    && NumberOfWeekToHoldUntilExpiration > 20
                    && OptionStrikePriceAt == optionStrikePriceAt
                    && OpenInterest > 500;
        }
    }
}