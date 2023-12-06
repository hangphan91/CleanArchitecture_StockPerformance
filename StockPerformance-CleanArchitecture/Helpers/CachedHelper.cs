using System.Collections.Concurrent;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Helpers
{
    public static class CachedHelper
    {
        private static ConcurrentBag<StockPerformanceResponse> _responses;

        public static void AddCache(StockPerformanceResponse response)
        {
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();

            if (GetResponseFromCache(response.SearchDetail) == null)
                _responses.Add(response);
        }


        public static void AddCaches(List<StockPerformanceResponse> responses)
        {
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();

            foreach (var response in responses)
            {
                if (GetResponseFromCache(response.SearchDetail) == null)
                    _responses.Add(response);
            }
        }

        public static List<StockPerformanceResponse> GetAllCache()
        {
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();
            return _responses.OrderBy(a => a.Symbol)
            .ThenBy(a => a.SearchDetail.SettingDate.Year).ToList();
        }

        internal static StockPerformanceResponse GetResponseFromCache(SearchDetail searchDetail)
        {
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();

            var cachedResponse = _responses.Where(a => a.SearchDetail.IsSame(searchDetail)).FirstOrDefault();

            return cachedResponse;
        }


    }
}

