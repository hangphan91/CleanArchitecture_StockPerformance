using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class StockLedgerCalculator
    {
        private StockLedger _stockLedger;
        public StockLedgerCalculator()
        {
            _stockLedger = new StockLedger();
        }

        internal decimal GetCurrentHoldingShare()
        {
            return _stockLedger.HoldingLedgers
                .Sum(holding => holding.ShareCount);
        }

        internal decimal GetAveragePriceForAllShares()
        {
            var sum = _stockLedger.HoldingLedgers
                .Sum(holding => holding.ShareCount * holding.BoughtPrice);
            var shares = _stockLedger.HoldingLedgers.Sum(a => a.ShareCount);
            if (shares == 0)
                return 0;
            return sum / shares;
        }

        public void AddBoughtLedger(StockLedgerDetail detail)
        {
            detail.PositionType = EntityDefinitions.PositionType.Bought;
            _stockLedger.BoughtLedgers.Add(detail);
            _stockLedger.HoldingLedgers.Add(detail);
        }

        public void RemoveSoldLedgers(decimal currentSoldPrice, DateTime date)
        {
            var soldLedgers = _stockLedger.HoldingLedgers.Select(ledger => new StockLedgerDetail
            {
                SoldPrice = currentSoldPrice,
                ShareCount = ledger.ShareCount,
                SoldDate = date,
            });

            var closedLedgers = _stockLedger.BoughtLedgers
                .Join(soldLedgers,
                bought => bought.ShareCount,
                sold => sold.ShareCount,
                (bought, sold) => new StockLedgerDetail
                {
                    BoughtDate = bought.BoughtDate,
                    BoughtPrice = bought.BoughtPrice,
                    PositionType = EntityDefinitions.PositionType.Closed,
                    ShareCount = bought.ShareCount,
                    SoldPrice = sold.SoldPrice,
                    SoldDate = sold.SoldDate,
                });

            _stockLedger.ClosedLedgers.AddRange(closedLedgers.ToList());
            _stockLedger.SoldLedgers.AddRange(closedLedgers);
            _stockLedger.HoldingLedgers.Clear();
        }

        public decimal GetCurrentHoldingValue(decimal price)
        {
            return GetCurrentHoldingShare() * price;
        }

        public StockLedger GetStockLedger()
        {
            _stockLedger.BoughtLedgers.ForEach(a => a.PositionType = EntityDefinitions.PositionType.Bought);
            _stockLedger.SoldLedgers.ForEach(a => a.PositionType = EntityDefinitions.PositionType.Sold);

            _stockLedger.All.AddRange(_stockLedger.BoughtLedgers);
            _stockLedger.All.AddRange(_stockLedger.SoldLedgers);
            return _stockLedger;
        }

        internal decimal GetTotalShareHoldingLedgers()
        {
            return _stockLedger.HoldingLedgers
               .Sum(l => l.ShareCount);
        }
    }
}

