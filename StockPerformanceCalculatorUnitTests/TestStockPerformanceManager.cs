using System;
using StockPerformanceCalculator.Logic;

namespace StockPerformanceCalculatorUnitTests
{
	[TestClass]
	public class TestStockPerformanceManager
	{
		[TestMethod]
		public void GivenMockHistoryPrice_TestStockPerformanceManagerLogic()
		{
            //Arrange

            var symbol = "AAPL";
            var year = 2020;

			var manager = new MockStockPerformanceManager(symbol, year);

			//Act
			var summaries = manager.StartStockPerforamanceCalculation();

			//Assert

			Assert.IsTrue(summaries != null);
			Assert.IsFalse(summaries.CurrentPrice == 0);
			Assert.AreEqual(summaries.Symbol, symbol);
			Assert.AreEqual(summaries.Year, year);

			Assert.IsTrue(summaries.ProfitByYears.Any());
            Assert.IsTrue(summaries.ProfitByMonths.Any());
            Assert.IsTrue(summaries.GrowthSpeedByMonths.Any());
			Assert.IsTrue(summaries.GrowthSpeedByYears.Any());


        }
    }
}

