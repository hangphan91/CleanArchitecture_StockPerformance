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
                StartingYear = 2020,
                EndingYear = 2023,
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
                    "vmc","acls", "alv","avgo","matx"
                 };

            var toSaveSymbols = symbols.Select(symbol => new Symbol
            {
                TradingSymbol = symbol.ToUpper(),
                Id = symbols.IndexOf(symbol),
            });

            return toSaveSymbols;
        }

        private DepositRule SetDefaultDepositRule()
        {
            return new DepositRule
            {
                DepositAmount = 1500,
                FirstDepositDate = 1,
                SecondDepositDate = 16,
                NumberOfDepositDate = 2,
                Id = 0,
            };
        }

        private static TradingRule SetDefaultTradingRule()
        {
            return new TradingRule
            {
                BuyPercentageLimitation = (decimal)1.07,
                SellPercentageLimitation = (decimal)0.94,
                HigherRangeOfTradingDate = 25,
                LowerRangeOfTradingDate = 10,
                PurchaseLimitation = 5000,
                Id = 0,
            };
        }
    }
}

