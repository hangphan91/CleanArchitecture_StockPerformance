namespace OptionPerformance.Models.Interfaces.Logics
{
    public interface IExitOptionsSetup
    {
        ExitOptionsSetup ExitOptionsSetup { get; }
    }

    public class ExitOptionsSetup
    {
        public ExitOptionsSetup(int numberOfExitingOptions, decimal stockPriceToExitOptions,
         decimal optionPremiumToExitOptions, int numberOfWeekUntilExpiration)
        {
            NumberOfExitingOptions = numberOfExitingOptions;
            StockPriceToExitOptions = stockPriceToExitOptions;
            OptionPremiumToExitOptions = optionPremiumToExitOptions;
            NumberOfWeekUntilExpiration = numberOfWeekUntilExpiration;
            IsQualifiedToExit = ExecuteExit();
        }
        public int NumberOfExitingOptions { get; }
        public decimal StockPriceToExitOptions { get; }
        public decimal OptionPremiumToExitOptions { get; }
        public int NumberOfWeekUntilExpiration { get; }
        public bool IsQualifiedToExit { get; }

        public bool ExecuteExit()
        {
            return NumberOfExitingOptions > 0 && NumberOfWeekUntilExpiration <= 4;
        }
    }
}