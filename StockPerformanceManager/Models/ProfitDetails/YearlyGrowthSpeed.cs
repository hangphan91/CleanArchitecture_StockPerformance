using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
	public class YearlyGrowthSpeed : GrowthSpeed
	{
		public int Year { get; set; }
		public YearlyGrowthSpeed()
		{
		}
        public override string DisplayGrowthSpeed()
        {
            return $"In {Year} " + base.DisplayGrowthSpeed();
        }
    }
}

