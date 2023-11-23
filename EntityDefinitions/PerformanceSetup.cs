using System;
namespace EntityDefinitions
{
	public class PerformanceSetup :IdBase
	{
		public int StartingYear { get; set; }
		public int EndingYear { get; set; }
		public long StartingSymbolId { get; set; }
		public long EndingSymbolId { get; set; }
		public PerformanceSetup()
		{
		}
	}
}

