using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class MonthlyGrowthSpeed : GrowthSpeed
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public MonthlyGrowthSpeed()
		{
		}
        public override string DisplayGrowthSpeed()
        {
            return $"In {Month}/{Year} " +base.DisplayGrowthSpeed();
        }
    }
}

