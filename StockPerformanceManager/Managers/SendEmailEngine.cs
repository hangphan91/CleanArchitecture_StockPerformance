using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using EntityDefinitions;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.EmailDetails;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailEngine
    {
        public static void CreateAndSendEmail(List<StockPerformanceResponse> responses,
            List<EmailContact> emailsTosend,
            bool isProfitable)
        {
            foreach (var email in emailsTosend)
            {
                var emailMessage = GenerateEmail(responses.Distinct().ToList(), email, isProfitable);

                if (isProfitable)
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

        public static MailMessage GenerateEmail(List<StockPerformanceResponse> list,
            EmailContact email, bool success)
        {
            var htmlHead = "<!DOCTYPE html>\n<html>\n<head>\n  <title></title>\n  <meta charset=\"UTF-8\">\n</head>\n<body>";
            string tableMessage = PerformanceResultFormatter.GetStockPerformanceResponseTableHTML(list);

            var direction = success ? "growth" : "degration";
            var emailStartMessage = $@"Dear {email.FirstName}, " +
                $"<br>These are the stock performance reports for {DateTime.Now.ToLongDateString()}. " +
                $"Our analysis highlights the following stocks that have shown {direction} in recent years:";

            var url = "https://stockperformance.azurewebsites.net";
            var emailEndSentence = $"<br>Thanks,<br> StockPerformance<br><br>Visit us at: <a href={url}>Stock Perfomance Site</a> ";

            var fullMessage = htmlHead + emailStartMessage + tableMessage + emailEndSentence + "\n</body>\n</html>";

            var emailMessage = new MailMessage(
                        "stockperformance2023@gmail.com",
                        email.EmailAddress,
                        $"Stock Performance Report on {DateTime.Now.ToLongDateString()}",
                        body: fullMessage);

            emailMessage.IsBodyHtml = true;

            return emailMessage;
        }

        public static List<SendEmailData> GetToSendEmailList(
            List<StockPerformanceResponse> stockPerformanceResponses,
            List<Email> emails)
        {
            var responses = stockPerformanceResponses.OrderByDescending(stock => stock.ProfitInPercentage);
            
             //uhaphan - Here is non profit List
            var allResponses = responses
                .Select(a => a).Distinct().ToList();
            
            // Hang - gaining list
            var gainingProfitResponses = responses
                 .Where(response => response.ProfitSummaryPercentage.IsProfitable())
                 .Select(a => a)
                 .Distinct().ToList();

            var emailsTosend = emails.Select(a =>
            new EmailContact
            {
                EmailAddress = a.EmailAddress,
                FirstName = a.FirstName,
                LastName = a.LastName,
            })
            .Distinct().ToList();

            var debugEmails = emailsTosend.Where(email => email.FirstName.Equals("Love")).ToList();

#if DEBUG
            foreach (var item in debugEmails)
            {
                item.FirstName = "Test";
            }

            emailsTosend = debugEmails ;
#endif

            int minCount = 1;
            var sendEmailGainData = new SendEmailData(gainingProfitResponses, minCount, emailsTosend, true);

            var emailsTosendForLostProfit = debugEmails;
            var sendEmailLostData = new SendEmailData(allResponses, 1, emailsTosendForLostProfit, true);

            var sendEmailData = new List<SendEmailData> { sendEmailGainData, sendEmailLostData };
            return sendEmailData;
        }
    }
}

