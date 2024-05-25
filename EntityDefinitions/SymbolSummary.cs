using System;
namespace EntityDefinitions
{
	public class SymbolSummary : IdBase
	{
		public decimal ClosingPrice { get; set; }
		public DateTime Date { get; set; }
		public string Symbol { get; set; }
		public decimal Volume { get; set; }

		public SymbolSummary()
		{
		}
	}
}

