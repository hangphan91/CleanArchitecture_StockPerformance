using StockPerformanceCalculator.Interfaces;
using StockPerformanceCalculator.Logic.Models;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class TradeCalculator : IBuyStock, ISellStock
    {
        private static decimal _oneTimePurchaseLimitation = TradingRule.GetPurchaseLimitation();
        private StockLedgerCalculator _stockLedgerCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;

        public TradeCalculator(
            StockLedgerCalculator stockLedgerCalculator,
            AvailableBalanceCalculator availableBalanceCalculator,
            ShareNumberCalculator shareNumberCalculator)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _availableBalanceCalculator = availableBalanceCalculator;
            _shareNumberCalculator = shareNumberCalculator;
        }

        public void Buy(StockLedgerDetail stockLedgerDetail)
        {
            if (stockLedgerDetail != null)
            {
                _stockLedgerCalculator.AddBoughtLedger(stockLedgerDetail);
                var toSubtractBalance = stockLedgerDetail.GetCost();
                _availableBalanceCalculator.DeductBalance(toSubtractBalance, stockLedgerDetail.Date);
            }
        }

        public void Sell(StockLedgerDetail stockLedgerDetail)
        {
            if (stockLedgerDetail != null)
            {
                var currentSoldPrice = stockLedgerDetail.Price;
                var shareCount = _shareNumberCalculator.GetHoldingShare();
                _stockLedgerCalculator.RemoveSoldLedgers(currentSoldPrice, stockLedgerDetail.Date);

                _availableBalanceCalculator.AddBalance(shareCount * currentSoldPrice, stockLedgerDetail.Date);
            }
        }

        public void ImplementTrade(TradeDetail tradeDetail)
        {
            var tradingDate = tradeDetail.TradingDate;
            var currentPrice = tradeDetail.CurrentPrice;
            var currentHoldingValue = tradeDetail.CurrentHoldingValue;
            var basicCost = tradeDetail.BasicCost;

            var availableCash = _availableBalanceCalculator.Calculate(tradingDate);
            var tradingCash = GetTradingCash(availableCash);
            var shareCount = _shareNumberCalculator.CountShare(currentPrice, tradingCash);

            var stockLedgerDetail = new StockLedgerDetail
            {
                Date = tradingDate,
                Price = currentPrice,
                ShareCount = shareCount,
            };

            if (TradingRule.IsValidForBuyingRule(currentHoldingValue, basicCost))
                Buy(stockLedgerDetail);

            else if (TradingRule.IsValidForSellingRule(currentHoldingValue, basicCost))
                Sell(stockLedgerDetail);
        }

        private decimal GetTradingCash(decimal availableCash)
        {
            if (_oneTimePurchaseLimitation >= availableCash)
                return availableCash;

            return _oneTimePurchaseLimitation;
        }
    }
}

