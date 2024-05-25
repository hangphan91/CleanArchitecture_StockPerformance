using System.Collections.Concurrent;
using EntityDefinitions;
using EntityPersistence.Calculators;
using StockPerformanceCalculator.DatabaseAccessors;

namespace EntityPersistence.DataAccessors
{
    public class InsertBase : IInsert
    {
        static IdCalculator _idCalculator;
        static DataContext _dataContext;
        public InsertBase(DataContext dataContext)
        {
            _dataContext = dataContext;
            _idCalculator = new IdCalculator();
        }
        public void SetId(IIdBase idBase, IEnumerable<long> ids)
        {
            var id = _idCalculator.GetNewId(ids.Max(a => a));
            idBase.Id = id;
        }

        public long Insert(DepositRule depositRule)
        {
            SetId(depositRule, _dataContext.DepositRules.Select(a => a.Id));

            _dataContext.DepositRules.Add(depositRule);
            return depositRule.Id;
        }

        public long Insert(Symbol symbol)
        {
            SetId(symbol, _dataContext.Symbols.Select(a => a.Id));

            _dataContext.Symbols.Add(symbol);
            return symbol.Id;
        }

        public long Insert(PerformanceSummary performance)
        {
            SetId(performance, _dataContext.DepositRules.Select(a => a.Id));

            //_dataContext.PerformanceSummaries.Add(performance);
            return performance.Id;
        }

        public List<long> Insert(List<PerformanceByMonth> performanceByMonths)
        {
            var increment = 0;

            performanceByMonths.ForEach(performance =>
            {
                increment++;
                var ids = _dataContext.DepositRules.Select(a => a.Id);
                if (_dataContext.DepositRules.Any())
                {
                    var id = _idCalculator.GetNewId(ids.Max(a => a));
                    performance.Id = increment + id;
                }
                else
                {
                    performance.Id = increment;
                }
            });

            // _dataContext.PerformanceByMonths.AddRange(performanceByMonths);
            return performanceByMonths.Select(s => s.Id).ToList();
        }

        public List<long> Insert(List<Position> positions)
        {
            var increment = 0;

            positions.ForEach(performance =>
            {
                increment++;
                var ids = _dataContext.Positions.Select(a => a.Id);
                if (_dataContext.Positions.Any())
                {
                    var id = _idCalculator.GetNewId(ids.Max(a => a));
                    performance.Id = increment + id;
                }
                else
                {
                    performance.Id = increment;
                }
            });

            _dataContext.Positions.AddRange(positions);
            return positions.Select(s => s.Id).ToList();
        }

        public List<long> Insert(List<Deposit> deposits)
        {
            var increment = 0;

            deposits.ForEach(performance =>
            {
                increment++;
                var ids = _dataContext.Deposits.Select(a => a.Id);
                if (_dataContext.Deposits.Any())
                {
                    var id = _idCalculator.GetNewId(ids.Max(a => a));
                    performance.Id = increment + id;
                }
                else
                {
                    performance.Id = increment;
                }
            });

            _dataContext.Deposits.AddRange(deposits);
            return deposits.Select(s => s.Id).ToList();
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
                    var id = _idCalculator.GetNewId(ids.Max(a => a));
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
                    var id = _idCalculator.GetNewId(ids.Max(a => a));
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

        public long Insert(DepositRule depositRule, TradingRule tradingRule,
            Symbol symbol, PerformanceSetup setup, string name)
        {
            var depositRuleID = Insert(depositRule);
            var tradingRuleID = Insert(tradingRule);
            var symbolID = Insert(symbol);
            var setupId = Insert(setup);
            var searchDetail = new SearchDetail
            {
                TradingRuleId = tradingRuleID,
                DepositRuleId = depositRuleID,
                SymbolId = symbolID,
                PerformanceSetupId = setupId,
                Name = name
            };

            if (_dataContext.SearchDetails.Any())
                SetId(searchDetail, _dataContext.SearchDetails.Select(a => a.Id));

            _dataContext.SearchDetails.Add(searchDetail);
            return searchDetail.Id;
        }
    }
}

