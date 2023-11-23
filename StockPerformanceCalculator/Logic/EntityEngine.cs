using StockPerformanceCalculator.Helpers;
using StockPerformanceCalculator.Logic.Mappers;
using StockPerformanceCalculator.Models.PerformanceCalculatorSetup;

namespace StockPerformanceCalculator.Logic
{
    public class EntityEngine
    {
        DatabaseAccessors.IEntityDefinitionsAccessor _entityDefinitionsAccessor;

        public EntityEngine(DatabaseAccessors.IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _entityDefinitionsAccessor = entityDefinitionsAccessor;
        }
         

        internal long AddPerformanceSummary(EntityDefinitions.PerformanceSummary performance)
        {
            // ToDO add performance details
           
            //
            return _entityDefinitionsAccessor.Insert(performance);

        }

        internal void AddSymbolSummaries(List<EntityDefinitions.SymbolSummary> mappedResult)
        {
            _entityDefinitionsAccessor.Insert(mappedResult);
        }

       

        internal InitialPerformanceSetup GetInitialSetup()
        {
            var performanceSetup = StockPerformanceManagerHelper
                .GetPerformanceSetupInstance(_entityDefinitionsAccessor);
           var symbols = StockPerformanceManagerHelper
                .GetSymbols(_entityDefinitionsAccessor, performanceSetup);
            var depositRule = StockPerformanceManagerHelper
                .GetDepositRuleInstance(_entityDefinitionsAccessor);
            var tradingRule = StockPerformanceManagerHelper
                .GetTradingRuleInstance(_entityDefinitionsAccessor);
            return SetupMapper.Map(performanceSetup, symbols, depositRule, tradingRule);
        }

        internal long AddTradingRule(EntityDefinitions.TradingRule newTradingRule)
        {
            return _entityDefinitionsAccessor.Insert(newTradingRule);
        }

        internal long AddDepositRule(EntityDefinitions.DepositRule newDepositRule)
        {
            return _entityDefinitionsAccessor.Insert(newDepositRule);
        }

        internal long GetSymbolId(string symbol)
        {
            return _entityDefinitionsAccessor.GetSymbolId(symbol);
        }

        internal List<long> GetSymbolIds(List<string> symbols)
        {
            return _entityDefinitionsAccessor.GetSymbolIds(symbols);
        }

        internal long AddPerformanceSetup(EntityDefinitions.PerformanceSetup performanceSetup)
        {
            return _entityDefinitionsAccessor.Insert(performanceSetup);
        }

        internal void AddPerformanceIdHub(EntityDefinitions.PerformanceIdHub performanceIdHub)
        {
            _entityDefinitionsAccessor.Insert(performanceIdHub);
        }
    }
}

