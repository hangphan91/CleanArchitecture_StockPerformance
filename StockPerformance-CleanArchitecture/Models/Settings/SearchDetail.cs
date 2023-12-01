using System.Text;
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
            Symbol = "AAPL";
            Year = 2020;
        }
        public string Symbol { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();

            str.Append(SearchSetup.ToString());
            str.Append(DepositRule.ToString());
            str.Append(TradingRule.ToString());

;            return str.ToString();
        }
    }
}

