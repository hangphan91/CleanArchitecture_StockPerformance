using StockPerformanceCalculator.Models.GrowthSpeeds;

namespace StockPerformanceCalculator.Models
{
    public class StockPerformanceSummary
    {
        public List<ProfitByMonth> ProfitByMonths { get; set; }
        public List<ProfitByYear> ProfitByYears { get; set; }
        public List<GrowthSpeedByYear> GrowthSpeedByYears { get; set; }
        public List<GrowthSpeedByMonth> GrowthSpeedByMonths { get; set; }
        public List<DepositLedger> DepositLedgers { get; set; }
        public StockLedger StockLedger { get; set; }
        public decimal Volume { get; set; }
        public string Symbol { get; set; }
        public DateDetail StartDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalBalanceAfterLoss { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalBalanceHoldingInPosition { get; set; }
        public decimal CurrentHoldingShare { get; set; }
        public decimal ProfitInDollar { get; set; }
        public decimal ProfitInPercentage { get; set; }
        public StockPerformanceSummary()
        {
            GrowthSpeedByMonths = new List<GrowthSpeedByMonth>();
            GrowthSpeedByYears = new List<GrowthSpeedByYear>();
            ProfitByMonths = new List<ProfitByMonth>();
            ProfitByYears = new List<ProfitByYear>();
            DepositLedgers = new List<DepositLedger>();
            StockLedger = new StockLedger();
            Symbol = "";
            StartDate = new DateDetail();
            CurrentPrice = 0;
            TotalDeposit = 0;
        }
    }
}

