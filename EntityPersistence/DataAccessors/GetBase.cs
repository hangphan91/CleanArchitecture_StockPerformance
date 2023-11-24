using System;
using EntityDefinitions;
using StockPerformanceCalculator.DatabaseAccessors;

namespace EntityPersistence.DataAccessors
{
	public class GetBase :IGet
	{
        DataContext _dataContext;
		public GetBase(DataContext dataContext)
		{
            _dataContext = dataContext;
		}

        public DepositRule GetDepositRule()
        {
            return _dataContext.DepositRules.First();
        }

        public List<PerformanceSummary> GetPerformancesBySymbol(string symbol)
        {
            return _dataContext.PerformanceSummaries
                .Where(performance => performance.Symbol == symbol)
                .ToList();
        }

        public PerformanceSetup GetPerformanceSetup()
        {
            return _dataContext.PerformanceSetups.First();
        }

        public List<long> GetSymbolIds(List<string> symbols)
        {
            return _dataContext.Symbols
                .Where(symbol => symbols.Contains(symbol.TradingSymbol))
                .Select(symbol => symbol.Id)
                .ToList();
        }

        public long GetSymbolId(string symbol)
        {
            return _dataContext.Symbols
                .Where(summary => summary.TradingSymbol == symbol)
                .Select(summary => summary.Id).First();
        }

        public TradingRule GetTradingRule()
        {
            return _dataContext.TradingRules.First();
        }

        public List<string> GetSymbolsBetweenIds(long startingId, long endingId)
        {
            return _dataContext.Symbols
                .Where(symbol => symbol.Id >= startingId && symbol.Id <= endingId)
                .Select(symbol => symbol.TradingSymbol)
                .ToList();
        }

    }
}

