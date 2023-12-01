using System.Collections.Concurrent;
using StockPerformance_CleanArchitecture.Models;

namespace StockPerformance_CleanArchitecture.Helpers
{
    public static class CachedHelper
	{
		private static ConcurrentBag<StockPerformanceResponse> _responses;

		public static void AddCache(StockPerformanceResponse searchDetail)
		{
			if (_responses == null)
				_responses = new ConcurrentBag<StockPerformanceResponse>();
			_responses.Add(searchDetail);
		}


        public static void AddCaches(List<StockPerformanceResponse> responses)
        {
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();

			foreach (var response in responses)
			{
                _responses.Add(response);
            }
        }


        public static List<StockPerformanceResponse> GetAllCache()
		{
            if (_responses == null)
                _responses = new ConcurrentBag<StockPerformanceResponse>();
            return _responses.Select(a => a).ToList();
		}
	
	}
}

