using Newtonsoft.Json;

namespace ExternalCommunicator.ExternalCommunications.Models
{
    public class MarketDataOptionChain
    {
        [JsonProperty("s")]
        public string S { get; set; }

        [JsonProperty("optionSymbol")]
        public List<string> OptionSymbol { get; set; }

        [JsonProperty("underlying")]
        public List<string> Underlying { get; set; }

        [JsonProperty("expiration")]
        public List<int> Expiration { get; set; }

        [JsonProperty("side")]
        public List<string> Side { get; set; }

        [JsonProperty("strike")]
        public List<decimal> Strike { get; set; }

        [JsonProperty("firstTraded")]
        public List<int> FirstTraded { get; set; }

        [JsonProperty("dte")]
        public List<int> Dte { get; set; }

        [JsonProperty("updated")]
        public List<int> Updated { get; set; }

        [JsonProperty("bid")]
        public List<decimal> Bid { get; set; }

        [JsonProperty("bidSize")]
        public List<int> BidSize { get; set; }

        [JsonProperty("mid")]
        public List<decimal> Mid { get; set; }

        [JsonProperty("ask")]
        public List<decimal> Ask { get; set; }

        [JsonProperty("askSize")]
        public List<int> AskSize { get; set; }

        [JsonProperty("last")]
        public List<decimal> Last { get; set; }

        [JsonProperty("openInterest")]
        public List<int> OpenInterest { get; set; }

        [JsonProperty("volume")]
        public List<int> Volume { get; set; }

        [JsonProperty("inTheMoney")]
        public List<bool> InTheMoney { get; set; }

        [JsonProperty("intrinsicValue")]
        public List<decimal> IntrinsicValue { get; set; }

        [JsonProperty("extrinsicValue")]
        public List<decimal> ExtrinsicValue { get; set; }

        [JsonProperty("underlyingPrice")]
        public List<decimal> UnderlyingPrice { get; set; }

        [JsonProperty("iv")]
        public List<decimal> Iv { get; set; }

        [JsonProperty("delta")]
        public List<decimal> Delta { get; set; }

        [JsonProperty("gamma")]
        public List<decimal> Gamma { get; set; }

        [JsonProperty("theta")]
        public List<decimal> Theta { get; set; }

        [JsonProperty("vega")]
        public List<decimal> Vega { get; set; }

        [JsonProperty("rho")]
        public List<decimal> Rho { get; set; }
    }
}