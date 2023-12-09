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
            str.AppendLine($"Deposit rule as:");
            str.AppendLine($"We deposit on {FirstDepositDate} and {SecondDepositDate} of the month.");
            str.AppendLine($" One time deposit ${InitialDepositAmount} and repeated deposit monthly ${DepositAmount}.");
            return str.ToString();
        }

        internal bool IsSame(DepositRule depositRule)
        {
            if (depositRule == null)
                return false;

            return depositRule.FirstDepositDate == FirstDepositDate
                && depositRule.SecondDepositDate == SecondDepositDate
                && depositRule.NumberOfDepositDate == NumberOfDepositDate
                && depositRule.DepositAmount == DepositAmount
                && depositRule.InitialDepositAmount == InitialDepositAmount;
        }
    }
}

