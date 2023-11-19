using System;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculator.Logic
{
    public class StockPerformanceSummaryCalculator
    {
        private string _symbol;
        private int _year;
        private decimal _currentPrice;
        private ProfitCalculator _profitCalculator;
        private GrowthRateCalculator _growthRateCalculator;
        private PriceCalculator _priceCalculator;
        private StockLedgerCalculator _stockLedgerCalculator;
        DepositLedgerCalculator _depositLedgerCalculator;


        public StockPerformanceSummaryCalculator(string symbol, int year,
            PriceCalculator priceCalculator,
            StockLedgerCalculator stockLedgerCalculator,
            DepositLedgerCalculator depositLedgerCalculator)
        {
            _symbol = symbol;
            _year = year;
            _priceCalculator = priceCalculator;
            _depositLedgerCalculator = depositLedgerCalculator;
            _stockLedgerCalculator = stockLedgerCalculator;
            _profitCalculator = new ProfitCalculator(_stockLedgerCalculator);
            _growthRateCalculator = new GrowthRateCalculator();
        }

        internal StockPerformanceSummary Calculate()
        {
            _currentPrice = _priceCalculator.GetCurrentPrice();
            var result = new StockPerformanceSummary
            {
                Symbol = _symbol,
                Year = _year,
                CurrentPrice = _currentPrice,
            };

            _profitCalculator.Calculate(_currentPrice);

            result.ProfitByYears = _profitCalculator.GetProfitByYear();
            result.ProfitByMonths = _profitCalculator.GetProfitByMonth();

            _growthRateCalculator.Calculate(result.ProfitByYears, result.ProfitByMonths);

            result.GrowthSpeedByMonths = _growthRateCalculator.GetGrowthRateByMonth();
            result.GrowthSpeedByYears = _growthRateCalculator.GetGrowthRateByYear();

            return result;
        }
    }
}

