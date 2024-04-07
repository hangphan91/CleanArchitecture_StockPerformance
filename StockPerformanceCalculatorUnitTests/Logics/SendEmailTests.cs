using System.Net;
using System.Net.Mail;
using EntityDefinitions;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.EmailDetails;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformanceCalculator.Models;
using static System.Net.WebRequestMethods;

namespace UnitTests.Logics
{
    [TestClass]
    public class SendEmailTests
    {
        [TestMethod]
        public void SendEmail()
        {

            var dictionary = new List<StockPerformanceResponse>
            {
                new StockPerformanceResponse
                {
                    StartDate = new DateDetail(2022, 11, 20),
                    Symbol = "AAPL",
                    ProfitSummaryPercentage = new ProfitSummaryPercentage(),
                }
            };
            var email = new EmailContact
            {
                EmailAddress = "funnyluv122@gmail.com",
                FirstName = "Amor"
            };
            var emailMessage = SendEmailEngine.GenerateEmail(dictionary, email, true);
            var success = SendEmailEngine.SendEmail(emailMessage);
            Assert.AreEqual(success, true);
        }

        [TestMethod]
        public void SendEmails()
        {

            var dictionary = new List<StockPerformanceResponse>
            {
                new StockPerformanceResponse
                {
                    StartDate = new DateDetail(2021, 11, 20),
                    Symbol = "AAPL",
                    ProfitSummaryPercentage = new ProfitSummaryPercentage{
                        MAXMonthlyProfit = (decimal)1.33333,
                        MAXYearlyProfit = (decimal)13.32323
                    },
                },
                new StockPerformanceResponse
                {
                    StartDate = new DateDetail(2022, 11, 20),
                    Symbol = "ARCB",
                    ProfitSummaryPercentage = new ProfitSummaryPercentage{
                        MAXMonthlyProfit = (decimal)1.33333,
                        MAXYearlyProfit = (decimal)13.32323
                    },
                }
            };

            var email = new EmailContact
            {
                EmailAddress = "funnyluv122@gmail.com",
                FirstName = "Amor"
            };
            SendEmailEngine.CreateAndSendEmail(dictionary, new List<EmailContact> { email }, false);
        }
    }
}

