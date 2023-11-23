using StockPerformance_CleanArchitecture.Models.Settings;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class SearchDetail
    {
        public SearchInitialSetup SearchSetup { get; set; }
        public DepositRule DepositRule { get; set; }
        public TradingRule TradingRule { get; set; }
        public SearchDetail()
        {
            SearchSetup = new SearchInitialSetup();
            DepositRule = new DepositRule();
            TradingRule = new TradingRule();
        }
        public string Symbol { get; set; }
        public int Year { get; set; }
    }
}

