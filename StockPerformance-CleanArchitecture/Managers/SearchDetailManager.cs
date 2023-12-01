using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.DatabaseAccessors;
using StockPerformanceCalculator.Logic;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SearchDetailManager
    {
        IEntityDefinitionsAccessor _entityDefinitionsAccessor;
        public SearchDetailManager()
        {
            _entityDefinitionsAccessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
        }

        internal SearchDetail GetCurrentSearchDetail()
        {
            return SearchDetailHelper.GetCurrentSearchDetail(_entityDefinitionsAccessor);
        }

        private SearchInitialSetup GetSearchInitialSetup()
        {
            return SearchDetailHelper.GetSearchInitialSetup();
        }

        internal void SetCurrentSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetSearchInitialSetup();
            SearchDetailHelper.SetCurrentSearchDetail(searchDetail);
        }

        internal void AddAdvanceSearchDetail(SearchDetail searchDetail)
        {
            searchDetail.SearchSetup = GetSearchInitialSetup();
            SearchDetailHelper.AddAdvanceSearchDetail(searchDetail);
        }

        public AdvanceSearch UpdateAdvanceSearch(AdvanceSearch advanceSearch, bool willClearAllSearch)
        {
            if (willClearAllSearch)
                ClearAdvanceSearch();

            var searchDetails = GetSearchDetails();
            var accessor = DatabaseAccessorHelper.EntityDefinitionsAccessor;
            var detail = SearchDetailHelper.GetCurrentSearchDetail(accessor);

            if ((string.IsNullOrWhiteSpace(advanceSearch.SearchDetail?.Symbol) && !searchDetails.Any())
                || willClearAllSearch)
            {
                advanceSearch.SearchDetail = detail;
                return advanceSearch;
            }

            for (int year = advanceSearch.StartYear; year <= advanceSearch.EndYear; year++)
            {
                advanceSearch.SearchDetail.Year = year;
                var searchDetail = new SearchDetail
                {
                    DepositRule = advanceSearch.SearchDetail.DepositRule,
                    SearchSetup = advanceSearch.SearchDetail.SearchSetup,
                    Symbol = advanceSearch.SearchDetail.Symbol,
                    TradingRule = advanceSearch.SearchDetail.TradingRule,
                    Year = advanceSearch.SearchDetail.Year,

                };
                AddAdvanceSearchDetail(searchDetail);

                advanceSearch.Symbols = detail.SearchSetup.Symbols.Select(a => a).ToList();
                advanceSearch.SearchDetail.SearchSetup.Symbols = advanceSearch.Symbols;
                advanceSearch.Count = searchDetails.Count();
            }

            var result = new AdvanceSearch
            {
                Count = searchDetails.Count(),
                SearchDetail = advanceSearch.SearchDetail,
                Symbols = advanceSearch.SearchDetails.Select(a => a.Symbol).ToList(),
                SearchDetails = searchDetails.Select(a => a).ToList(),
                StartYear = advanceSearch.StartYear,
                EndYear = advanceSearch.EndYear,
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
            var currentSearchDetail = GetCurrentSearchDetail();
            if (searchDetail == null || string.IsNullOrWhiteSpace(searchDetail.Symbol))
                searchDetail = currentSearchDetail;
            SetCurrentSearchDetail(searchDetail);

            var symbol = searchDetail.Symbol ?? "AAPL";
            var year = searchDetail.Year == 0 ? 2020 : searchDetail.Year;
            var response = new StockPerformanceResponse(symbol, year);
            SetCurrentSearchDetail(searchDetail);

            var performanceMangager = SearchDetailHelper.GetStockPerformanceManager(year, symbol);

            var mapped = SearchDetailHelper.Map(searchDetail);
            var summary = await performanceMangager.StartStockPerforamanceCalculation(mapped);

            response = response.Map(summary);
            response.SearchDetail = searchDetail;

            CachedHelper.AddCache(response);
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

        internal void SetSearchDetail(SearchDetail searchDetail)
        {
            SetCurrentSearchDetail(searchDetail);
        }

        internal void ClearAdvanceSearch()
        {
            SearchDetailHelper.ClearSearchDetails();
        }

        public SearchDetail SetInitialView(string symbol)
        {
            var currentSearchDetail = GetCurrentSearchDetail();
            if (!string.IsNullOrWhiteSpace(symbol))
                currentSearchDetail.Symbol = symbol;
            return currentSearchDetail;
        }

        public async Task<StockPerformanceHistory> PerformAdvanceSearch()
        {
            var advancedSearchResult = new StockPerformanceHistory();

            foreach (var searchDetail in GetSearchDetails())
            {
                var response = await GetStockPerformanceResponse(searchDetail);
                advancedSearchResult.StockPerformanceResponses.Add(response);
            }

            CachedHelper.AddCaches(advancedSearchResult.StockPerformanceResponses);
            return advancedSearchResult;
        }

        public StockPerformanceHistory GetStockPerformanceHistory()
        {
            var responses = CachedHelper.GetAllCache();
            var history = new StockPerformanceHistory
            {
                StockPerformanceResponses = responses
            };
            return history;
        }

    }

}

