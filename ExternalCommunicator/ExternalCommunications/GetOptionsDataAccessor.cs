using System.Net.Http.Headers;
using ExternalCommunicator.ExternalCommunications.Models;
using Newtonsoft.Json;

namespace ExternalCommunicator.ExternalCommunications
{
    public class GetOptionsDataAccessor
    {

        public async Task GetOptionData2(string symbol)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://127.0.0.1:25510/v2/bulk_hist/option/quote?exp=20231103&start_date=20231103&end_date=20231103&root=AAPL&ivl=900000"),
                Headers =
                        {
                            { "Accept", "application/json" },
                        },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }

        public async Task<OptionChainResponse> GetOptionsData(string symbol)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v3/get-options?symbol={symbol}&region=US&lang=en-US"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "66ca04f1b2msh6e01f8b0fc4767ep1caaaejsn889b168ca42a" },
                        { "X-RapidAPI-Host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);

                    var myDeserializedClass = JsonConvert.DeserializeObject<OptionChainResponse>(body);
                    if (myDeserializedClass == null)
                        throw new Exception("Failed to retrieve data");

                    return myDeserializedClass;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed To Call OptionsData API", ex);
            }
        }
    }
}