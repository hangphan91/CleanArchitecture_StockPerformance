using System.Net;
using System.Net.Mail;
using StockPerformance_CleanArchitecture.Models;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailEngine
    {
        public static void CreateAndSendEmail(List<StockPerformanceResponse> responses,
            List<EntityDefinitions.Email> emailsTosend)
        {
            foreach (var email in emailsTosend)
            {
                MailMessage emailMessage;
                bool success;
                GenerateEmail(responses, email, out emailMessage, out success);
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

        public static void GenerateEmail(List<StockPerformanceResponse> dictionary, EntityDefinitions.Email email, out MailMessage emailMessage, out bool success)
        {
            var text = string.Join(", ",
                dictionary.Select(d => $"{d.Symbol}, {d.ProfitSummaryPercentage.DisplayProfitSummary()}" +
                $"{d.Conclusion()}" +
                $" Visit https://stockperformance.azurewebsites.net/?symbol={d.Symbol} for more details"));

            emailMessage = new MailMessage(
"stockperformance2023@gmail.com",
email.EmailAddress,
$"Stock Performance Report on {DateTime.Now.ToLongDateString()}",
@$"Dear {email.FistName}, 
This is your stock performance reports for {DateTime.Now.ToLongDateString()}.
Based on our analysis, these are growing stocks that is growing strong in recent years:
{text}.");
            success = false;
        }
    }
}

