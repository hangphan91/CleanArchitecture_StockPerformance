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

        public async Task<MarketDataOptionChain> GetOptionsData(string symbol)
        {
            try
            {
                var token = "V1gteXlZUGJESUpMYjNMZjI0NXJNQ1l4OGJZS1g3RENPcVpqZHFqbDJwaz0";
                var url = $"https://api.marketdata.app/v1/options/chain/{symbol}/?expiration=2025-01-17&side=call";
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Headers =
                    {
                        { "Accept", "application/json" },
                        { "Authorization", $"Bearer {token}" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);

                    var myDeserializedClass = JsonConvert.DeserializeObject<MarketDataOptionChain>(body);
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