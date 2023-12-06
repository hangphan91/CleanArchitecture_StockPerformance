using System.Text;
using System.Text.Json;
using StockPerformance_CleanArchitecture.Models.Settings;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class SearchDetail
    {
        public SearchInitialSetup SearchSetup { get; set; }
        public DepositRule DepositRule { get; set; }
        public TradingRule TradingRule { get; set; }
        public string Symbol { get; set; }
        public SettingDate SettingDate { get; set; }

        public SearchDetail()
        {
            SearchSetup = new SearchInitialSetup();
            DepositRule = new DepositRule();
            TradingRule = new TradingRule();
            Symbol = "AAPL";
            var now = DateTime.Now;
            SettingDate = new SettingDate(2020, now.Month, now.Day);
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            str.Append(DepositRule.ToString());
            str.Append(TradingRule.ToString());

            ; return str.ToString();
        }

        internal bool IsSame(SearchDetail searchDetail)
        {
            if (searchDetail == null
                || searchDetail.DepositRule == null
                || searchDetail.SearchSetup == null
                || searchDetail.TradingRule == null
                || searchDetail.SearchSetup == null)
                return false;

            return searchDetail.DepositRule.IsSame(DepositRule)
                && searchDetail.SearchSetup.IsSame(SearchSetup)
                && searchDetail.TradingRule.IsSame(TradingRule)
                && searchDetail.Symbol.Equals(Symbol)
                && searchDetail.SettingDate.IsSame(SettingDate);

        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

