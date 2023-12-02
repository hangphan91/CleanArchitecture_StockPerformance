using System;
namespace EntityDefinitions
{
	public class PerformanceSetup :IdBase
	{
		public DateOnly StartingYear { get; set; }
		public DateOnly EndingYear { get; set; }
		public long StartingSymbolId { get; set; }
		public long EndingSymbolId { get; set; }
		public PerformanceSetup()
		{
		}
	}
}

