using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class Profit
	{
        public decimal Amount { get; set; }
        public Profit()
		{
		}
        public virtual string DisplayProfit()
        {
            var amount = Math.Abs(Amount);

            var toDisplay = "Lose";
            if (Amount > 0)
                toDisplay = "Gain";

            toDisplay = $"{toDisplay} {amount}";

            return toDisplay;
        }
    }
}

