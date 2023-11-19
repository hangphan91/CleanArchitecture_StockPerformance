using StockPerformanceCalculator.ExternalCommunications;

namespace StockPerformanceCalculatorUnitTests.ExternalCommunications
{
    [TestClass]
	public class TestYahooFinanceAPIMapper
	{
		[TestMethod]
		public void UseMockCaller_TestMapping()
		{
			//Arrange

			var symbol = "AAPL";
			var year = 2020;

			//Act
			var summaries = new MockYahooFinanceCaller().GetStockHistory(symbol, year);
			var mappedSummaries = YahooFinanceAPIMapper.Map(summaries);
			//Assert

			Assert.IsTrue(summaries.Count > 0);
			Assert.AreEqual(summaries.Count, mappedSummaries.Count);
		}
	}
}

