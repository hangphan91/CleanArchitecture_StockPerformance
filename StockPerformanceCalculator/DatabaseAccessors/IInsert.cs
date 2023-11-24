using EntityDefinitions;

namespace StockPerformanceCalculator.DatabaseAccessors
{
    public interface IInsert
    {
        long Insert(TradingRule tradingRule);
        long Insert(DepositRule depositRule);
        long Insert(PerformanceSummary performance);
        List<long> Insert(List<SymbolSummary> symbolSummaries);
        List<long> Insert(List<Symbol> symbols);
        long Insert(PerformanceSetup performanceSetup);
        long Insert(PerformanceIdHub performanceIdHub);
        public List<long> Insert(List<PerformanceByMonth> performanceByMonths);
        public List<long> Insert(List<Position> positions);
        public List<long> Insert(List<Deposit> positions);
    }
}

