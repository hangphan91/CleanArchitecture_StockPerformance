using System.Net;
using System.Net.Mail;
using OoplesFinance.YahooFinanceAPI.Models;
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
                bool success;
                GenerateEmail(responses.Distinct().ToList(), email, out var emailMessage, out success);
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
            var tableStartHtml = htmlHead + $"<table> ";

            var tableColumnsMessage =
                            $" <tr> " +
                            $"  <th>Symbol</th> " +
                            $"  <th>Start Date $</th> " +
                            $"  <th>End Date $</th> " +
                            $"  <th>Max Yearly Profit %</th> " +
                            $"  <th>Max Monthly Profit %</th> " +
                            $"  <th>Min Yearly Profit %</th> " +
                            $"  <th>Min Monthly Profit %</th> " +
                            $"  <th>Average Yearly Profit %</th> " +
                            $"  <th>Average Monthly Profit %</th> " +
                            $"  <th>Total Profit %</th> " +
                            $" </tr> ";
            var tableRowsMessage = "";
            foreach (var item in list)
            {
                if (item == null)
                    continue;

                item.ProfitSummaryPercentage?.SetTotalProfit();
                var link = $"https://stockperformance.azurewebsites.net/?symbol={item.Symbol}";

                var yearlyAVGGrowth = item.ProfitSummaryPercentage?.AVGYearlyProfit;
                var monthlyAVGGrowth = item.ProfitSummaryPercentage?.AVGMonthlyProfit;
                var yearlyMinGrowth = item.ProfitSummaryPercentage?.MINYearlyProfit;
                var monthlyMinGrowth = item.ProfitSummaryPercentage?.MINMonthlyProfit;
                var yearlyMaxGrowth = item.ProfitSummaryPercentage?.MAXYearlyProfit;
                var monthlyMaxGrowth = item.ProfitSummaryPercentage?.MAXMonthlyProfit;

                tableRowsMessage += $"  <tr> ";
                tableRowsMessage += $"  <th> <a href={link}>{item.Symbol}</a></th>";
                tableRowsMessage += $"  <th>{item.SearchDetail?.SettingDate}</th> ";
                tableRowsMessage += $"  <th>{ item.SearchDetail?.SearchSetup?.EndingYear}</th> ";
                tableRowsMessage += $"  <th>{yearlyMaxGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyMaxGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{yearlyMinGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyMinGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{yearlyAVGGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{monthlyAVGGrowth.RoundNumber()}</th> ";
                tableRowsMessage += $"  <th>{item.ProfitInPercentage.RoundNumber()}</th> ";
                tableRowsMessage += $" </tr> ";
            }

            var tableMessage = tableStartHtml + tableColumnsMessage + tableRowsMessage + $"</table>";

            var emailStartMessage = $@"Dear {email.FistName}, " +
                $"<br>These are the stock performance reports for {DateTime.Now.ToLongDateString()}. " +
                $"Our analysis highlights the following stocks that have shown growth in recent years:";

            var url = "https://stockperformance.azurewebsites.net";
            var emailEndSentence = $"<br>Thanks,<br> StockPerformance<br><br>Visit us at: <a href={url}>Stock Perfomance Site</a> ";

            var fullMessage = htmlHead + emailStartMessage + tableMessage  + emailEndSentence + "\n</body>\n</html>";

            emailMessage = new MailMessage(
                        "stockperformance2023@gmail.com",
                        email.EmailAddress,
                        $"Stock Performance Report on {DateTime.Now.ToLongDateString()}",
                        body: fullMessage);

            emailMessage.IsBodyHtml = true;

            success = true;
        }
    }
}

