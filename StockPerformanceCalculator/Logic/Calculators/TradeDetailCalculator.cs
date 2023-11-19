using System;
using StockPerformanceCalculator.Logic.Models;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class TradeDetailCalculator
    {
        private int _year;
        private StockLedgerCalculator _stockLedgerCalculator;
        private PriceCalculator _priceCalculator;

        public TradeDetailCalculator(StockLedgerCalculator stockLedgerCalculator,
            PriceCalculator priceCalculator, int year)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _priceCalculator = priceCalculator;
            _year = year;
        }

        public List<TradeDetail> GetTradeDetails(List<SymbolSummary> stockSummaries)
        {
            var tradeDetails = new List<TradeDetail>();
            for (int i = 0; i < _year; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    var toTradeStock = stockSummaries
                        .Where(summary => summary.Date.Month == j
                        && summary.Date.Year == _year &&
                        TradingRule.IsValidToTradeStockByDate(summary.Date.Date))
                        .FirstOrDefault();

                    if (toTradeStock == null)
                        continue;

                    var currentHoldingShareCount = _stockLedgerCalculator.GetCurrentHoldingShare();
                    var currentPrice = _priceCalculator.GetCurrentPrice(stockSummaries);
                    var holdingValue = currentPrice * currentHoldingShareCount;
                    var costBasicForHoldingValue = _stockLedgerCalculator.GetCostBasicForCurrentHolding();

                    var tradeDate = toTradeStock.Date;
                    var tradePrice = toTradeStock.ClosingPrice;

                    var tradingDetail = new TradeDetail
                    {
                        BasicCost = costBasicForHoldingValue,
                        TradingDate = tradeDate,
                        CurrentHoldingValue = costBasicForHoldingValue,
                        CurrentPrice = currentPrice,
                    };
                    tradeDetails.Add(tradingDetail);
                }
            }

            return tradeDetails;
        }
    }
}

