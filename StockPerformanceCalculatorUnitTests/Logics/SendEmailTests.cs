using System.Net;
using System.Net.Mail;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
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

            MailMessage emailMessage;
            bool success;
            EntityDefinitions.Email email = new EntityDefinitions.Email
            {
                EmailAddress = "funnyluv122@gmail.com",
                FistName = "Amor"
            };
            SendEmailEngine.GenerateEmail(dictionary, email, out emailMessage, out success);
            success = SendEmailEngine.SendEmail(emailMessage);
            Assert.AreEqual(success, true);
        }

        [TestMethod]
        public void SendEmails()
        {

            var dictionary = new List<StockPerformanceResponse>
            {
                new StockPerformanceResponse
                {
                    StartDate = new DateDetail(2022, 11, 20),
                    Symbol = "AAPL",
                    ProfitSummaryPercentage = new ProfitSummaryPercentage{
                        MAXMonthlyProfit = (decimal)1.33333,
                        MAXYearlyProfit = (decimal)13.32323
                    },
                }
            };

            EntityDefinitions.Email email = new EntityDefinitions.Email
            {
                EmailAddress = "funnyluv122@gmail.com",
                FistName = "Amor"
            };
            SendEmailEngine.CreateAndSendEmail(dictionary, new List<EntityDefinitions.Email> { email });
        }
    }
}

