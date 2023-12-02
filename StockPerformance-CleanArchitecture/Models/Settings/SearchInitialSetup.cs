using System.Text;

namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class SearchInitialSetup
	{
        public DateOnly StartingYear { get; set; }
        public DateOnly EndingYear { get; set; }
        public List<string> Symbols { get; set; }
		public List<string> AddingSymbols { get; set; }
		public List<string> AddedSymbols { get; set; }
		public string AddSymbol { get; set; }
		public SearchInitialSetup()
		{
			Symbols = new List<string>();
			AddingSymbols = new List<string>();
			AddedSymbols = new List<string>();	
		}
        public override string ToString()
        {
			var str = new StringBuilder();
			str.AppendLine($"Start year: {StartingYear}, End year: {EndingYear}");
            return str.ToString();
        }

        internal bool IsSame(SearchInitialSetup searchSetup)
        {
            return searchSetup.StartingYear.Equals(StartingYear)
                && searchSetup.EndingYear.Equals(EndingYear)
                && Enumerable.SequenceEqual(searchSetup.Symbols, Symbols);
        }
    }
}

