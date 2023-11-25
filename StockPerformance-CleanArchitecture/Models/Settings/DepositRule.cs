using System;
using System.Text;

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
        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine($"First deposit date {FirstDepositDate}, second deposit date: {SecondDepositDate}");
            str.AppendLine($"Number of Deposit Date {NumberOfDepositDate}");
            str.AppendLine($"Repeated deposit amount: {DepositAmount}. Initial deposit amount: {InitialDepositAmount}");
            return str.ToString();
        }
    }
}

