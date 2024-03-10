using System;
namespace StockPerformanceCalculator.DatabaseAccessors
{
	public interface IGet
	{
		List<EntityDefinitions.PerformanceSummary> GetPerformancesBySymbol(string symbol);
		EntityDefinitions.DepositRule GetDepositRule();
		EntityDefinitions.TradingRule GetTradingRule();
		long GetSymbolId(string symbol);
		public List<long> GetSymbolIds(List<string> symbols);
        public List<string> GetSymbolsBetweenIds(long startingId, long endingId);
        EntityDefinitions.PerformanceSetup GetPerformanceSetup();
		public List<string> GetSavedSymbols(List<string> symbols);
        public List<string> GetAllSavedSymbols();
        public List<EntityDefinitions.Email> GetEmails();
		public List<Tuple<EntityDefinitions.DepositRule,
			EntityDefinitions.TradingRule,
			EntityDefinitions.Symbol,
			EntityDefinitions.PerformanceSetup, string>> GetSearchDetails();
	}
}

