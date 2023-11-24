using System;
namespace EntityDefinitions
{
	public class PerformanceByMonth: IdBase
	{
		public int Year { get; set; }

        public int Month { get; set; }

        public decimal Profit { get; set; }

		public long PerformanceSummaryId { get; set; }

		public PerformanceByMonth()
		{
		}
	}
}

