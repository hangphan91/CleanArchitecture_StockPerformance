
using ExternalCommunicator.ExternalCommunications;
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
        public async Task TestEnterOption()
        {
            var result = await OptionPerformanceManager.GetStrategies();
            var openOptions = await OptionPerformanceManager.EnterOptions(result);

            Assert.IsNotNull(openOptions);
            Assert.IsTrue(openOptions.Count > 0);
        }

        [TestMethod]
        public async Task TestExitOption()
        {
            var result = await OptionPerformanceManager.GetStrategies();
            var exitOptions = await OptionPerformanceManager.ExitOptions(result);
            Assert.IsNotNull(exitOptions);
            Assert.IsTrue(exitOptions.Count > 0);
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

            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.Any());
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().StrikePrice > 0);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().DailyMovingAverage > 0);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().ExpirationDate > DateOnly.MinValue);
            Assert.IsTrue(batchedOptionsDatas.OptionsDatas.First().OptionPremium > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(batchedOptionsDatas.OptionsDatas.First().OptionName));
        }

        [TestMethod]
        public async Task TestGetOptionsData2()
        {
            await new GetOptionsDataAccessor().GetOptionData2("Amzn");
        }
    }
}