using Newtonsoft.Json;

namespace ExternalCommunicator.ExternalCommunications.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Call
    {
        [JsonProperty("contractSymbol")]
        public string ContractSymbol;

        [JsonProperty("strike")]
        public double? Strike;

        [JsonProperty("currency")]
        public string Currency;

        [JsonProperty("lastPrice")]
        public double? LastPrice;

        [JsonProperty("change")]
        public double? Change;

        [JsonProperty("percentChange")]
        public double? PercentChange;

        [JsonProperty("volume")]
        public int? Volume;

        [JsonProperty("openInterest")]
        public int? OpenInterest;

        [JsonProperty("bid")]
        public double? Bid;

        [JsonProperty("ask")]
        public double? Ask;

        [JsonProperty("contractSize")]
        public string ContractSize;

        [JsonProperty("expiration")]
        public int? Expiration;

        [JsonProperty("lastTradeDate")]
        public int? LastTradeDate;

        [JsonProperty("impliedVolatility")]
        public double? ImpliedVolatility;

        [JsonProperty("inTheMoney")]
        public bool? InTheMoney;
    }

    public class Option
    {
        [JsonProperty("expirationDate")]
        public int? ExpirationDate;

        [JsonProperty("hasMiniOptions")]
        public bool? HasMiniOptions;

        [JsonProperty("straddles")]
        public List<Straddle> Straddles;
    }

    public class OptionChain
    {
        [JsonProperty("result")]
        public List<Result> Result;

        [JsonProperty("error")]
        public object Error;
    }

    public class Put
    {
        [JsonProperty("contractSymbol")]
        public string ContractSymbol;

        [JsonProperty("strike")]
        public double? Strike;

        [JsonProperty("currency")]
        public string Currency;

        [JsonProperty("lastPrice")]
        public double? LastPrice;

        [JsonProperty("change")]
        public double? Change;

        [JsonProperty("percentChange")]
        public double? PercentChange;

        [JsonProperty("volume")]
        public int? Volume;

        [JsonProperty("openInterest")]
        public int? OpenInterest;

        [JsonProperty("bid")]
        public double? Bid;

        [JsonProperty("ask")]
        public double? Ask;

        [JsonProperty("contractSize")]
        public string ContractSize;

        [JsonProperty("expiration")]
        public int? Expiration;

        [JsonProperty("lastTradeDate")]
        public int? LastTradeDate;

        [JsonProperty("impliedVolatility")]
        public double? ImpliedVolatility;

        [JsonProperty("inTheMoney")]
        public bool? InTheMoney;
    }

    public class Quote
    {
        // [JsonProperty("language")]
        // public string Language;

        // [JsonProperty("region")]
        // public string Region;

        // [JsonProperty("quoteType")]
        // public string QuoteType;

        // [JsonProperty("typeDisp")]
        // public string TypeDisp;

        // [JsonProperty("quoteSourceName")]
        // public string QuoteSourceName;

        // [JsonProperty("triggerable")]
        // public bool? Triggerable;

        // [JsonProperty("customPriceAlertConfidence")]
        // public string CustomPriceAlertConfidence;

        // [JsonProperty("currency")]
        // public string Currency;

        // [JsonProperty("marketState")]
        // public string MarketState;

        // [JsonProperty("regularMarketChangePercent")]
        // public double? RegularMarketChangePercent;

        // [JsonProperty("regularMarketPrice")]
        // public double? RegularMarketPrice;

        // [JsonProperty("exchange")]
        // public string Exchange;

        // [JsonProperty("shortName")]
        // public string ShortName;

        // [JsonProperty("longName")]
        // public string LongName;

        // [JsonProperty("messageBoardId")]
        // public string MessageBoardId;

        // [JsonProperty("exchangeTimezoneName")]
        // public string ExchangeTimezoneName;

        // [JsonProperty("exchangeTimezoneShortName")]
        // public string ExchangeTimezoneShortName;

        // [JsonProperty("gmtOffSetMilliseconds")]
        // public int? GmtOffSetMilliseconds;

        // [JsonProperty("market")]
        // public string Market;

        // [JsonProperty("esgPopulated")]
        // public bool? EsgPopulated;

        // [JsonProperty("hasPrePostMarketData")]
        // public bool? HasPrePostMarketData;

        // [JsonProperty("firstTradeDateMilliseconds")]
        // public long? FirstTradeDateMilliseconds;

        // [JsonProperty("priceHint")]
        // public int? PriceHint;

        // [JsonProperty("postMarketChangePercent")]
        // public double? PostMarketChangePercent;

        // [JsonProperty("postMarketTime")]
        // public int? PostMarketTime;

        // [JsonProperty("postMarketPrice")]
        // public double? PostMarketPrice;

        // [JsonProperty("postMarketChange")]
        // public double? PostMarketChange;

        // [JsonProperty("regularMarketChange")]
        // public double? RegularMarketChange;

        // [JsonProperty("regularMarketTime")]
        // public int? RegularMarketTime;

        // [JsonProperty("regularMarketDayHigh")]
        // public double? RegularMarketDayHigh;

        // [JsonProperty("regularMarketDayRange")]
        // public string RegularMarketDayRange;

        // [JsonProperty("regularMarketDayLow")]
        // public double? RegularMarketDayLow;

        // [JsonProperty("regularMarketVolume")]
        // public int? RegularMarketVolume;

        // [JsonProperty("regularMarketPreviousClose")]
        // public double? RegularMarketPreviousClose;

        // [JsonProperty("bid")]
        // public double? Bid;

        // [JsonProperty("ask")]
        // public double? Ask;

        // [JsonProperty("bidSize")]
        // public int? BidSize;

        // [JsonProperty("askSize")]
        // public int? AskSize;

        // [JsonProperty("fullExchangeName")]
        // public string FullExchangeName;

        // [JsonProperty("financialCurrency")]
        // public string FinancialCurrency;

        // [JsonProperty("regularMarketOpen")]
        // public double? RegularMarketOpen;

        [JsonProperty("averageDailyVolume3Month")]
        public int? AverageDailyVolume3Month;

        [JsonProperty("averageDailyVolume10Day")]
        public int? AverageDailyVolume10Day;

        // [JsonProperty("fiftyTwoWeekLowChange")]
        // public double? FiftyTwoWeekLowChange;

        // [JsonProperty("fiftyTwoWeekLowChangePercent")]
        // public double? FiftyTwoWeekLowChangePercent;

        // [JsonProperty("fiftyTwoWeekRange")]
        // public string FiftyTwoWeekRange;

        // [JsonProperty("fiftyTwoWeekHighChange")]
        // public double? FiftyTwoWeekHighChange;

        // [JsonProperty("fiftyTwoWeekHighChangePercent")]
        // public double? FiftyTwoWeekHighChangePercent;

        // [JsonProperty("fiftyTwoWeekLow")]
        // public double? FiftyTwoWeekLow;

        // [JsonProperty("fiftyTwoWeekHigh")]
        // public double? FiftyTwoWeekHigh;

        // [JsonProperty("fiftyTwoWeekChangePercent")]
        // public double? FiftyTwoWeekChangePercent;

        // [JsonProperty("earningsTimestamp")]
        // public int? EarningsTimestamp;

        // [JsonProperty("earningsTimestampStart")]
        // public int? EarningsTimestampStart;

        // [JsonProperty("earningsTimestampEnd")]
        // public int? EarningsTimestampEnd;

        // [JsonProperty("trailingAnnualDividendRate")]
        // public double? TrailingAnnualDividendRate;

        // [JsonProperty("trailingAnnualDividendYield")]
        // public int? TrailingAnnualDividendYield;

        // [JsonProperty("epsTrailingTwelveMonths")]
        // public double? EpsTrailingTwelveMonths;

        // [JsonProperty("epsForward")]
        // public double? EpsForward;

        // [JsonProperty("epsCurrentYear")]
        // public double? EpsCurrentYear;

        // [JsonProperty("priceEpsCurrentYear")]
        // public double? PriceEpsCurrentYear;

        // [JsonProperty("sharesOutstanding")]
        // public int? SharesOutstanding;

        // [JsonProperty("bookValue")]
        // public double? BookValue;

        // [JsonProperty("fiftyDayAverage")]
        // public double? FiftyDayAverage;

        // [JsonProperty("fiftyDayAverageChange")]
        // public double? FiftyDayAverageChange;

        // [JsonProperty("fiftyDayAverageChangePercent")]
        // public double? FiftyDayAverageChangePercent;

        // [JsonProperty("twoHundredDayAverage")]
        // public double? TwoHundredDayAverage;

        // [JsonProperty("twoHundredDayAverageChange")]
        // public double? TwoHundredDayAverageChange;

        // [JsonProperty("twoHundredDayAverageChangePercent")]
        // public double? TwoHundredDayAverageChangePercent;

        // [JsonProperty("marketCap")]
        // public int? MarketCap;

        // [JsonProperty("forwardPE")]
        // public double? ForwardPE;

        // [JsonProperty("priceToBook")]
        // public double? PriceToBook;

        // [JsonProperty("sourceInterval")]
        // public int? SourceInterval;

        // [JsonProperty("exchangeDataDelayedBy")]
        // public int? ExchangeDataDelayedBy;

        // [JsonProperty("averageAnalystRating")]
        // public string AverageAnalystRating;

        // [JsonProperty("tradeable")]
        // public bool? Tradeable;

        // [JsonProperty("cryptoTradeable")]
        // public bool? CryptoTradeable;

        [JsonProperty("displayName")]
        public string DisplayName;

        [JsonProperty("symbol")]
        public string Symbol;
    }

    public class Result
    {
        // [JsonProperty("underlyingSymbol")]
        // public string UnderlyingSymbol;

        // [JsonProperty("expirationDates")]
        // public List<int?> ExpirationDates;

        // [JsonProperty("strikes")]
        // public List<double?> Strikes;

        // [JsonProperty("hasMiniOptions")]
        // public bool? HasMiniOptions;

        [JsonProperty("quote")]
        public Quote Quote;

        [JsonProperty("options")]
        public List<Option> Options;
    }

    public class OptionChainResponse
    {
        [JsonProperty("optionChain")]
        public OptionChain OptionChain;
    }

    public class Straddle
    {
        [JsonProperty("strike")]
        public double? Strike;

        [JsonProperty("call")]
        public Call Call;

        [JsonProperty("put")]
        public Put Put;
    }


}