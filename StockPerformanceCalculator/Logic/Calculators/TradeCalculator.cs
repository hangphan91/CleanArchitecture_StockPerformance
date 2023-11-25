using StockPerformanceCalculator.Interfaces;
using StockPerformanceCalculator.Logic.Models;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class TradeCalculator : IBuyStock, ISellStock
    {
        private StockLedgerCalculator _stockLedgerCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        private TradingRule _tradingRule;
        private decimal _averagePrice;
        private decimal _totalShareCount;

        public TradeCalculator(
            StockLedgerCalculator stockLedgerCalculator,
            AvailableBalanceCalculator availableBalanceCalculator,
            ShareNumberCalculator shareNumberCalculator,
            TradingRule tradingRule)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _availableBalanceCalculator = availableBalanceCalculator;
            _shareNumberCalculator = shareNumberCalculator;
            _tradingRule = tradingRule;
        }

        public void Buy(StockLedgerDetail stockLedgerDetail)
        {
            if (stockLedgerDetail != null)
            {
                _stockLedgerCalculator.AddBoughtLedger(stockLedgerDetail);
                var toSubtractBalance = stockLedgerDetail.GetCost();
                _availableBalanceCalculator.DeductBalance(toSubtractBalance, stockLedgerDetail.BoughtDate);
            }
        }

        public void Sell(StockLedgerDetail stockLedgerDetail)
        {
            if (stockLedgerDetail != null)
            {
                var currentSoldPrice = stockLedgerDetail.BoughtPrice;
                var shareCount = _shareNumberCalculator.GetHoldingShare();
                _stockLedgerCalculator.RemoveSoldLedgers(currentSoldPrice, stockLedgerDetail.BoughtDate);

                _availableBalanceCalculator.AddBalance(shareCount * currentSoldPrice, stockLedgerDetail.BoughtDate);
            }
        }

        public void ImplementTrade(TradeDetail tradeDetail)
        {
            var tradingDate = tradeDetail.TradingDate;
            var currentPrice = tradeDetail.CurrentPrice;
            var aboutToTradePrice = tradeDetail.AboutToTradePrice;

            var availableCash = _availableBalanceCalculator.Calculate(tradingDate);
            var tradingCash = GetTradingCash(availableCash);
            var shareCount = _shareNumberCalculator.CountShare(currentPrice, tradingCash);

            var currentLoss = _totalShareCount * (currentPrice - _averagePrice);
            var stockLedgerDetail = new StockLedgerDetail
            {
                BoughtDate = tradingDate,
                BoughtPrice = currentPrice,
                ShareCount = shareCount,
            };

            if (_tradingRule.IsValidForBuyingRule(aboutToTradePrice, _averagePrice)
                && availableCash > 0)
            {
                _averagePrice = (currentPrice * shareCount + _averagePrice * _totalShareCount) /
                    (_totalShareCount + shareCount);
                _totalShareCount += shareCount;

                Buy(stockLedgerDetail);
            }

            else if ((_tradingRule.IsValidForSellingRule(aboutToTradePrice, _averagePrice)
                && _stockLedgerCalculator.GetTotalShareHoldingLedgers() > 0)
                || _tradingRule.IsLostMoreThanLimitation(currentLoss))
            {
                _totalShareCount = 0;
                _averagePrice = 0;
                Sell(stockLedgerDetail);
            }

        }

        private decimal GetTradingCash(decimal availableCash)
        {
            var oneTimePurchaseLimitation = _tradingRule.GetPurchaseLimitation();
            if (oneTimePurchaseLimitation >= availableCash)
                return availableCash;

            return oneTimePurchaseLimitation;
        }
    }
}

