﻿using System;
namespace StockPerformanceCalculator.Models.PerformanceCalculatorSetup
{
	public class InitialPerformanceSetup
	{
        public DateOnly StartingYear { get; set; }
        public DateOnly EndingYear { get; set; }
		public List<string> Symbols { get; set; }
        public decimal PurchaseLimitation { get; set; }
        public decimal SellPercentageLimitation { get; set; }
        public decimal LossLimitation { get; set; }
        public decimal BuyPercentageLimitation { get; set; }
        public int NumberOfTradeAMonth { get; set; }
        public int LowerRangeOfTradingDate { get; set; }
        public int HigherRangeOfTradingDate { get; set; }
        public int FirstDepositDate { get; set; }
        public int SecondDepositDate { get; set; }
        public int NumberOfDepositDate { get; set; }
        public int DepositAmount { get; set; }
        public int InitialDepositAmount { get; set; }
        public decimal SellAllWhenPriceDropAtPercentageSinceLastTrade { get; set; }


        public InitialPerformanceSetup()
		{
			Symbols = new List<string>();
		}
	}
}

