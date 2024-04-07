using EntityPersistence.DataAccessors;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformanceCalculator.Models;

namespace StockPerformanceCalculatorUnitTests
{
    [TestClass]
	public class TestStockPerformanceManager
	{
		[TestMethod]
		public async Task GivenMockHistoryPrice_TestStockPerformanceManagerLogic()
		{
            //Arrange

            var symbol = "AAPL";
            var year = 2020;
			var context = new DataContext();
			var accessor = new PerformanceDataAccessor(context);
			var date = new DateDetail(2020, DateTime.Now.Month, DateTime.Now.Day);
            var manager = new MockStockPerformanceManager(symbol, date, accessor);

			//Act
			var searchDetail = SearchDetailHelper.GetCurrentSearchDetail(accessor);
            var mapped = SearchDetailHelper.Map(searchDetail);
            var summaries = await manager.StartStockPerforamanceCalculation(mapped);

			//Assert

			Assert.IsTrue(summaries != null);
			Assert.IsFalse(summaries.CurrentPrice == 0);
			Assert.AreEqual(summaries.Symbol, symbol);
			Assert.AreEqual(summaries.StartDate.Year, year);

			Assert.IsTrue(summaries.ProfitByYears.Any());
            Assert.IsTrue(summaries.ProfitByMonths.Any());
            Assert.IsTrue(summaries.GrowthSpeedByMonths.Any());
			Assert.IsTrue(summaries.GrowthSpeedByYears.Any());


        }
    }
}

