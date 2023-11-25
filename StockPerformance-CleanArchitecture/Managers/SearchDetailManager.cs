using System;
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

        public async Task<StockPerformanceResponse> GetStockPerformanceResponse(
                    SearchDetail searchDetail)
        {
            var currentSearchDetail = GetCurrentSearchDetail();
            if (searchDetail == null || string.IsNullOrWhiteSpace(searchDetail.Symbol))
                searchDetail = currentSearchDetail;
            SetCurrentSearchDetail(searchDetail);

            var symbol = searchDetail.Symbol;
            var year = searchDetail.Year;
            var response = new StockPerformanceResponse(symbol, year);
            SetCurrentSearchDetail(searchDetail);
            var performanceMangager = new StockPerformanceManager(symbol, year, _entityDefinitionsAccessor);
            var mapped = SearchDetailHelper.Map(searchDetail);
            var summary = await performanceMangager.StartStockPerforamanceCalculation(mapped);

            response = response.Map(summary);

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
            searchSetup.Symbols.AddRange(symbols.OrderBy(a=> a));
        }
    }

}

