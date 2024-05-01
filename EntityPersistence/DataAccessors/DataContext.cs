using EntityDefinitions;

namespace EntityPersistence.DataAccessors
{
    public class DataContext
    {
        public List<PerformanceIdHub> PerformanceIdHubs { get; set; }
        public List<DepositRule> DepositRules { get; set; }
        public List<PerformanceSummary> PerformanceSummaries { get; set; }
        public List<PerformanceByMonth> PerformanceByMonths { get; set; }
        public List<SymbolSummary> SymbolSummaries { get; set; }
        public List<TradingRule> TradingRules { get; set; }
        public List<Symbol> Symbols { get; set; }
        public List<PerformanceSetup> PerformanceSetups { get; set; }
        public List<Position> Positions { get; set; }
        public List<Deposit> Deposits { get; set; }
        public List<Email> Emails { get; set; }
        public List<SearchDetail> SearchDetails { get; set; }

        public DataContext()
        {
            DepositRules = new List<DepositRule>();
            PerformanceSummaries = new List<PerformanceSummary>();
            SymbolSummaries = new List<SymbolSummary>();
            TradingRules = new List<TradingRule>();
            Symbols = new List<Symbol>();
            PerformanceSetups = new List<PerformanceSetup>();
            PerformanceIdHubs = new List<PerformanceIdHub>();
            PerformanceByMonths = new List<PerformanceByMonth>();
            Positions = new List<Position>();
            Deposits = new List<Deposit>();
            Emails = new List<Email>();
            SearchDetails = new List<SearchDetail>();
            var symbols = SetDefaultSymbolList();
            Symbols.AddRange(symbols);

            for (int i = 0; i < 6; i++)
            {
                SetSearchDetail(i == 0);
            }

            Emails.AddRange(GetEmailAddresses());
        }

        private void SetSearchDetail(bool isDefault)
        {
            var tradingRule = GetRandomTradingRule(isDefault);
            var depositRule = GetRandomDepositRule(isDefault);

            DepositRules.Add(depositRule);
            TradingRules.Add(tradingRule);

            var lastSymbolId = Symbols.Last().Id;
            var firstSymbolId = Symbols.First().Id;

            var performanceSetup = GetRandomPerformanceSetup(lastSymbolId, firstSymbolId, isDefault);
            PerformanceSetups.Add(performanceSetup);
            var rand = new Random();
            var randSymbolId = (long)rand.Next((int)firstSymbolId, (int)lastSymbolId);
            SearchDetails.Add(GetSearchDetails(depositRule, tradingRule, randSymbolId, performanceSetup.Id, isDefault));
        }

        private SearchDetail GetSearchDetails(DepositRule depositRule,
            TradingRule tradingRule, long symbolId, long performanceSetupId, bool isDefault)
        {
            return new SearchDetail
            {
                DepositRuleId = depositRule.Id,
                TradingRuleId = tradingRule.Id,
                Name = isDefault ? "Default Setting" : "Saved Setting" + symbolId.ToString(),
                SymbolId = symbolId,
                PerformanceSetupId = performanceSetupId,
            };
        }

        private List<Email> GetEmailAddresses()
        {
            return new List<Email>
            {
                new Email{Id = 0, EmailAddress = "cristian.g.navarrete@gmail.com", FirstName = "Cristian"},
                //new Email{Id = 1, EmailAddress = "funnyluv122@gmail.com", FistName = "Hang"},
                new Email{Id = 2, EmailAddress = "stockperformance2023@gmail.com", FirstName = "Love"},
            };
        }

        private PerformanceSetup GetDefaultPerformanceSetup(long lastSymbolId, long firstSymbolId)
        {
            return new PerformanceSetup
            {
                EndingSymbolId = lastSymbolId,
                StartingSymbolId = firstSymbolId,
                StartingYear = new DateOnly(DateTime.Now.AddYears(-4).Year, DateTime.Now.Month, DateTime.Now.Day),
                EndingYear = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Id = 1,
            };
        }

        private PerformanceSetup GetRandomPerformanceSetup(long lastSymbolId, long firstSymbolId, bool isDefault)
        {
            if (isDefault)
                return GetDefaultPerformanceSetup(lastSymbolId, firstSymbolId);

            var rand = new Random();

            var randYear = rand.Next(2018, DateTime.Now.Year);
            var randMonth = rand.Next(1, 12);
            var randDay = rand.Next(1, 28);
            var randDate = new DateOnly(randYear, randMonth, randDay).AddDays(rand.Next(1, 100));
            return new PerformanceSetup
            {
                EndingSymbolId = lastSymbolId,
                StartingSymbolId = firstSymbolId,
                StartingYear = randDate,
                EndingYear = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Id = rand.Next(1, 1000),
            };
        }

        private IEnumerable<Symbol> SetDefaultSymbolList()
        {
            var symbols = new List<string>
                {
                    "pg", "low",  "cost", "sbux", "nke", "fdx", "ko", "nflx", "gm",
                    "aapl", "bac", "DOCU", "lly" , "ma", "nvo", "pbr", "alsn", "ares",
                    "GIC", "bwmx", "rs" , "spok", "arcb", "jbht", "googl", "nvda",
                    "msft", "lkncy", "vrt", "cmt", "stvn", "anet", "onto", "MLM",
                    "vmc","acls", "alv","avgo","matx", "goog"
                 };
            var growingIn2022To2023 = new List<string>
            {
                "ndva", "onto", "anet", "avgo", "msft", "vrt",
                "cdns", "cprt", "klac", "pcar", "tdg", "tol",
                "bld", "smci", "bkng", "pwr", "mlm", "lly",
                "sap", "arcb"
            };

            var growingIn2020To2024 = new List<string>
            {
                "SAP", "ALV", "CELH", "HCA", "CADE", "SBR",
                "FERG", "EP", "ARCH", "EG", "RGA",
            };

            var optionStocks042024 = new List<string>
            {
                "SPY", "SWAV", "GLD", "XOM", "ROOT", "UBER", "OXY",
                "DXYZ", "DJT", "NEM", "INTC", "DNUT", "GEO", "TDOC",
                "ZGN", "GCTS", "CADL",
            };

            var highOpenInterest042024 = new List<string>
            {
                "XLF", "XLE", "SPY", "IWM", "tsla", "mara", "riot", "coin",
                 "nkla", "nio", "wkhs", "lcid", "amzn", "spy", "gld", "xom",
                 "root", "uber", "oxy","dxyz", "djt","nem", "dnut", "geo",
                 "zgn", "gcts", "cadl","xlf","iwm", "xle","xlf"
            };

            var from20To50Dollar = new List<string>
            {
                "CLS", "CLF", "X", "rig", "WFC", "WMT", "MS", "ET", "BSX",
            };

            var from20to50Finviz = new List<string>
            {
                "CSX", "bkr"
            };

            symbols.AddRange(growingIn2022To2023);
            symbols.AddRange(growingIn2020To2024);
            symbols.AddRange(optionStocks042024);
            symbols.AddRange(highOpenInterest042024);
            symbols.AddRange(from20To50Dollar);
            symbols.AddRange(from20to50Finviz);

            symbols = symbols.Select(a => a.ToUpper()).Distinct().ToList();
            var toSaveSymbols = symbols.Select(symbol => new Symbol
            {
                TradingSymbol = symbol.ToUpper(),
                Id = symbols.IndexOf(symbol),
            });

            return toSaveSymbols.Distinct().ToList();
        }

        private DepositRule GetDefaultDepositRule()
        {
            return new DepositRule
            {
                DepositAmount = 300,
                FirstDepositDate = 1,
                SecondDepositDate = 16,
                NumberOfDepositDate = 2,
                Id = 0,
                InitialDepositAmount = 0,
            };
        }

        private DepositRule GetRandomDepositRule(bool isDefault)
        {
            if (isDefault)
                return GetDefaultDepositRule();

            var rand = new Random();

            return new DepositRule
            {
                DepositAmount = rand.Next(1, 5) * 100,
                FirstDepositDate = 1,
                SecondDepositDate = 16,
                NumberOfDepositDate = 2,
                Id = rand.Next(1, 1000),
                InitialDepositAmount = rand.Next(0, 5) * 100,
            };
        }

        private static TradingRule GetDefaultTradingRule()
        {
            return new TradingRule
            {
                BuyPercentageLimitation = (decimal)0,
                SellPercentageLimitation = (decimal)20,
                HigherRangeOfTradingDate = 31,
                LowerRangeOfTradingDate = 1,
                PurchaseLimitation = 1000,
                Id = 0,
                LossLimitation = 3000,
                NumberOfTradeAMonth = 2,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = (decimal)0.5 * 100,
            };
        }

        private static TradingRule GetRandomTradingRule(bool isDefault)
        {
            if (isDefault)
                return GetDefaultTradingRule();

            var rand = new Random();
            var randBuyNumber = (decimal)rand.Next(0, 20);
            var randSellNumber = (decimal)rand.Next(0, 20);
            var randTradeDateHigh = rand.Next(15, 30);
            var randTradeDateLow = rand.Next(1, 14);

            var randPurchaseLimit = rand.Next(1, 5) * 500;
            var randLostLimit = rand.Next(1, 5) * randPurchaseLimit;
            var randNumberOfTrade = rand.Next(1, 2);
            var randSellAll = ((decimal)rand.Next(1, 10)) * 10;
            return new TradingRule
            {
                BuyPercentageLimitation = randBuyNumber,
                SellPercentageLimitation = randSellNumber,
                HigherRangeOfTradingDate = randTradeDateHigh,
                LowerRangeOfTradingDate = randTradeDateLow,
                PurchaseLimitation = randPurchaseLimit,
                Id = rand.Next(1, 1000),
                LossLimitation = randLostLimit,
                NumberOfTradeAMonth = randNumberOfTrade,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = randSellAll,
            };
        }
    }
}

