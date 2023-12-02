using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformance_CleanArchitecture.Models.Settings;

namespace StockPerformance_CleanArchitecture.Models
{
    public class AdvanceSearch
	{
        public SearchDetail SearchDetail { get; set; }
        public int Count { get; set; }
        public List<string> Symbols { get; set; }
        public List<SearchDetail> SearchDetails { get; set; }
        public SettingDate StartDate { get; set; } = new SettingDate(2020, 0,0);
        public SettingDate EndDate { get; set; } = new SettingDate(2023, 12,1);
        public AdvanceSearch()
		{
			SearchDetail = new SearchDetail();
            Symbols = new List<string>();
            SearchDetails = new List<SearchDetail>();
            Count = 0;
		}
	}
}

