using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.ExternalCommunications;
using StockPerformanceCalculator.Helpers;
using StockPerformanceCalculator.Logic.Calculators;
using StockPerformanceCalculator.Logic.Mappers;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;
using StockPerformanceCalculator.Models.PerformanceCalculatorSetup;

namespace StockPerformanceCalculator.Logic
{
    public class StockPerformanceManager
    {
        protected StockLedgerCalculator _stockLedgerCalculator;
        private TradeCalculator _tradeCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        protected DepositLedgerCalculator _depositLedgerCalculator;
        private TradeDetailCalculator _tradeDetailCalculator;
        protected StockPerformanceSummaryCalculator _stockPerformanceSummaryCalculator;
        protected PriceCalculator _priceCalculator;
        protected HoldingPositionCalculator _holdingPositionCalculator;
        protected decimal _totalBalance = 0;
        protected IYahooFinanceCaller _yahooFinanceCaller;
        protected GrowthRateCalculator _growthRateCalculator;
        protected EntityEngine _entityEngine;
        protected TradingRule _tradingRule;

        private string _symbol;
        private DateTime _startingDate;

        public StockPerformanceManager(string symbol, int year, IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            var now = DateTime.Now;
            _startingDate = new DateTime(year, now.Month, now.Day);
            _entityEngine = new EntityEngine(entityDefinitionsAccessor);

            _symbol = symbol;
            _yahooFinanceCaller = new YahooFinanceCaller(_entityEngine);
            _depositLedgerCalculator = new DepositLedgerCalculator(_startingDate, _entityEngine);
            _stockLedgerCalculator = new StockLedgerCalculator();
            _shareNumberCalculator = new ShareNumberCalculator(_stockLedgerCalculator);
            _availableBalanceCalculator = new AvailableBalanceCalculator
                (_depositLedgerCalculator, _stockLedgerCalculator);
            _tradingRule = new TradingRule(_entityEngine);
            _tradeCalculator = new TradeCalculator(_stockLedgerCalculator,
                _availableBalanceCalculator, _shareNumberCalculator, _tradingRule);
            _priceCalculator = new PriceCalculator(_yahooFinanceCaller);
            _tradeDetailCalculator = new TradeDetailCalculator(_stockLedgerCalculator, _priceCalculator, _tradingRule, year);
            _stockPerformanceSummaryCalculator =
                new StockPerformanceSummaryCalculator
                (symbol, year, _priceCalculator, _stockLedgerCalculator, _depositLedgerCalculator, _availableBalanceCalculator);
            _holdingPositionCalculator = new HoldingPositionCalculator(_stockLedgerCalculator);
            _growthRateCalculator = new GrowthRateCalculator(_depositLedgerCalculator);
        }

        public StockPerformanceManager(IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _entityEngine = new EntityEngine(entityDefinitionsAccessor);
        }

        public async Task<StockPerformanceSummary> StartStockPerforamanceCalculation(InitialPerformanceSetup mapped)
        {
            var stockSummaries = await _yahooFinanceCaller.GetStockHistory(_symbol, _startingDate);
            var newTradingRule = SearchDetailMapper.MapTradingRule(mapped);
            var newDepositRule = SearchDetailMapper.MapDepositRule(mapped);
            var symbolIds = _entityEngine.GetSymbolIds(mapped.Symbols);
            var newPerformanceSetup = SearchDetailMapper.MapPerformanceSetup(mapped, symbolIds);

            StockPerformanceManagerHelper.SetDepositRule(newDepositRule);
            StockPerformanceManagerHelper.SetTradingRule(newTradingRule);
            StockPerformanceManagerHelper.SetPerformanceSetup(newPerformanceSetup);

            ImplementTradingStocks(stockSummaries);

            var result = CalculateStockPerformance();
            SavePerformanceRecord(result, newPerformanceSetup, newTradingRule, newDepositRule);
            return result;
        }

        private void SavePerformanceRecord(StockPerformanceSummary result,
            EntityDefinitions.PerformanceSetup newPerformanceSetup,
            EntityDefinitions.TradingRule newTradingRule,
            EntityDefinitions.DepositRule newDepositRule)
        {
            var symbolId = _entityEngine.GetSymbolId(result.Symbol);
            var performanceSetupId = _entityEngine.AddPerformanceSetup(newPerformanceSetup);
            var tradingRuleId = _entityEngine.AddTradingRule(newTradingRule);
            var mappedPerformance = StockPerformanceSummaryMapper.Map(result);
            var performanceId = _entityEngine.AddPerformanceSummary(mappedPerformance);
            var depositRuleId = _entityEngine.AddDepositRule(newDepositRule);

            var performanceIdHub = new EntityDefinitions.PerformanceIdHub
            {
                DepositRuleId = depositRuleId,
                PerformanceId = performanceId,
                PerformanceSetupId = performanceSetupId,
                SymbolId = symbolId,
                TradingRuleId = tradingRuleId
            };
            _entityEngine.AddPerformanceIdHub(performanceIdHub);
        }

        private StockPerformanceSummary CalculateStockPerformance()
        {
            var summary = _stockPerformanceSummaryCalculator.Calculate();

            var totalDeposit = _availableBalanceCalculator.GetTotalDeposit();
            var totalProfit = summary.ProfitByYears.Sum(profit => profit.Amount);
            summary.TotalBalanceAfterLoss = totalDeposit + totalProfit;
            summary.TotalDeposit = totalDeposit;

            var currentHoldingShare = _stockLedgerCalculator.GetTotalShareHoldingLedgers();
            var currentPrice = _priceCalculator.GetCurrentPrice();
            summary.TotalBalanceHoldingInPosition = currentHoldingShare * currentPrice;
            summary.CurrentHoldingShare = currentHoldingShare;
            summary.ProfitInDollar = currentHoldingShare * currentPrice - totalDeposit;
            summary.ProfitInPercentage = summary.ProfitInDollar * 100 / totalDeposit;
            _holdingPositionCalculator.Calculate(summary);
            return summary;
        }

        private void ImplementTradingStocks(List<SymbolSummary> stockSummaries)
        {
            var tradeDetails = _tradeDetailCalculator.GetTradeDetails(stockSummaries);

            tradeDetails.ForEach(_tradeCalculator.ImplementTrade);
        }

        public InitialPerformanceSetup GetInitialSetup()
        {
            return _entityEngine.GetInitialSetup();
        }
    }
}

