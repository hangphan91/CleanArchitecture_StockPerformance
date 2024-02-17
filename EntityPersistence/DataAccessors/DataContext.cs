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

            var tradingRule = SetDefaultTradingRule();
            var symbols = SetDefaultSymbolList();
            var depositRule = SetDefaultDepositRule();

            DepositRules.Add(depositRule);
            TradingRules.Add(tradingRule);
            Symbols.AddRange(symbols);

            var lastSymbolId = Symbols.Last().Id;
            var firstSymbolId = Symbols.First().Id;

            var performanceSetup = SetDefaultPerformanceSetup(lastSymbolId, firstSymbolId);
            PerformanceSetups.Add(performanceSetup);
        }

        private PerformanceSetup SetDefaultPerformanceSetup(long lastSymbolId, long firstSymbolId)
        {
            return new PerformanceSetup
            {
                EndingSymbolId = lastSymbolId,
                StartingSymbolId = firstSymbolId,
                StartingYear = new DateOnly(DateTime.Now.AddYears(-4).Year, DateTime.Now.Month, DateTime.Now.Day),
                EndingYear = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
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

            symbols.AddRange(growingIn2022To2023);
            symbols = symbols.Select(a => a.ToUpper()).Distinct().ToList();
            var toSaveSymbols = symbols.Select(symbol => new Symbol
            {
                TradingSymbol = symbol.ToUpper(),
                Id = symbols.IndexOf(symbol),
            });

            return toSaveSymbols.Distinct().ToList();
        }

        private DepositRule SetDefaultDepositRule()
        {
            return new DepositRule
            {
                DepositAmount = 300,
                FirstDepositDate = 1,
                SecondDepositDate = 16,
                NumberOfDepositDate = 2,
                Id = 0,
                InitialDepositAmount = 3000,
            };
        }

        private static TradingRule SetDefaultTradingRule()
        {
            return new TradingRule
            {
                BuyPercentageLimitation = (decimal)1.07*100,
                SellPercentageLimitation = (decimal)0.94*100,
                HigherRangeOfTradingDate = 31,
                LowerRangeOfTradingDate = 1,
                PurchaseLimitation = 1000,
                Id = 0,
                LossLimitation = 3000,
                NumberOfTradeAMonth = 2,
                SellAllWhenPriceDropAtPercentageSinceLastTrade = (decimal)0.5* 100,
            };
        }
    }
}

