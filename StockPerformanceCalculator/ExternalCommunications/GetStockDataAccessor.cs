using NodaTime;
using YahooQuotesApi;

namespace HP.PersonalStocks.Mgr.Helpers
{
    public class GetStockDatAccessor
    {
        protected string CurrentSticker = "";
        protected DateTime _startingDate;
        public GetStockDatAccessor(string currentTicker, DateTime startingDate)
        {
            CurrentSticker = currentTicker;
            _startingDate = startingDate;
        }

        public async Task<Security?> GetHistoricalQuotesInfoAsync()
        {
            try
            {
                var year = _startingDate.Year;
                var month = _startingDate.Month;
                var day = _startingDate.Day;
                var quotes =
                    new YahooQuotesBuilder()
                    .WithHistoryStartDate(Instant.FromUtc(year, month, day, 0, 0, 0))
                    .Build();

                var result = await quotes.GetAsync(CurrentSticker, Histories.PriceHistory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Get Historical Quotes Info. {ex.Message}");
            }
        }

    }
}
