using System;
using EntityDefinitions;
using StockPerformanceCalculator.Helpers;

namespace StockPerformanceCalculator.Logic.TradingRules
{
    public class DepositRule
    {
        EntityEngine _entityEngine;
        EntityDefinitions.DepositRule _depositRule;
        public DepositRule(EntityEngine entityEngine)
        {
            _entityEngine = entityEngine;
        }

        public int GetFirstDepositDate()
        {
            var initialSetup = _entityEngine.GetInitialSetup();
           return initialSetup.FirstDepositDate;
        }

        public int GetSecondDepositDate()
        {
            var initialSetup = _entityEngine.GetInitialSetup();

            return initialSetup.SecondDepositDate;
        }

        internal decimal GetDepositAmount()
        {
            var initialSetup = _entityEngine.GetInitialSetup();

            return initialSetup.DepositAmount;
        }
    }
}

