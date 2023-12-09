using System;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformance_CleanArchitecture.Models.Settings;
using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.Logic;
using StockPerformanceCalculator.Models;
using StockPerformanceCalculator.Models.PerformanceCalculatorSetup;

namespace StockPerformance_CleanArchitecture.Helpers
{
    public static class SearchDetailHelper
    {
        private static string _symbol = "AAPL";
        private static DateDetail _startDate = new DateDetail();
        private static SearchDetail _searchDetail;
        private static SearchInitialSetup _searchInitialSetup;
        private static IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        private static List<SearchDetail> _searchDetails = new List<SearchDetail>();
        private static StockPerformanceManager _stockPerformanceManager;

        public static SearchDetail GetCurrentSearchDetail(IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _entityDefinitionsAccessor = entityDefinitionsAccessor;
            return GetCurrentSearchDetail();
        }
        public static void SetCurrentSearchDetail(SearchDetail searchDetail)
        {
            _searchDetail = searchDetail;
        }

        public static void AddAdvanceSearchDetail(SearchDetail searchDetail)
        {
            _searchDetails.Add(searchDetail);
        }

        public static SearchInitialSetup GetSearchInitialSetup()
        {
            return _searchInitialSetup;
        }

        public static StockPerformanceManager GetStockPerformanceManager(DateDetail startDate, string symbol)
        {
            _stockPerformanceManager = new StockPerformanceManager(
                symbol, startDate, _entityDefinitionsAccessor);

            return _stockPerformanceManager;
        }

        private static SearchDetail GetCurrentSearchDetail()
        {
            var performanceMangager = GetStockPerformanceManager(_startDate, _symbol);

            if (_searchDetail == null)
            {
                var searchInitialSetup = performanceMangager.GetInitialSetup();
                SearchDetail searchDetail = Map(searchInitialSetup);
                _searchDetail = searchDetail;
            }
            _stockPerformanceManager = performanceMangager;

            return _searchDetail;
        }

        private static SearchDetail Map(InitialPerformanceSetup searchInitialSetup)
        {
            var mappedInitialSetup = new SearchInitialSetup
            {
                EndingYear = searchInitialSetup.EndingYear,
                StartingYear = searchInitialSetup.StartingYear,
                Symbols = searchInitialSetup.Symbols.OrderBy(a => a).ToList(),
            };
            var initialDepositRule = new DepositRule
            {
                DepositAmount = searchInitialSetup.DepositAmount,
                FirstDepositDate = searchInitialSetup.FirstDepositDate,
                SecondDepositDate = searchInitialSetup.SecondDepositDate,
                NumberOfDepositDate = searchInitialSetup.NumberOfDepositDate,
                InitialDepositAmount = searchInitialSetup.InitialDepositAmount,
            };
            var initialTradingRule = new TradingRule
            {
                BuyPercentageLimitation = searchInitialSetup.BuyPercentageLimitation,
                SellPercentageLimitation = searchInitialSetup.SellPercentageLimitation,
                HigherRangeOfTradingDate = searchInitialSetup.HigherRangeOfTradingDate,
                LowerRangeOfTradingDate = searchInitialSetup.LowerRangeOfTradingDate,
                PurchaseLimitation = searchInitialSetup.PurchaseLimitation,
                LossLimitation = searchInitialSetup.LossLimitation,
                NumberOfTradeAMonth = searchInitialSetup.NumberOfTradeAMonth,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = searchInitialSetup.SellAllWhenPriceDropAtPercentageSinceLastTrade,

            };
            var searchDetail = new SearchDetail
            {
                SearchSetup = mappedInitialSetup,
                DepositRule = initialDepositRule,
                TradingRule = initialTradingRule,
            };
            _searchInitialSetup = mappedInitialSetup;
            return searchDetail;
        }

        public static InitialPerformanceSetup Map(SearchDetail searchInitialSetup)
        {
            var depositRule = searchInitialSetup.DepositRule;
            var tradingrule = searchInitialSetup.TradingRule;
            var performanceSetup = searchInitialSetup.SearchSetup;
            return new InitialPerformanceSetup
            {
                DepositAmount = depositRule.DepositAmount,
                FirstDepositDate = depositRule.FirstDepositDate,
                NumberOfDepositDate = depositRule.NumberOfDepositDate,
                SecondDepositDate = depositRule.SecondDepositDate,
                BuyPercentageLimitation = tradingrule.BuyPercentageLimitation,
                SellPercentageLimitation = tradingrule.SellPercentageLimitation,
                HigherRangeOfTradingDate = tradingrule.HigherRangeOfTradingDate,
                LowerRangeOfTradingDate = tradingrule.LowerRangeOfTradingDate,
                PurchaseLimitation = tradingrule.PurchaseLimitation,
                StartingYear = performanceSetup.StartingYear,
                EndingYear = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Symbols = performanceSetup.Symbols,
                LossLimitation = tradingrule.LossLimitation,
                InitialDepositAmount = depositRule.InitialDepositAmount,
                NumberOfTradeAMonth = tradingrule.NumberOfTradeAMonth,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = tradingrule.SellAllWhenPriceDropAtPercentageSinceLastTrade,
            };
        }

        internal static List<SearchDetail> GetSearchDetails()
        {
            return _searchDetails;
        }

        internal static void ClearSearchDetails()
        {
            _searchDetails.Clear();
        }
    }
}

