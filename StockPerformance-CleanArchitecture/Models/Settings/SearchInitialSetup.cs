using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class SearchInitialSetup
	{
        public int StartingYear { get; set; }
        public int EndingYear { get; set; }
        public List<string> Symbols { get; set; }
        public SearchInitialSetup()
		{
			Symbols = new List<string>();
		}
	}
}

