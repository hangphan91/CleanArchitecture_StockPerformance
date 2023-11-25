using System;
namespace EntityDefinitions
{
	public class DepositRule :IdBase
	{
        public int FirstDepositDate { get; set; }
        public int SecondDepositDate { get; set; }
        public int NumberOfDepositDate { get; set; }
        public int DepositAmount { get; set; }
        public int InitialDepositAmount { get; set; }
        public DepositRule()
		{
		}

        
    }
}

