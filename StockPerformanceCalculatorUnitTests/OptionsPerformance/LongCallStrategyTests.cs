
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptionPerformance;
using OptionPerformance.DataAccessors;

namespace UnitTests.OptionPerformance.UnitTests
{
    [TestClass]
    public class LongCallStrategyTest
    {
        [TestMethod]
        public async Task TestGetStrategies()
        {
            var result = await OptionPerformanceManager.GetStrategies();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }


        [TestMethod]
        public async Task TestGetOptionsData()
        {
            var batchedOptionsDatas = await OptionDataAccessor.GetOptionsData("AAPL");

            Assert.IsNotNull(batchedOptionsDatas);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.Count > 0);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().YearlyReturn > 0);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().MonthlyReturn > 0);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().StockPrice > 0);
        }
    }
}