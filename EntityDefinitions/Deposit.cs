using System;
namespace EntityDefinitions
{
    public class Deposit : IdBase
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public long PerformanceSummaryId { get; set; }
        public Deposit()
        {
        }
    }
}

