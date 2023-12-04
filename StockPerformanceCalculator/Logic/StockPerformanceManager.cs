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
        protected AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        protected DepositLedgerCalculator _depositLedgerCalculator;
        private TradeDetailCalculator _tradeDetailCalculator;
        protected StockPerformanceSummaryCalculator _stockPerformanceSummaryCalculator;
        protected PriceCalculator _priceCalculator;
        protected BalanceHoldingCalculator _holdingPositionCalculator;
        protected decimal _totalBalance = 0;
        protected IYahooFinanceCaller _yahooFinanceCaller;
        protected GrowthRateCalculator _growthRateCalculator;
        protected EntityEngine _entityEngine;
        protected TradingRule _tradingRule;
        private BalanceHoldingCalculator _balanceHoldingCalculator;
        private DepositRule _depositRule;


        private string _symbol;
        private DateTime _startingDate;

        public StockPerformanceManager(string symbol, DateDetail startDate, IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _startingDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            _entityEngine = new EntityEngine(entityDefinitionsAccessor);
            _depositRule = new DepositRule(_entityEngine);
            _symbol = symbol;
            _yahooFinanceCaller = new YahooFinanceCaller(_entityEngine);
            _depositLedgerCalculator = new DepositLedgerCalculator(_depositRule);
            _stockLedgerCalculator = new StockLedgerCalculator();
            _shareNumberCalculator = new ShareNumberCalculator(_stockLedgerCalculator);
            _balanceHoldingCalculator = new BalanceHoldingCalculator(_depositLedgerCalculator);
            _availableBalanceCalculator = new AvailableBalanceCalculator
                (_depositLedgerCalculator, _balanceHoldingCalculator);
            _tradingRule = new TradingRule(_entityEngine);
            _tradeCalculator = new TradeCalculator(_stockLedgerCalculator,
                _availableBalanceCalculator, _shareNumberCalculator, _tradingRule);
            _priceCalculator = new PriceCalculator(_yahooFinanceCaller);
            _tradeDetailCalculator = new TradeDetailCalculator(_stockLedgerCalculator,
                _priceCalculator, _tradingRule, startDate, _depositRule );
            _stockPerformanceSummaryCalculator =
                new StockPerformanceSummaryCalculator
                (symbol, startDate, _priceCalculator, _stockLedgerCalculator,
                _depositLedgerCalculator, _availableBalanceCalculator);
            _growthRateCalculator = new GrowthRateCalculator(_depositLedgerCalculator);
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
            _depositLedgerCalculator.SetUpDepositLeggerFromDate(_startingDate);
            ImplementTradingStocks(stockSummaries);

            var result = CalculateStockPerformance();
            var performanceInMonths = StockPerformanceSummaryMapper.Map(result.ProfitByMonths);
            var allDeposit = StockPerformanceSummaryMapper.Map(result.DepositLedgers);
            var allPositions = StockPerformanceSummaryMapper.Map(result.StockLedger);

            SavePerformanceRecord(result, newPerformanceSetup,
                newTradingRule, newDepositRule, performanceInMonths,
                allPositions, allDeposit);

            return result;
        }

        private void SavePerformanceRecord(StockPerformanceSummary result,
            EntityDefinitions.PerformanceSetup newPerformanceSetup,
            EntityDefinitions.TradingRule newTradingRule,
            EntityDefinitions.DepositRule newDepositRule,
            List<EntityDefinitions.PerformanceByMonth> performanceByMonths,
            List<EntityDefinitions.Position> positions,
            List<EntityDefinitions.Deposit> deposits)
        {
            var symbolId = _entityEngine.GetSymbolId(result.Symbol);
            var performanceSetupId = _entityEngine.AddPerformanceSetup(newPerformanceSetup);
            var tradingRuleId = _entityEngine.AddTradingRule(newTradingRule);
            var mappedPerformance = StockPerformanceSummaryMapper.Map(result);

            var performanceId = _entityEngine
                .AddPerformanceSummary(mappedPerformance, performanceByMonths,
                deposits, positions);
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
            _availableBalanceCalculator.Calculate();

            var totalDeposit = _availableBalanceCalculator.GetTotalDeposit();
            var totalProfit = summary.ProfitByYears.Sum(profit => profit.Amount);
            summary.TotalBalanceAfterLoss = totalDeposit + totalProfit;
            summary.TotalDeposit = totalDeposit;

            var currentHoldingShare = _stockLedgerCalculator.GetTotalShareHoldingLedgers();
            var currentPrice = _priceCalculator.GetCurrentPrice();
            summary.TotalBalanceHoldingInPosition = currentHoldingShare * currentPrice;
            summary.CurrentHoldingShare = currentHoldingShare;

            var hasFreeCash = summary.TotalBalanceAfterLoss > summary.TotalBalanceHoldingInPosition;

            summary.ProfitInDollar = totalProfit;

            summary.ProfitInPercentage = totalDeposit != 0 ? summary.ProfitInDollar * 100 / totalDeposit :0;
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

