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
    }

}

