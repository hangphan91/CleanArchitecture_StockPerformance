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
        private static ConcurrentBag<SearchDetail> _savedSearchDetailsSeeting = new ConcurrentBag<SearchDetail>();
        private static ConcurrentBag<SearchDetail> _activeSearchDetailsSeeting = new ConcurrentBag<SearchDetail>();

        private static StockPerformanceManager _stockPerformanceManager;

        public static SearchDetail GetCurrentSearchDetail(IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            _entityDefinitionsAccessor = entityDefinitionsAccessor;
            GetAllSearchDetails(entityDefinitionsAccessor);
            return GetCurrentSearchDetail();
        }

        public static SearchDetail GetInitialSearchDetail()
        {
            if (_initalSearchDetail == null)
            {
                var toView = GetAllSearchDetails(_entityDefinitionsAccessor);
                _initalSearchDetail = toView.First();
            }
            return Map(Map(_initalSearchDetail));
        }

        public static void SetCurrentSearchDetail(SearchDetail searchDetail)
        {
            _searchDetail = searchDetail;
        }

        public static void AddAdvanceSearchDetail(SearchDetail searchDetail)
        {
            _activeSearchDetailsSeeting.Add(searchDetail);
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
                _searchDetail = GetInitialSearchDetail();

                if (_initalSearchDetail == null)
                    _initalSearchDetail = _searchDetail;
            }
            _stockPerformanceManager = performanceMangager;
            _searchDetail.ActiveSelectedSearchDetails = GetAllSearchDetails(_entityDefinitionsAccessor);
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
                ActiveSelectedSearchDetails = _savedSearchDetailsSeeting.Select(a => a).ToList(),
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
            return _activeSearchDetailsSeeting.Select(a => a).ToList();
        }

        internal static List<SearchDetail> GetSearchDetailsForAll()
        {
            var current = _savedSearchDetailsSeeting.FirstOrDefault();

            if (current == null)
                current = GetAllSearchDetails(_entityDefinitionsAccessor).First();

            var starting = current.SearchSetup.StartingYear.Year;
            var ending = _savedSearchDetailsSeeting.First().SearchSetup.EndingYear.Year;
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
                    DepositRule = current.DepositRule,
                    SearchSetup = searchSetup,
                    SettingDate = current.SettingDate,
                    Symbol = SymbolSummary,
                    TradingRule = current.TradingRule,
                    Name = current.Name,
                });
                all.AddRange(eachSearchDetailList);
            }

            return all;

        }

        internal static void ResetInitialSearch()
        {
            _searchDetail = _initalSearchDetail;
        }

        internal static List<SearchDetail> GetAllSearchDetails(IEntityDefinitionsAccessor entityDefinitionsAccessor)
        {
            var list = new List<SearchDetail>();
            var details = entityDefinitionsAccessor.GetSearchDetails();
           
            foreach (var item in details)
            {
                var symbols = entityDefinitionsAccessor.GetSymbolsBetweenIds(item.Item4.StartingSymbolId,
                                                item.Item4.EndingSymbolId);
                SearchDetail detail = GetDetail(item, symbols);
             
                list.Add(detail);
                _savedSearchDetailsSeeting.Add(detail);
            }
            return list;
        }

        private static SearchDetail GetDetail(Tuple<EntityDefinitions.DepositRule,
            EntityDefinitions.TradingRule, EntityDefinitions.Symbol,
            EntityDefinitions.PerformanceSetup, string> item,
            List<string> symbols)
        {
            var a = new DepositRule
            {
                DepositAmount = item.Item1.DepositAmount,
                InitialDepositAmount = item.Item1.InitialDepositAmount,
                FirstDepositDate = item.Item1.FirstDepositDate,
                NumberOfDepositDate = item.Item1.NumberOfDepositDate,
                SecondDepositDate = item.Item1.SecondDepositDate,
            };

            var b = new TradingRule
            {
                NumberOfTradeAMonth = item.Item2.NumberOfTradeAMonth,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = item.Item2.SellAllWhenPriceDropAtPercentageSinceLastTrade,
                BuyPercentageLimitation = item.Item2.BuyPercentageLimitation,
                HigherRangeOfTradingDate = item.Item2.HigherRangeOfTradingDate,
                LossLimitation = item.Item2.LossLimitation,
                LowerRangeOfTradingDate = item.Item2.LowerRangeOfTradingDate,
                SellPercentageLimitation = item.Item2.SellPercentageLimitation,
                PurchaseLimitation = item.Item2.PurchaseLimitation,
            };
            var d = new SettingDate
            {
                Day = item.Item4.StartingYear.Day,
                Month = item.Item4.StartingYear.Month,
                Year = item.Item4.StartingYear.Year,
            };
            var c = new SearchInitialSetup
            {
                StartingYear = item.Item4.StartingYear,
                EndingYear = item.Item4.EndingYear,
                Symbols = symbols
            };
            return new SearchDetail
            {
                DepositRule = a,
                TradingRule = b,
                SearchSetup = c,
                Symbol = item.Item3.TradingSymbol,
                SettingDate = d,
                Name = item.Item5
            };
        }

        internal static void ClearSelectedAllSearches()
        {
            _activeSearchDetailsSeeting.Clear();
        }
    }
}

