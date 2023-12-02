using StockPerformanceCalculator.DatabaseAccessors;

namespace StockPerformanceCalculator.Helpers
{
    public  static class StockPerformanceManagerHelper
	{
        private static EntityDefinitions.TradingRule _tradingRule;
        private static EntityDefinitions.DepositRule _depositRule;
        public static EntityDefinitions.PerformanceSetup _performanceSetup;

        public static EntityDefinitions.TradingRule GetTradingRuleInstance(
            IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            if (_tradingRule == null|| _tradingRule.BuyPercentageLimitation == 0)
                _tradingRule = entityDefinitionsAccessor.GetTradingRule();

            return _tradingRule;
        }

        public static EntityDefinitions.DepositRule GetDepositRuleInstance(
            IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            if (_depositRule == null || _depositRule.DepositAmount == 0)
                _depositRule = entityDefinitionsAccessor.GetDepositRule();

            return _depositRule;
        }

        public static EntityDefinitions.PerformanceSetup GetPerformanceSetupInstance(
            IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            if (_performanceSetup == null)
                _performanceSetup = entityDefinitionsAccessor.GetPerformanceSetup();

            return _performanceSetup;
        }

        public static void SetTradingRule(EntityDefinitions.TradingRule tradingRule)
        {
            _tradingRule = tradingRule;
        }

        public static void SetDepositRule(EntityDefinitions.DepositRule depositRule)
        {
            _depositRule = depositRule;
        }

        public static void SetPerformanceSetup(EntityDefinitions.PerformanceSetup performanceSetup)
        {
            _performanceSetup = performanceSetup;
        }

        internal static List<string> GetSymbols(
            IEntityDefinitionsAccessor entityDefinitionsAccessor,
            EntityDefinitions.PerformanceSetup performanceSetup)
        {
            return entityDefinitionsAccessor
                           .GetSymbolsBetweenIds(performanceSetup.StartingSymbolId,
                           performanceSetup.EndingSymbolId);
        }
    }
}

