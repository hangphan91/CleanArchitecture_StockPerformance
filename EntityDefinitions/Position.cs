using System;
namespace EntityDefinitions
{
	public class Position :IdBase
	{
        public DateTime Date { get; set; }
        public decimal ShareCount { get; set; }
        public decimal Price { get; set; }
		public PositionType PositionType { get; set; }
		public long PerfomanceSummaryId { get; set; }
		public Position()
		{
		}
	}
}

