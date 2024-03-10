using System;
using System.Collections.Concurrent;
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
        private static SearchDetail _initalSearchDetail;
        private static SearchInitialSetup _searchInitialSetup;
        private static SearchInitialSetup _searchSetup;
        private static IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        private static ConcurrentBag<SearchDetail> _searchDetails = new ConcurrentBag<SearchDetail>();
        private static StockPerformanceManager _stockPerformanceManager;

        public static SearchDetail GetCurrentSearchDetail(IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _entityDefinitionsAccessor = entityDefinitionsAccessor;
            return GetCurrentSearchDetail();
        }

        public static SearchDetail GetInitialSearchDetail()
        {
            return Map(Map(_initalSearchDetail));
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

        public static SearchInitialSetup GetCurrentSearchSetup()
        {
            return _searchSetup;
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

                if (_initalSearchDetail == null)
                    _initalSearchDetail = _searchDetail;
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
                Name = "Default Set up",
                SettingDate = new SettingDate
                (mappedInitialSetup.StartingYear.Year,
                mappedInitialSetup.StartingYear.Month,
                mappedInitialSetup.StartingYear.Day),
                SearchDetails = _searchDetails.Select(a=> a).ToList(),
            };
            if (_searchInitialSetup == null)
                _searchInitialSetup = mappedInitialSetup;
            _searchSetup = mappedInitialSetup;
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
            return _searchDetails.Select(a => a).ToList();
        }

        internal static List<SearchDetail> GetSearchDetailsForAll()
        {
            var current = _searchDetails.FirstOrDefault();

            if (current == null)
                return new List<SearchDetail>();

            var starting = current.SearchSetup.StartingYear.Year;
            var ending = _searchDetails.First().SearchSetup.EndingYear.Year;
            var all = new List<SearchDetail>();
            for (int i = starting; i < ending; i++)
            {
                var searchSetup = new SearchInitialSetup
                {
                    StartingYear = new DateOnly(i, current.SearchSetup.StartingYear.Month,
                    current.SearchSetup.StartingYear.Day),
                    EndingYear = current.SearchSetup.EndingYear,
                    Symbols = current.SearchSetup.Symbols,
                };
                var eachSearchDetailList = _searchSetup.Symbols.Select(SymbolSummary => new SearchDetail
                {
                    DepositRule =  current.DepositRule,
                    SearchSetup = searchSetup,
                    SettingDate = current.SettingDate,
                    Symbol =  SymbolSummary,
                    TradingRule = current.TradingRule,
                    Name = current.Name,
                });
                all.AddRange(eachSearchDetailList);
            }

            return all;

        }

        internal static void ClearSearchDetails()
        {
            _searchDetails.Clear();
        }
    }
}

