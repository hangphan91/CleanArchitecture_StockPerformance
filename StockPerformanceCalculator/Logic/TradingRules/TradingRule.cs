namespace StockPerformanceCalculator.Logic.TradingRules
{
    public class TradingRule
    {
        private EntityEngine _entityEngine;

        public TradingRule(EntityEngine entityEngine)
        {
            _entityEngine = entityEngine;
        }

        public decimal GetPurchaseLimitation()
        {
            var initialSetup = _entityEngine.GetInitialSetup();
            return initialSetup.PurchaseLimitation;
        }
        public bool IsValidForBuyingRule(decimal aboutToTradePrice, decimal averagePrice)
        {
            // Buy stock on the available date, then continue to buy
            // When Stock Gain 5% overall

            var initialSetup = _entityEngine.GetInitialSetup();
            var buyPercentage = initialSetup.BuyPercentageLimitation;

            if (aboutToTradePrice >= averagePrice * (decimal)buyPercentage/100)
                return true;

            return false;
        }

        public bool IsValidForSellingRule(decimal aboutToTradePrice, decimal averagePrice)
        {
            //Sell stock when losing 7% overall
            var initialSetup = _entityEngine.GetInitialSetup();
            var sellPercentage = initialSetup.SellPercentageLimitation;
            if (aboutToTradePrice <= averagePrice * (decimal)sellPercentage/100)
                return true;

            return false;
        }

        public bool IsValidToTradeStockByDate(DateTime date)
        {
            //only buy stock from day 1 to day 10 of each month
            var initialSetup = _entityEngine.GetInitialSetup();

            var lowerDate = initialSetup.LowerRangeOfTradingDate;
            var higherDate = initialSetup.HigherRangeOfTradingDate;

            if (date.Day >= lowerDate && date.Day <= higherDate)
                return true;

            return false;
        }

        internal int GetNumberOfTrade()
        {
            var initialSetup = _entityEngine.GetInitialSetup();
            return initialSetup.NumberOfTradeAMonth;
        }

        internal bool IsLostMoreThanLimitation(decimal currentLoss)
        {
            var initialSetup = _entityEngine.GetInitialSetup();
            return initialSetup.LossLimitation < Math.Abs(currentLoss) && currentLoss < 0;
        }
    }
}

