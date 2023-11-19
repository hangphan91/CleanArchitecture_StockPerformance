using StockPerformanceCalculator.Interfaces;
using StockPerformanceCalculator.Logic.Models;
using StockPerformanceCalculator.Logic.TradingRules;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class TradeCalculator : IBuyStock, ISellStock
    {
        private static decimal _oneTimePurchaseLimitation = TradingRule.GetPurchaseLimitation();
        private StockLedgerDetail? _stockLedgerDetail;
        private StockLedgerCalculator _stockLedgerCalculator;
        private AvailableBalanceCalculator _availableBalanceCalculator;
        private ShareNumberCalculator _shareNumberCalculator;
        private TradeDetailCalculator _tradeDetailCalculator;


        public TradeCalculator(
            StockLedgerCalculator stockLedgerCalculator,
            AvailableBalanceCalculator availableBalanceCalculator,
            ShareNumberCalculator shareNumberCalculator)
        {
            _stockLedgerCalculator = stockLedgerCalculator;
            _availableBalanceCalculator = availableBalanceCalculator;
            _shareNumberCalculator = shareNumberCalculator;
        }

        public void Buy()
        {
            if (_stockLedgerDetail != null)
            {
                _stockLedgerCalculator.AddBoughtLedger(_stockLedgerDetail);
                var toSubtractBalance = _stockLedgerDetail.GetCost();
                _availableBalanceCalculator.DeductBalance(toSubtractBalance, _stockLedgerDetail.Date);
            }
        }

        public void Sell()
        {
            if (_stockLedgerDetail != null)
            {
                var currentSoldPrice = _stockLedgerDetail.Price;
                var shareCount =_shareNumberCalculator.GetHoldingShare();
                _stockLedgerCalculator.RemoveSoldLedgers(currentSoldPrice, _stockLedgerDetail.Date);

                _availableBalanceCalculator.AddBalance(shareCount * currentSoldPrice, _stockLedgerDetail.Date);
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

            _stockLedgerDetail = new StockLedgerDetail
            {
                Date = tradingDate,
                Price = currentPrice,
                ShareCount = shareCount,
            };

            if (TradingRule.IsValidForBuyingRule(currentHoldingValue, basicCost))
                Buy();

            if (TradingRule.IsValidForSellingRule(currentHoldingValue, basicCost))
                Sell();
        }

        private decimal GetTradingCash(decimal availableCash)
        {
            if (_oneTimePurchaseLimitation >= availableCash)
                return availableCash;

            return _oneTimePurchaseLimitation;
        }
    }
}

