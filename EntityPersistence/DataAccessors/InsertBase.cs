﻿using System;
using EntityDefinitions;
using EntityPersistence.Calculators;
using StockPerformanceCalculator.DatabaseAccessors;
using YahooQuotesApi;

namespace EntityPersistence.DataAccessors
{
    public class InsertBase : IInsert
    {
        IdCalculator _idCalculator;
        DataContext _dataContext;
        public InsertBase(DataContext dataContext)
        {
            _dataContext = dataContext;
            _idCalculator = new IdCalculator(dataContext);
        }
        public void SetId(IIdBase idBase, IEnumerable<long> ids)
        {
            var id = _idCalculator.GetNewId(ids);
            idBase.Id = id;
        }

        public long Insert(DepositRule depositRule)
        {
            SetId(depositRule, _dataContext.DepositRules.Select(a => a.Id));

            _dataContext.DepositRules.Add(depositRule);
            return depositRule.Id;
        }

        public long Insert(PerformanceSummary performance)
        {
            SetId(performance, _dataContext.DepositRules.Select(a => a.Id));

            _dataContext.Performances.Add(performance);
            return performance.Id;
        }

        public List<long> Insert(List<SymbolSummary> symbolSummaries)
        {
            var increment = 0;

            symbolSummaries.ForEach(symbol =>
            {
                increment++;
                var ids = _dataContext.DepositRules.Select(a => a.Id);
                if (_dataContext.DepositRules.Any())
                {
                    var id = _idCalculator.GetNewId(ids);
                    symbol.Id = increment + id;
                }
                else
                {
                    symbol.Id = increment;
                }
            });

            _dataContext.SymbolSummaries.AddRange(symbolSummaries);
            return symbolSummaries.Select(s => s.Id).ToList();
        }

        public List<long> Insert(List<EntityDefinitions.Symbol> symbols)
        {
            var increment = 0;

            foreach (var symbol in symbols)
            {
                increment++;
                var ids = _dataContext.DepositRules.Select(a => a.Id);
                if (_dataContext.DepositRules.Any())
                {
                    var id = _idCalculator.GetNewId(ids);
                    symbol.Id = increment + id;
                }
                else
                {
                    symbol.Id = increment;
                }
            };

            _dataContext.Symbols.AddRange(symbols);
            return symbols.Select(s => s.Id).ToList();
        }

        public long Insert(PerformanceSetup performanceSetup)
        {
            if (_dataContext.DepositRules.Any())
                SetId(performanceSetup, _dataContext.DepositRules.Select(a => a.Id));

            _dataContext.PerformanceSetups.Add(performanceSetup);
            return performanceSetup.Id;
        }

        public long Insert(TradingRule tradingRule)
        {
            if (_dataContext.TradingRules.Any())
                SetId(tradingRule, _dataContext.TradingRules.Select(a => a.Id));

            _dataContext.TradingRules.Add(tradingRule);
            return tradingRule.Id;
        }

        public long Insert(PerformanceIdHub performanceIdHub)
        {
            if (_dataContext.PerformanceIdHubs.Any())
                SetId(performanceIdHub, _dataContext.PerformanceIdHubs.Select(a => a.Id));

            _dataContext.PerformanceIdHubs.Add(performanceIdHub);
            return performanceIdHub.Id;
        }
    }
}

