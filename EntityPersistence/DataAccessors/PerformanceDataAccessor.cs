using EntityDefinitions;
using StockPerformanceCalculator.DatabaseAccessors;

namespace EntityPersistence.DataAccessors
{
    public class PerformanceDataAccessor : IEntityDefinitionsAccessor
    {
        GetBase _getBase;
        InsertBase _insertBase;

        public PerformanceDataAccessor()
        {
            var dataContext = new DataContext();
            _getBase = new GetBase(dataContext);
            _insertBase = new InsertBase(dataContext);
        }

        public DepositRule GetDepositRule()
        {
            return _getBase.GetDepositRule();
        }

        public List<PerformanceSummary> GetPerformancesBySymbol(string symbol)
        {
            return _getBase.GetPerformancesBySymbol(symbol);
        }

        public PerformanceSetup GetPerformanceSetup()
        {
            return _getBase.GetPerformanceSetup();
        }

        public List<string> GetSavedSymbols(List<string> symbols)
        {
            return _getBase.GetSavedSymbols(symbols);
        }

        public List<string> GetAllSavedSymbols()
        {
            return _getBase.GetAllSavedSymbols();
        }


        public long GetSymbolId(string symbol)
        {
            return _getBase.GetSymbolId(symbol);
        }

        public List<long> GetSymbolIds(List<string> symbols)
        {
            return _getBase.GetSymbolIds(symbols);
        }

        public List<string> GetSymbolsBetweenIds(long startingId, long endingId)
        {
            return _getBase.GetSymbolsBetweenIds(startingId, endingId);
        }

        public TradingRule GetTradingRule()
        {
            return _getBase.GetTradingRule();
        }

        public List<Email> GetEmails()
        {
            return _getBase.GetEmails();
        }

        public long Insert(TradingRule tradingRule)
        {
            return _insertBase.Insert(tradingRule);
        }

        public long Insert(DepositRule depositRule)
        {
            return _insertBase.Insert(depositRule);
        }

        public long Insert(PerformanceSummary performance)
        {
            return _insertBase.Insert(performance);
        }

        public List<long> Insert(List<SymbolSummary> symbolSummaries)
        {
            return _insertBase.Insert(symbolSummaries);
        }

        public List<long> Insert(List<Symbol> symbols)
        {
            return _insertBase.Insert(symbols);
        }

        public long Insert(PerformanceSetup performanceSetup)
        {
            return _insertBase.Insert(performanceSetup);
        }

        public long Insert(PerformanceIdHub performanceIdHub)
        {
            return _insertBase.Insert(performanceIdHub);
        }

        public List<long> Insert(List<PerformanceByMonth> performanceByMonths)
        {
            return _insertBase.Insert(performanceByMonths);
        }

        public List<long> Insert(List<Position> positions)
        {
            return _insertBase.Insert(positions);
        }

        public List<long> Insert(List<Deposit> deposits)
        {
            return _insertBase.Insert(deposits);
        }

        public List<Tuple<DepositRule,
                    TradingRule,
                    Symbol,
                    PerformanceSetup, string>> GetSearchDetails()
        {
            return _getBase.GetSearchDetails();
        }

        public long Insert(DepositRule depositRule, TradingRule tradingRule,
            Symbol symbol, PerformanceSetup setup, string name)
        {
            return _insertBase.Insert(depositRule, tradingRule, symbol, setup, name);
        }
    }
}

