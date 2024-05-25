using System.Collections.Concurrent;

namespace StockPerformance_CleanArchitecture.Models.Storages
{
    public static class StockPerformanceHistoryStorage
    {
        public static ConcurrentBag<StockPerformanceResponse> StockPerformanceResponses = new ConcurrentBag<StockPerformanceResponse>();

        public static void AddResponse(StockPerformanceResponse response){
            if(StockPerformanceResponses == null)
                StockPerformanceResponses = new ConcurrentBag<StockPerformanceResponse>();

            StockPerformanceResponses.Add(response);
        }
    }
}