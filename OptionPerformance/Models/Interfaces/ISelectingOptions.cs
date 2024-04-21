namespace OptionPerformance.Models.Interfaces
{
    public interface ISelectingOptions
    {
        SelectingOptions SelectingOptions { get; }
    }

    public class SelectingOptions
    {
        public SelectingOptions(int openInterest, decimal optionsPremium,
        decimal strikePrice, DateOnly expiration)
        {
            OpenInterest = openInterest;
            OptionsPremium = optionsPremium;
            StrikePrice = strikePrice;
            ExpirationDate = expiration;
            BreakEven = strikePrice + optionsPremium;
        }
        public int OpenInterest { get; set; }
        public decimal OptionsPremium { get; set; }
        public decimal StrikePrice { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public decimal BreakEven { get; set; }
    }
}