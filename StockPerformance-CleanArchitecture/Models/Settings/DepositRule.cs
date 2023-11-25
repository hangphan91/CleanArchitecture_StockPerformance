using System;
namespace StockPerformance_CleanArchitecture.Models.Settings
{
	public class DepositRule
	{
        public int FirstDepositDate { get; set; }
        public int SecondDepositDate { get; set; }
        public int NumberOfDepositDate { get; set; }
        public int DepositAmount { get; set; }
        public int InitialDepositAmount { get; set; }

        public DepositRule()
		{
		}
	}
}

