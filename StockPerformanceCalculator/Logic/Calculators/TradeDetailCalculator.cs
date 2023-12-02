using StockPerformanceCalculator.Logic.Models;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class TradeDetailCalculator
    {
        private DateDetail _startDate;
        private StockLedgerCalculator _stockLedgerCalculator;
        private PriceCalculator _priceCalculator;
        private TradingRule _tradingRule;

        public TradeDetailCalculator(StockLedgerCalculator stockLedgerCalculator,
            PriceCalculator priceCalculator, TradingRule tradingRule, DateDetail startDate)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _priceCalculator = priceCalculator;
            _startDate = startDate;
            _tradingRule = tradingRule;
        }

        public List<TradeDetail> GetTradeDetails(List<SymbolSummary> stockSummaries)
        {
            var numberOfYear = DateTime.Now.Year - _startDate.Year;
            var tradeDetails = new List<TradeDetail>();
            for (int i = 0; i <= numberOfYear; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    var toTradeStock = stockSummaries
                        .Where(summary => summary.Date.Month == j
                        && summary.Date.Year == _startDate.Year + i &&
                        _tradingRule.IsValidToTradeStockByDate(summary.Date.Date))
                        .FirstOrDefault();

                    if (toTradeStock == null)
                        continue;

                    var currentHoldingShareCount = _stockLedgerCalculator.GetCurrentHoldingShare();
                    var aboutToTradePrice = toTradeStock.ClosingPrice;
                    var averagePrice = _stockLedgerCalculator.GetAveragePriceForAllShares();

                    var tradeDate = toTradeStock.Date;
                    var tradePrice = toTradeStock.ClosingPrice;

                    var tradingDetail = new TradeDetail
                    {
                        PriceAverage = averagePrice,
                        TradingDate = tradeDate,
                        AboutToTradePrice = aboutToTradePrice,
                        CurrentPrice = aboutToTradePrice,
                    };
                    tradeDetails.Add(tradingDetail);
                }
            }

            return tradeDetails;
        }
    }
}

