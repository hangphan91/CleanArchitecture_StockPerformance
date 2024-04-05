using System.Net;
using System.Net.Mail;
using EntityDefinitions;
using OoplesFinance.YahooFinanceAPI.Models;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models;
using Utilities;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailEngine
    {
        public static void CreateAndSendEmail(List<StockPerformanceResponse> responses,
            List<EntityDefinitions.Email> emailsTosend)
        {
            foreach (var email in emailsTosend)
            {
                GenerateEmail(responses.Distinct().ToList(), email, out var emailMessage, out var success);
                SendEmail(emailMessage);
            }
        }

        public static bool SendEmail(MailMessage emailMessage)
        {
            bool success;
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                // uhaphan this password was set from gmail account 2 step authentication
                // https://noted.lol/setup-gmail-smtp-sending-2023/#:~:text=People%20assumed%20Google%20was%20disabling,2FA%20and%20Google%20app%20passwords.
                smtp.Credentials =
                        new NetworkCredential("stockperformance2023@gmail.com", "druikmkorfbzcvia");
                smtp.Send(emailMessage);
                success = true;
            }

            return success;
        }

        public static void GenerateEmail(List<StockPerformanceResponse> list,
            EntityDefinitions.Email email, out MailMessage emailMessage, out bool success)
        {
            var htmlHead = "<!DOCTYPE html>\n<html>\n<head>\n  <title></title>\n  <meta charset=\"UTF-8\">\n</head>\n<body>";
            string tableMessage = PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(list);

            var emailStartMessage = $@"Dear {email.FistName}, " +
                $"<br>These are the stock performance reports for {DateTime.Now.ToLongDateString()}. " +
                $"Our analysis highlights the following stocks that have shown growth in recent years:";

            var url = "https://stockperformance.azurewebsites.net";
            var emailEndSentence = $"<br>Thanks,<br> StockPerformance<br><br>Visit us at: <a href={url}>Stock Perfomance Site</a> ";

            var fullMessage = htmlHead + emailStartMessage + tableMessage + emailEndSentence + "\n</body>\n</html>";

            emailMessage = new MailMessage(
                        "stockperformance2023@gmail.com",
                        email.EmailAddress,
                        $"Stock Performance Report on {DateTime.Now.ToLongDateString()}",
                        body: fullMessage);

            emailMessage.IsBodyHtml = true;

            success = true;
        }

        public static (List<StockPerformanceResponse>, List<Email>, int) GetToSendEmailList(
            List<StockPerformanceResponse> stockPerformanceResponses,
            List<Email> emails)
        {
            List<StockPerformanceResponse> responses;
            List<Email> emailsTosend;
            int minCount;

            responses = stockPerformanceResponses
                .Where(response => response.ProfitSummaryPercentage.IsProfitable())
                .Select(a => a).Distinct().ToList();
            emailsTosend = emails.Select(a => a).Distinct().ToList();
            // uhaphan only send email at 5 am and on week day
            minCount = 5;

            return (responses, emailsTosend, minCount);
        }
    }
}

