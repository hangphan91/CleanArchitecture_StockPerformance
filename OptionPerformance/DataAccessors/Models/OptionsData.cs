namespace OptionPerformance.DataAccessors.Models
{
    public class BatchedOptionsData
    {
        public string StockSymbol { get; set; }

        public List<OptionsData> OptionsDatas { get; set; }

        public BatchedOptionsData(string symbol)
        {
            StockSymbol = symbol;
            OptionsDatas = new List<OptionsData>();
        }
    }

    public class OptionsData
    {
        public string OptionName { get; set; }
        public int NumberOfEnterOptions { get; set; }
        public int DailyMovingAverage { get; set; }
        public decimal StockPrice { get; set; }
        public decimal YearlyReturn { get; set; }
        public decimal MonthlyReturn { get; set; }
        public int OpenInterest { get; set; }
        public decimal OptionPremium { get; set; }
        public decimal StrikePrice { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public string RiskDesc { get; set; }
        public decimal Delta { get; set; }
        public decimal Gamma { get; set; }
        public decimal Theta { get; set; }
        public decimal Vega { get; set; }
        public decimal Rho { get; set; }
        public bool IsCall { get; set; }
        public bool IsPut { get; set; }

        public OptionsData(string symbol)
        {
            RiskDesc = "";
            OptionName = "";
        }
    }
}