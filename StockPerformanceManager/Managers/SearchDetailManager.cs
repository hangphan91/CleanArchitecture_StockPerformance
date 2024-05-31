using DocumentFormat.OpenXml.Drawing.Diagrams;
using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformance_CleanArchitecture.Models.Settings;
using StockPerformance_CleanArchitecture.Models.Storages;
using StockPerformanceCalculator.DatabaseAccessors;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SearchDetailManager
    {
        IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        private bool IsFirstAdvancedSearch = true;
        SendEmailTimer _timer;
        ReportTimer _reportTimer;
        public SearchDetailManager()
        {
            _entityDefinitionsAccessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            //_timer = new SendEmailTimer();
            _reportTimer = new ReportTimer();
        }

        public SearchDetail GetCurrentSearchDetail()
        {
            return SearchDetailHelper.GetCurrentSearchDetail(_entityDefinitionsAccessor);
        }

        public SearchDetail GetInitialSearchDetail()
        {
            return SearchDetailHelper.GetInitialSearchDetail();
        }

        private SearchInitialSetup GetCurrentSearchSetup()
        {
            return SearchDetailHelper.GetCurrentSearchSetup();
        }

        public SearchInitialSetup GetInitialSearchSetup()
        {
            return SearchDetailHelper.GetSearchInitialSetup();
        }

        public void SetCurrentSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetCurrentSearchSetup();
            SearchDetailHelper.SetCurrentSearchDetail(searchDetail);
        }

        public void AddAdvanceSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetCurrentSearchSetup();
            SearchDetailHelper.AddAdvanceSearchDetail(searchDetail);
        }

        public AdvanceSearch AddAdvanceSearch(AdvanceSearch advanceSearch,
            bool willClearAllSearch)
        {
            string name = advanceSearch.SearchDetail.Name;

            var toView = GetAllSavedSearchDetails();
            var match = toView.FirstOrDefault(a =>
            {
                return a.Name.Equals(name);
            });
            if (!string.IsNullOrWhiteSpace(name) && match != null)
            {
                match.SavedSearchDetails = toView;
                advanceSearch.SearchDetail = match;
                return advanceSearch;
            }

            if (willClearAllSearch)
            {
                ClearAdvanceSearch();
                advanceSearch.SearchDetail.SavedSearchDetails = toView;
            }

            var searchDetails = GetActiveSearchDetails();
            var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            var detail = SearchDetailHelper.GetCurrentSearchDetail(accessor);

            if (willClearAllSearch || advanceSearch.StartDate.Day == 0)
            {
                advanceSearch.SearchDetail = detail;
                return advanceSearch;
            }

            foreach (var symbol in advanceSearch.Symbols)
            {

                var month = advanceSearch.StartDate.Month;
                var day = advanceSearch.StartDate.Day;
                advanceSearch.SearchDetail.SettingDate = advanceSearch.StartDate;
                var searchDetail = new SearchDetail
                {
                    DepositRule = advanceSearch.SearchDetail.DepositRule,
                    SearchSetup = advanceSearch.SearchDetail.SearchSetup,
                    Symbol = symbol,
                    TradingRule = advanceSearch.SearchDetail.TradingRule,
                    SettingDate = advanceSearch.SearchDetail.SettingDate,
                    ActiveSelectedSearchDetails = GetActiveSearchDetails(),
                    SavedSearchDetails = toView,
                };
                AddAdvanceSearchDetail(searchDetail);
            }

            advanceSearch.Symbols = _entityDefinitionsAccessor.GetAllSavedSymbols();
            advanceSearch.SearchDetail.SearchSetup.Symbols = advanceSearch.Symbols;
            advanceSearch.Count = searchDetails.Count();
            advanceSearch.SearchDetail.SearchSetup.StartingYear = advanceSearch.StartDate.MapDateOnly();
            advanceSearch.SearchDetail.SearchSetup.EndingYear = advanceSearch.EndDate.MapDateOnly();
            advanceSearch.SearchDetail.ActiveSelectedSearchDetails = GetActiveSearchDetails();
            advanceSearch.SearchDetail.SavedSearchDetails = toView;


            var result = new AdvanceSearch
            {
                Count = searchDetails.Count(),
                SearchDetail = advanceSearch.SearchDetail,
                Symbols = advanceSearch.SearchDetail.SavedSearchDetails.Select(a => a.Symbol).ToList(),
                StartDate = advanceSearch.StartDate,
                EndDate = advanceSearch.EndDate,
                WillPerformSearch = advanceSearch.WillPerformSearch,
            };

            return result;
        }

        public SearchInitialSetup AddSymbol(SearchInitialSetup searchSetup)
        {
            var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            var detail = SearchDetailHelper.GetCurrentSearchDetail(accessor);

            if (!string.IsNullOrWhiteSpace(searchSetup?.AddSymbol))
            {
                searchSetup.AddingSymbols.Add(searchSetup.AddSymbol);
                AddNewSymbols(searchSetup, detail.SearchSetup.Symbols);
                searchSetup.StartingYear = detail.SearchSetup.StartingYear;
                searchSetup.EndingYear = detail.SearchSetup.EndingYear;
                detail.SearchSetup = searchSetup;
            }
            else
                searchSetup = detail.SearchSetup;
            SearchDetailHelper.SetCurrentSearchDetail(detail);
            return searchSetup;
        }

        public async Task<StockPerformanceResponse> GetStockPerformanceResponse(
                    SearchDetail searchDetail)
        {
            if (searchDetail.SaveCurrentSetting)
                SaveSearchDetail(searchDetail);

            var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
            if (cachedResponse != null)
                return cachedResponse;

            var currentSearchDetail = GetCurrentSearchDetail();
            var allSearchDetails = GetAllSavedSearchDetails();
            currentSearchDetail.ActiveSelectedSearchDetails = allSearchDetails;

            if (searchDetail == null || string.IsNullOrWhiteSpace(searchDetail.Symbol))
                searchDetail = currentSearchDetail;
            SetCurrentSearchDetail(searchDetail);

            var symbol = searchDetail.Symbol ?? "AAPL";
            var startingDate = searchDetail.SettingDate.Year == 0 ? new SettingDate() : searchDetail.SettingDate;
            var response = new StockPerformanceResponse(symbol, startingDate.Map());
            SetCurrentSearchDetail(searchDetail);

            var performanceMangager = SearchDetailHelper.GetStockPerformanceManager(startingDate.Map(), symbol);

            var mapped = SearchDetailHelper.Map(searchDetail);
            var summary = await performanceMangager.StartStockPerforamanceCalculation(mapped);

            response = response.Map(summary);
            response.SymbolSummaries = performanceMangager.GetSymbolSummaries();
            response.ProfitChart = new ProfitChart(response);

            response.SearchDetail = searchDetail;

            CachedHelper.AddCache(response);
            //  _timer.AddResponse(response, _entityDefinitionsAccessor.GetEmails());
            return response;
        }

        public void AddNewSymbols(SearchInitialSetup searchSetup, List<string> symbols)
        {
            var addSymbols = searchSetup.AddingSymbols;
            var savedSymbols = _entityDefinitionsAccessor.GetSavedSymbols(addSymbols);
            var toAdd = addSymbols
                .Where(a => !savedSymbols.Contains(a));
            var symbolsToInsert = toAdd
                .Select(a => new EntityDefinitions.Symbol
                {
                    TradingSymbol = a.ToUpper(),
                })
                .ToList();
            symbols.AddRange(toAdd);
            _entityDefinitionsAccessor.Insert(symbolsToInsert);
            searchSetup.AddedSymbols = toAdd.ToList();
            searchSetup.Symbols.AddRange(symbols.OrderBy(a => a));
        }

        public List<SearchDetail> GetActiveSearchDetails()
        {
            return SearchDetailHelper.GetSearchDetails();
        }

        public List<SearchDetail> GetSearchDetailsForAll()
        {
            return SearchDetailHelper.GetSearchDetailsForAll();
        }

        public void SetSearchDetail(SearchDetail searchDetail)
        {
            SetCurrentSearchDetail(searchDetail);
        }

        public void ClearAdvanceSearch()
        {
            SearchDetailHelper.ClearSelectedAllSearches();
            SearchDetailHelper.ResetInitialSearch();
        }

        public SearchDetail SetInitialView(string symbol, int startYear, bool useDefaultSetting, string name)
        {
            var toView = GetAllSavedSearchDetails();
            var match = toView.FirstOrDefault(a => a.Name.Equals(name));
            if (!string.IsNullOrWhiteSpace(name) && match != null)
            {
                match.ActiveSelectedSearchDetails = toView;
                return match;
            }

            var currentSearchDetail = GetCurrentSearchDetail();
            if (currentSearchDetail != null && !string.IsNullOrWhiteSpace(symbol))
            {
                currentSearchDetail.Symbol = symbol;
                currentSearchDetail.SettingDate.Year = startYear;
            }
            if (useDefaultSetting)
                currentSearchDetail = GetInitialSearchDetail();

            if (currentSearchDetail != null)
                currentSearchDetail.ActiveSelectedSearchDetails = toView;

            return currentSearchDetail ?? GetCurrentSearchDetail();
        }

        public async Task<StockPerformanceHistory> PerformAdvanceSearch(bool searchAll)
        {
            if (searchAll)
                return await PerformAdvanceSearchForAll();

            var advancedSearchResult = new StockPerformanceHistory();

            List<SearchDetail> searchDetails = GetActiveSearchDetails();
            foreach (var searchDetail in searchDetails)
            {
                var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
                if (cachedResponse != null && cachedResponse?.CurrentPrice != 0)
                {
                    StockPerformanceHistoryStorage.AddResponse(cachedResponse);
                    advancedSearchResult.StockPerformanceResponses.Add(cachedResponse);

                    continue;
                }
                var response = await GetStockPerformanceResponse(searchDetail);
                StockPerformanceHistoryStorage.AddResponse(response);
                advancedSearchResult.StockPerformanceResponses.Add(response);
            }
            CachedHelper.AddCaches(advancedSearchResult.StockPerformanceResponses);
            advancedSearchResult.ProfitChart =
                new ProfitChart(advancedSearchResult.StockPerformanceResponses);

            ClearAdvanceSearch();

            return advancedSearchResult;
        }

        public async Task<StockPerformanceHistory> PerformAdvanceSearchForAll()
        {
            var advancedSearchResult = new StockPerformanceHistory();

            var details = GetSearchDetailsForAll();
            foreach (var searchDetail in details)
            {
                var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
                if (cachedResponse != null)
                {
                    StockPerformanceHistoryStorage.AddResponse(cachedResponse);
                    advancedSearchResult.StockPerformanceResponses.Add(cachedResponse);

                    continue;
                }
                var response = await GetStockPerformanceResponse(searchDetail);
                // StockPerformanceHistoryStorage.AddResponse(response);
                advancedSearchResult.StockPerformanceResponses.Add(response);
            }

            CachedHelper.AddCaches(advancedSearchResult.StockPerformanceResponses);
            advancedSearchResult.ProfitChart =
                new ProfitChart(advancedSearchResult.StockPerformanceResponses);

            ClearAdvanceSearch();

            return advancedSearchResult;
        }

        public StockPerformanceHistory GetStockPerformanceHistory()
        {
            var responses = CachedHelper.GetAllCache();
            var history = new StockPerformanceHistory
            {
                ProfitChart = new ProfitChart(responses),
                StockPerformanceResponses = responses
            };
            return history;
        }

        public void SaveSearchDetail(SearchDetail toSave)
        {
            if (toSave == null || toSave.DepositRule == null || toSave.TradingRule == null
                || toSave.SearchSetup == null)
                return;

            var symbols = _entityDefinitionsAccessor.GetAllSavedSymbols();
            var symbolIds = _entityDefinitionsAccessor.GetSymbolIds(symbols);

            var depositRule = new EntityDefinitions.DepositRule
            {
                DepositAmount = toSave.DepositRule.DepositAmount,
                FirstDepositDate = toSave.DepositRule.FirstDepositDate,
                InitialDepositAmount = toSave.DepositRule.InitialDepositAmount,
                NumberOfDepositDate = toSave.DepositRule.NumberOfDepositDate,
                SecondDepositDate = toSave.DepositRule.SecondDepositDate,
            };
            var tradingRule = new EntityDefinitions.TradingRule
            {
                LossLimitation = toSave.TradingRule.LossLimitation,
                NumberOfTradeAMonth = toSave.TradingRule.NumberOfTradeAMonth,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = toSave.TradingRule.SellAllWhenPriceDropAtPercentageSinceLastTrade,
                BuyPercentageLimitation = toSave.TradingRule.BuyPercentageLimitation,
                HigherRangeOfTradingDate = toSave.TradingRule.HigherRangeOfTradingDate,
                LowerRangeOfTradingDate = toSave.TradingRule.LowerRangeOfTradingDate,
                PurchaseLimitation = toSave.TradingRule.PurchaseLimitation,
                SellPercentageLimitation = toSave.TradingRule.SellPercentageLimitation
            };
            var setup = new EntityDefinitions.PerformanceSetup
            {
                StartingYear = toSave.SearchSetup.StartingYear,
                EndingYear = toSave.SearchSetup.EndingYear,
                StartingSymbolId = symbolIds.Min(),
                EndingSymbolId = symbolIds.Max(),
            };

            var symbol = new EntityDefinitions.Symbol
            {
                TradingSymbol = toSave.Symbol,

            };
            _entityDefinitionsAccessor.Insert(depositRule, tradingRule, symbol, setup, toSave.Name);
        }

        public List<SearchDetail> GetAllSavedSearchDetails()
        {
            return SearchDetailHelper.GetAllSearchDetails(_entityDefinitionsAccessor);
        }
    }

}

