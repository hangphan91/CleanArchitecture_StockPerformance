namespace OptionPerformance.Models.Interfaces.Logics
{
    public interface IEnteringOptionsSetup
    {
        EnteringOptionsSetup EnteringOptionsSetup { get; }
    }

    public class EnteringOptionsSetup
    {
        public EnteringOptionsSetup(int numberOfEnteringOptions, decimal stockPriceToEnterOptions,
         decimal optionPremiumToEnterOptions, int numberOfWeekToHoldUntilExpiration)
        {
            NumberOfEnterOptions = numberOfEnteringOptions;
            StockPriceToEnterOptions = stockPriceToEnterOptions;
            OptionPremiumToEnterOptions = optionPremiumToEnterOptions;
            NumberOfWeekToHoldUntilExpiration = numberOfWeekToHoldUntilExpiration;
            IsQualifiedToEnter = ExecuteEnter();
        }
        public int NumberOfEnterOptions { get; set; }
        public decimal StockPriceToEnterOptions { get; set; }
        public decimal OptionPremiumToEnterOptions { get; set; }
        public int NumberOfWeekToHoldUntilExpiration { get; set; }
        public bool IsQualifiedToEnter { get; }
        public bool ExecuteEnter() { return NumberOfEnterOptions >= 0; }
    }
}