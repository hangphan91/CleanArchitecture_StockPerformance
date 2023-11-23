using System;
namespace StockPerformanceCalculator.DatabaseAccessors
{
	public interface IInsert
	{
		long Insert(EntityDefinitions.TradingRule tradingRule);
		long Insert(EntityDefinitions.DepositRule depositRule);
		long Insert(EntityDefinitions.PerformanceSummary performance);
		List<long> Insert(List<EntityDefinitions.SymbolSummary> symbolSummaries);
		List<long> Insert(List<EntityDefinitions.Symbol> symbols);
		long Insert(EntityDefinitions.PerformanceSetup performanceSetup);
        long Insert(EntityDefinitions.PerformanceIdHub performanceIdHub);
    }
}

