using System;
namespace EntityDefinitions
{
    public class PerformanceSummary : IdBase
    {
        public decimal ProfitInDollar { get; set; }
        public decimal ProfitInPercentage { get; set; }
        public string ProfitCurrency { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalBalanceInPosition { get; set; }
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public PerformanceSummary()
        {
        }
    }
}

