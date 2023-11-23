using System;
namespace EntityDefinitions
{
	public class PerformanceIdHub :IdBase
	{
		public long DepositRuleId { get; set; }
		public long PerformanceId { get; set; }
		public  long PerformanceSetupId { get; set; }
		public long SymbolId { get; set; }
		public long TradingRuleId { get; set; }
		public PerformanceIdHub()
		{
				
		}
	}
}

