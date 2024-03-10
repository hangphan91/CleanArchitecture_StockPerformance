using System;
namespace EntityDefinitions
{
    public class SearchDetail : IdBase
    {
        public string Name { get; set; }
        public long TradingRuleId { get; set; }
        public long DepositRuleId { get; set; }
        public long SymbolId { get; set; }
        public long PerformanceSetupId { get; set; }
        public SearchDetail()
        {
        }
    }
}

