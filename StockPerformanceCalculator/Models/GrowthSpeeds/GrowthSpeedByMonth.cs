using System;
namespace StockPerformanceCalculator.Models.GrowthSpeeds
{
	public class GrowthSpeedByMonth : GrowthSpeed
	{
		public int Month { get; set; }
		public int Year { get; set; }
		public GrowthSpeedByMonth()
		{
		}
	}
}

