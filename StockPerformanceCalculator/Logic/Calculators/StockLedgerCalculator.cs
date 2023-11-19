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

        internal int GetCurrentHoldingShare()
        {
            return _stockLedger.HoldingLedgers
                .Sum(holding => holding.ShareCount);
        }

        internal decimal GetCostBasicForCurrentHolding()
        {
            return _stockLedger.HoldingLedgers
                .Sum(holding => holding.ShareCount * holding.Price);
        }

        public void AddBoughtLedger(StockLedgerDetail detail)
        {
            _stockLedger.BoughtLedgers.Add(detail);
            _stockLedger.HoldingLedgers.Add(detail);
        }

        public void RemoveSoldLedgers(decimal currentSoldPrice, DateTime date)
        {
            var soldLedgers =_stockLedger.BoughtLedgers.Select(ledger => new StockLedgerDetail
            {
                Date = date,
                Price = currentSoldPrice,
                ShareCount = ledger.ShareCount,
            });

            _stockLedger.SoldLedgers.AddRange(soldLedgers);
            _stockLedger.HoldingLedgers.Clear();
        }

        public decimal GetCurrentHoldingValue(decimal price)
        {
            return GetCurrentHoldingShare() * price;
        }

        public StockLedger GetStockLedger()
        {
            return _stockLedger;
        }
    }
}

