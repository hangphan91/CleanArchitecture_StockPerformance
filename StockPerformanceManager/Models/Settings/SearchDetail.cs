using System.Text;
using System.Text.Json;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models.Settings;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class SearchDetail
    {
        public string Name { get; set; }
        public SearchInitialSetup SearchSetup { get; set; }
        public DepositRule DepositRule { get; set; }
        public TradingRule TradingRule { get; set; }
        public string Symbol { get; set; }
        public SettingDate SettingDate { get; set; }
        public bool SaveCurrentSetting { get; set; }
        public List<SearchDetail> ActiveSelectedSearchDetails { get; set; }
        public List<SearchDetail> SavedSearchDetails { get; set; }

        public SearchDetail()
        {
            SearchSetup = new SearchInitialSetup();
            DepositRule = new DepositRule();
            TradingRule = new TradingRule();
            ActiveSelectedSearchDetails = new List<SearchDetail>();
            SavedSearchDetails = new List<SearchDetail>();
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
            return searchDetail.Symbol.ToUpper().Equals(Symbol.ToUpper())
              && searchDetail.SettingDate.IsSame(SettingDate)
              && searchDetail.TradingRule.IsSame(TradingRule);

        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

