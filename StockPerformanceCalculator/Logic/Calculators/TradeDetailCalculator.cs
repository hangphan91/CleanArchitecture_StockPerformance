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
        private DepositRule _depositRule;

        public TradeDetailCalculator(StockLedgerCalculator stockLedgerCalculator,
            PriceCalculator priceCalculator, TradingRule tradingRule,
            DateDetail startDate, DepositRule depositRule)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _priceCalculator = priceCalculator;
            _startDate = startDate;
            _tradingRule = tradingRule;
            _depositRule = depositRule;
        }

        public List<TradeDetail> GetTradeDetails(List<SymbolSummary> stockSummaries)
        {
            var numberOfYear = DateTime.Now.Year - _startDate.Year;
            var tradeDetails = new List<TradeDetail>();
            var startDate = new DateTime(_startDate.Year, _startDate.Month, _startDate.Day);

            var dates = new List<DateOnly>();
            for (DateTime date = startDate; date <= DateTime.Now; date = date.AddDays(1))
            {
                dates.Add(new DateOnly(date.Year, date.Month, date.Day));
            }

            foreach (var date in dates)
            {
                var isTradingDate1 = _depositRule.GetFirstDepositDate() == date.Day;
                var isTradingDate2 = _depositRule.GetSecondDepositDate() == date.Day;

                var toTradeStocks = stockSummaries
                               .Where(summary => summary.Date.Month == date.Month
                               && summary.Date.Year == date.Year &&
                               _tradingRule.IsValidToTradeStockByDate(summary.Date.Date))
                               .OrderBy(a => a.Date.Date);

                SymbolSummary? toTradeStock = null;
                var numberOfTradeAMonth = _tradingRule.GetNumberOfTrade();

                if (isTradingDate2 && numberOfTradeAMonth >=2)
                    toTradeStock = toTradeStocks
                        .FirstOrDefault(a => a.Date.Day >= _depositRule.GetSecondDepositDate());

                if (isTradingDate1)
                    toTradeStock = toTradeStocks
                        .FirstOrDefault(a => a.Date.Day >= _depositRule.GetFirstDepositDate());
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

            return tradeDetails;
        }
    }
}

