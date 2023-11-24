using StockPerformanceCalculator.ExternalCommunications;

namespace StockPerformanceCalculatorUnitTests.ExternalCommunications
{
    [TestClass]
	public class TestYahooFinanceAPIMapper
	{
		[TestMethod]
		public async void UseMockCaller_TestMapping()
		{
			//Arrange

			var symbol = "AAPL";
			var date = DateTime.Now.AddYears(-3);

			//Act
			var summaries = await (new MockYahooFinanceCaller().GetStockHistory(symbol, date));
			//Assert

			Assert.IsTrue(summaries.Count > 0);
		}
	}
}

