using System;
namespace StockPerformanceCalculator.Models
{
    public class StockLedger
    {
        public List<StockLedgerDetail> BoughtLedgers { get; set; }

        public List<StockLedgerDetail> SoldLedgers { get; set; }

        public List<StockLedgerDetail> HoldingLedgers { get; set; }

        public List<StockLedgerDetail> ClosedLedgers { get; set; }
    
        public List <StockLedgerDetail> All { get; set; }

        public StockLedger()
        {
            BoughtLedgers = new List<StockLedgerDetail>();
            SoldLedgers = new List<StockLedgerDetail>();
            HoldingLedgers = new List<StockLedgerDetail>();
            ClosedLedgers = new List<StockLedgerDetail>();
            All = new List<StockLedgerDetail>();
        }        
    }
}

