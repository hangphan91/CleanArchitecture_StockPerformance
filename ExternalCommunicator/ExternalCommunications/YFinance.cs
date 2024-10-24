using System.Net.Http.Headers;
using ExternalCommunications.Models;
using Newtonsoft.Json;
using StockPerformanceCalculator.ExternalCommunications;
public class YFinanceRapidApi
{
    public async Task<List<SymbolSummary>> GetHistory(string symbol, DateTime startingDate)
    {
        var client = new HttpClient();
        HttpRequestMessage request = PopulateRequest(startingDate, symbol);
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Root>(body);
            var history = new YahooFinanceAPIMapper().Map(result, symbol);
            return history;
        }
    }

    private static HttpRequestMessage PopulateRequest(DateTime startingDate, string symbol)
    {
        var numberOfday = DateTime.Now.Subtract(startingDate).TotalDays;
        var requestValue = numberOfday + "d";
        return new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://yfinance-stock-market-data.p.rapidapi.com/price-customdate"),
            Headers =
            {
                { "x-rapidapi-key", "66ca04f1b2msh6e01f8b0fc4767ep1caaaejsn889b168ca42a" },
                { "x-rapidapi-host", "yfinance-stock-market-data.p.rapidapi.com" },
            },
            Content = new MultipartFormDataContent
            {
                new StringContent(symbol)
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "symbol",
                        }
                    }
                },
                new StringContent(DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "end",
                        }
                    }
                },
                new StringContent(startingDate.ToString("yyyy-MM-dd"))
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "start",
                        }
                    }
                },
            },
        };
    }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Datum
{
    [JsonProperty("Close")]
    public double Close { get; set; }

    [JsonProperty("Date")]
    public long Date { get; set; }

    [JsonProperty("Dividends")]
    public double Dividends { get; set; }

    [JsonProperty("High")]
    public double High { get; set; }

    [JsonProperty("Low")]
    public double Low { get; set; }

    [JsonProperty("Open")]
    public double Open { get; set; }

    [JsonProperty("Stock Splits")]
    public double StockSplits { get; set; }

    [JsonProperty("Volume")]
    public int Volume { get; set; }
}

public class Root
{
    [JsonProperty("data")]
    public List<Datum> Data { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("status")]
    public int Status { get; set; }
}

