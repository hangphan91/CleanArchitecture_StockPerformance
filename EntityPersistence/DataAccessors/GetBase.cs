using EntityDefinitions;
using StockPerformanceCalculator.DatabaseAccessors;

namespace EntityPersistence.DataAccessors
{
    public class GetBase : IGet
    {
        DataContext _dataContext;
        public GetBase(DataContext dataContext)
        {
            if(dataContext == null)
                _dataContext = new  DataContext();
            else
                _dataContext = dataContext;
        }

        public DepositRule GetDepositRule()
        {
            return _dataContext.DepositRules.First();
        }
        public DepositRule GetDepositRule(long id)
        {
            return _dataContext.DepositRules.Find(x => x.Id == id);
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

        public PerformanceSetup GetPerformanceSetup(long id)
        {
            return _dataContext.PerformanceSetups.First(x => x.Id == id);
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


        public Symbol GetSymbol(long id)
        {
            return _dataContext.Symbols
                .Where(summary => summary.Id == id).First();
        }

        public TradingRule GetTradingRule()
        {
            return _dataContext.TradingRules.First();
        }


        public TradingRule GetTradingRule(long id)
        {
            return _dataContext.TradingRules.Find(x => x.Id == id);
        }

        public List<string> GetSymbolsBetweenIds(long startingId, long endingId)
        {
            return _dataContext.Symbols
                .Where(symbol => symbol.Id >= startingId && symbol.Id <= endingId)
                .Select(symbol => symbol.TradingSymbol)
                .ToList();
        }

        public List<string> GetSavedSymbols(List<string> symbols)
        {
            return _dataContext.Symbols
                .Where(s => symbols.Contains(s.TradingSymbol))
                .Select(s => s.TradingSymbol)
                .ToList();
        }
        public List<string> GetAllSavedSymbols()
        {
            return _dataContext.Symbols
                .Select(s => s.TradingSymbol)
                .ToList();
        }

        public List<Email> GetEmails()
        {
            return _dataContext.Emails;
        }

        public List<Tuple<DepositRule,
                    TradingRule,
                    Symbol,
                    PerformanceSetup, string>> GetSearchDetails()
        {
            var list = new List<Tuple<DepositRule,
                    TradingRule,
                    Symbol,
                    PerformanceSetup,
                    string>>();
            var details = _dataContext.SearchDetails;

            foreach (var item in details)
            {
                var depositRule = GetDepositRule(item.DepositRuleId);
                var tradingRule = GetTradingRule(item.TradingRuleId);
                var symbol = GetSymbol(item.SymbolId);
                var setup = GetPerformanceSetup(item.PerformanceSetupId);
                var table = new Tuple<DepositRule, TradingRule, Symbol, PerformanceSetup, string>
                    (depositRule, tradingRule, symbol, setup, item.Name);
                list.Add(table);
            }
            return list;
        }
    }
}

