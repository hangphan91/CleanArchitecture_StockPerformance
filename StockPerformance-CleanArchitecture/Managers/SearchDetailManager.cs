using FusionChartsRazorSamples.Pages;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformance_CleanArchitecture.Models.Settings;
using StockPerformanceCalculator.DatabaseAccessors;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SearchDetailManager
    {
        IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        private bool IsFirstAdvancedSearch = true;
        SendEmailTimer _timer;
        public SearchDetailManager()
        {
            _entityDefinitionsAccessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            _timer = new SendEmailTimer();
        }

        internal SearchDetail GetCurrentSearchDetail()
        {
            return SearchDetailHelper.GetCurrentSearchDetail(_entityDefinitionsAccessor);
        }

        internal SearchDetail GetInitialSearchDetail()
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

        internal void SetCurrentSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetCurrentSearchSetup();
            SearchDetailHelper.SetCurrentSearchDetail(searchDetail);
        }

        internal void AddAdvanceSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetCurrentSearchSetup();
            SearchDetailHelper.AddAdvanceSearchDetail(searchDetail);
        }

        public AdvanceSearch UpdateAdvanceSearch(AdvanceSearch advanceSearch,
            bool willClearAllSearch)
        {
            if (willClearAllSearch)
                ClearAdvanceSearch();

            var searchDetails = GetSearchDetails();
            var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            var detail = SearchDetailHelper.GetCurrentSearchDetail(accessor);

            if (willClearAllSearch || advanceSearch.StartDate.Day == 0)
            {
                advanceSearch.SearchDetail = detail;
                return advanceSearch;
            }
            if (!IsFirstAdvancedSearch)
                for (int year = advanceSearch.StartDate.Year; year <= advanceSearch.EndDate.Year; year++)
                {
                    var month = advanceSearch.StartDate.Month;
                    var day = advanceSearch.StartDate.Day;
                    advanceSearch.SearchDetail.SettingDate = new SettingDate(year, month, day);
                    var searchDetail = new SearchDetail
                    {
                        DepositRule = advanceSearch.SearchDetail.DepositRule,
                        SearchSetup = advanceSearch.SearchDetail.SearchSetup,
                        Symbol = advanceSearch.SearchDetail.Symbol,
                        TradingRule = advanceSearch.SearchDetail.TradingRule,
                        SettingDate = advanceSearch.SearchDetail.SettingDate,

                    };
                    AddAdvanceSearchDetail(searchDetail);

                    advanceSearch.Symbols = detail.SearchSetup.Symbols.Select(a => a).ToList();
                    advanceSearch.SearchDetail.SearchSetup.Symbols = advanceSearch.Symbols;
                    advanceSearch.Count = searchDetails.Count();
                    advanceSearch.SearchDetail.SearchSetup.StartingYear = advanceSearch.StartDate.MapDateOnly();
                    advanceSearch.SearchDetail.SearchSetup.EndingYear = advanceSearch.EndDate.MapDateOnly();
                }
            var result = new AdvanceSearch
            {
                Count = searchDetails.Count(),
                SearchDetail = advanceSearch.SearchDetail,
                Symbols = advanceSearch.SearchDetails.Select(a => a.Symbol).ToList(),
                SearchDetails = searchDetails.Select(a => a).ToList(),
                StartDate = advanceSearch.StartDate,
                EndDate = advanceSearch.EndDate,
                WillPerformSearch = advanceSearch.WillPerformSearch
            };

            IsFirstAdvancedSearch = false;
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

            var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
            if (cachedResponse != null)
                return cachedResponse;

            var currentSearchDetail = GetCurrentSearchDetail();
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
            _timer.AddResponse(response, _entityDefinitionsAccessor.GetEmails());
            return response;
        }

        internal void AddNewSymbols(SearchInitialSetup searchSetup, List<string> symbols)
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

        internal List<SearchDetail> GetSearchDetails()
        {
            return SearchDetailHelper.GetSearchDetails();
        }

        internal List<SearchDetail> GetSearchDetailsForAll()
        {
            return SearchDetailHelper.GetSearchDetailsForAll();
        }

        internal void SetSearchDetail(SearchDetail searchDetail)
        {
            SetCurrentSearchDetail(searchDetail);
        }

        internal void ClearAdvanceSearch()
        {
            SearchDetailHelper.ClearSearchDetails();
        }

        public SearchDetail SetInitialView(string symbol, int startYear, bool useDefaultSetting)
        {

            var currentSearchDetail = GetCurrentSearchDetail();
            if (currentSearchDetail != null && !string.IsNullOrWhiteSpace(symbol))
            {
                currentSearchDetail.Symbol = symbol;
                currentSearchDetail.SettingDate.Year = startYear;
            }
            if (useDefaultSetting)
                currentSearchDetail = GetInitialSearchDetail();

            return currentSearchDetail;
        }

        public async Task<StockPerformanceHistory> PerformAdvanceSearch(bool searchAll)
        {
            if (searchAll)
                return await PerformAdvanceSearchForAll();

            var advancedSearchResult = new StockPerformanceHistory();

            foreach (var searchDetail in GetSearchDetails())
            {
                var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
                if (cachedResponse != null)
                {
                    advancedSearchResult.StockPerformanceResponses.Add(cachedResponse);
                    continue;
                }
                var response = await GetStockPerformanceResponse(searchDetail);
                advancedSearchResult.StockPerformanceResponses.Add(response);
                advancedSearchResult.StockPerformanceResponses =
                    advancedSearchResult.StockPerformanceResponses
                    .OrderByDescending(a => a.ProfitInDollar)
                    .ToList();
            }
            advancedSearchResult.StockPerformanceResponses =
                advancedSearchResult.StockPerformanceResponses.OrderBy(a => a.Symbol)
            .ThenBy(a => a.SearchDetail.SettingDate.Year).ToList();

            CachedHelper.AddCaches(advancedSearchResult.StockPerformanceResponses);
            advancedSearchResult.ProfitChart =
                new FusionChartsRazorSamples.Pages.ProfitChart(advancedSearchResult.StockPerformanceResponses);

            ClearAdvanceSearch();

            return advancedSearchResult;
        }

        public async Task<StockPerformanceHistory> PerformAdvanceSearchForAll()
        {
            var advancedSearchResult = new StockPerformanceHistory();

            foreach (var searchDetail in GetSearchDetailsForAll())
            {
                var cachedResponse = CachedHelper.GetResponseFromCache(searchDetail);
                if (cachedResponse != null)
                {
                    advancedSearchResult.StockPerformanceResponses.Add(cachedResponse);
                    continue;
                }
                var response = await GetStockPerformanceResponse(searchDetail);
                advancedSearchResult.StockPerformanceResponses.Add(response);
                advancedSearchResult.StockPerformanceResponses =
                    advancedSearchResult.StockPerformanceResponses
                    .OrderByDescending(a => a.ProfitInDollar)
                    .ToList();
            }
            advancedSearchResult.StockPerformanceResponses =
                advancedSearchResult.StockPerformanceResponses.OrderBy(a => a.Symbol)
            .ThenBy(a => a.SearchDetail.SettingDate.Year).ToList();

            CachedHelper.AddCaches(advancedSearchResult.StockPerformanceResponses);
            advancedSearchResult.ProfitChart =
                new FusionChartsRazorSamples.Pages.ProfitChart(advancedSearchResult.StockPerformanceResponses);

            ClearAdvanceSearch();

            return advancedSearchResult;
        }

        public StockPerformanceHistory GetStockPerformanceHistory()
        {
            var responses = CachedHelper.GetAllCache();
            var history = new StockPerformanceHistory
            {
                StockPerformanceResponses = responses,
                ProfitChart = new FusionChartsRazorSamples.Pages.ProfitChart(responses)
            };
            return history;
        }

    }

}

