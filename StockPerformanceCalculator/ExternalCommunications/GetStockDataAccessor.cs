using System.Net.Http.Headers;
using NodaTime;
using YahooQuotesApi;
using OoplesFinance.YahooFinanceAPI;
using OoplesFinance.YahooFinanceAPI.Enums;
using OoplesFinance.YahooFinanceAPI.Models;
using Fynance;
using Newtonsoft.Json;
using System;

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


        public async Task<RapiAPiResponse> GetHistoricalDataRapicAPI()
        {
            var now = DateTime.Now;
            var year = _startingDate.Year;
            var month = _startingDate.Month;
            var day = _startingDate.Day;

            var a = "symbol=" +CurrentSticker +"&";
            var b = "dateStart="+ year + "-" + month.ToString("D2") + "-" + day.ToString("D2") + "&";
            var c = "dateEnd=" + now.Year + "-" + now.Month.ToString("D2") + "-" + now.Day.ToString("D2") + "&";

            string QUERY_URL = "https://apistocks.p.rapidapi.com/weekly?" + a + b + c;

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(QUERY_URL),
                Headers =
                        {
                            { "X-RapidAPI-Key", "66ca04f1b2msh6e01f8b0fc4767ep1caaaejsn889b168ca42a" },
                            { "X-RapidAPI-Host", "apistocks.p.rapidapi.com" },
                        },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var myDeserializedClass = JsonConvert.DeserializeObject<RapiAPiResponse>(body);
                if (myDeserializedClass == null)
                    throw new Exception("Failed to retrieve data");

                return myDeserializedClass;
            }

            throw new Exception("Failed to retrieve data");
        }

        public async Task<Security?> GetHistoricalQuotesInfoAsyncFromYahoo()
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


        public async Task< List<HistoricalData>> GetHistoricalQuotesInfoAsyncFromYahoo2()
        {
            try
            {
                var year = _startingDate.Year;
                var month = _startingDate.Month;
                var day = _startingDate.Day;
                var yahooClient = new YahooClient();
                var historicalDataList = await yahooClient
                    .GetHistoricalDataAsync(CurrentSticker, DataFrequency.Daily, _startingDate, DateTime.Now);

                return historicalDataList.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Get Historical Quotes Info. {ex.Message}");
            }
        }

        public async Task<Fynance.Result.FyResult> GetHistoricalQuotesInfoAsyncFromYahoo3()
        {
            var year = _startingDate.Year;
            var month = _startingDate.Month;
            var day = _startingDate.Day;
            var period = Fynance.Period.OneYear;
            try
            {
                period = GetPeriod(year, period);

                var yahooClient = new YahooClient();

                var result = await Ticker.Build()
                         .SetSymbol(CurrentSticker)
                         .SetPeriod(period)
                         .SetInterval(Fynance.Interval.OneDay)
                         .GetAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Get Historical Quotes Info. {ex.Message}");
            }
        }

        private static Fynance.Period GetPeriod(int year, Fynance.Period period)
        {
            var yearNumber = DateTime.Now.Year - year + 1;
            switch (yearNumber)
            {
                case 1:
                    period = Fynance.Period.OneYear;
                    break;
                case 2:
                    period = Fynance.Period.TwoYears;
                    break;
                case 3:
                case 4:
                case 5:
                    period = Fynance.Period.FiveYears;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                default:
                    period = Fynance.Period.TenYears;
                    break;
            }

            return period;
        }

        public async Task<Response> GetHistoricalQuotesInfoAsyncFromMarketStack()
        {
            var apiKey = "62fc8f7f2a50984eebb353419743d1a9";
            

            try
            {
                var now = DateTime.Now;
                var year = _startingDate.Year;
                var month = _startingDate.Month;
                var day = _startingDate.Day;
                string QUERY_URL = $"http://api.marketstack.com/v1/eod?access_key={apiKey}" +
                    $"&symbols={CurrentSticker}&" +
                    $"date_from={year}-{month.ToString("D2")}-{day.ToString("D2")}&" +
                    $"date_to={now.Year}-{now.Month.ToString("D2")}-{now.Day.ToString("D2")}&" +
                    $"limit=5000";
                using (HttpClient client = new HttpClient())
                {

                    var json_data = await client.GetAsync(QUERY_URL);
                    var json = await json_data.Content.ReadAsStringAsync();
                    var result = System.Text.Json.JsonSerializer.Deserialize<Response>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class Datum
    {
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
        public double? adj_high { get; set; }
        public double? adj_low { get; set; }
        public double adj_close { get; set; }
        public double? adj_open { get; set; }
        public double? adj_volume { get; set; }
        public double split_factor { get; set; }
        public double dividend { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string date { get; set; }
    }

    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class Response
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("symbol")]
        public string Symbol;

        [JsonProperty("interval")]
        public string Interval;

        [JsonProperty("timezone")]
        public string Timezone;
    }

    public class Result
    {
        [JsonProperty("date")]
        public string Date;

        [JsonProperty("open")]
        public double Open;

        [JsonProperty("close")]
        public double Close;

        [JsonProperty("high")]
        public double High;

        [JsonProperty("low")]
        public double Low;

        [JsonProperty("adjClose")]
        public double AdjClose;
    }

    public class RapiAPiResponse
    {
        [JsonProperty("metadata")]
        public Metadata Metadata;

        [JsonProperty("results")]
        public List<Result> Results;
    }
}
