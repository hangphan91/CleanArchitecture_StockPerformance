using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Models
{
    public class AdvanceSearch
	{
        public SearchDetail SearchDetail { get; set; }
        public int Count { get; set; }
        public List<string> Symbols { get; set; }
        public List<SearchDetail> SearchDetails { get; set; }
        public int StartYear { get; set; } = 2019;
        public int EndYear { get; set; } = 2023;
        public AdvanceSearch()
		{
			SearchDetail = new SearchDetail();
            Symbols = new List<string>();
            SearchDetails = new List<SearchDetail>();
		}
	}
}

