﻿using System.Net;
using System.Net.Mail;
using StockPerformance_CleanArchitecture.Models;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailEngine
    {
        public static void CreateAndSendEmail(List<StockPerformanceResponse> responses,
            List<EntityDefinitions.Email> emailsTosend)
        {
            MailMessage emailMessage = new MailMessage();

            foreach (var email in emailsTosend)
            {
                bool success;
                GenerateEmail(responses, email, out emailMessage, out success);
            }

            SendEmail(emailMessage);

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

        public static void GenerateEmail(List<StockPerformanceResponse> dictionary,
            EntityDefinitions.Email email,out MailMessage emailMessage, out bool success)
        {
            var htmlHead = "<!DOCTYPE html>\n<html>\n<head>\n  <title></title>\n  <meta charset=\"UTF-8\">\n</head>\n<body>";
            var textTable = htmlHead + $"<table> ";

            var tableColumnsMessage = 
                            $" <tr> " +
                            $"  <th>Symbol</th>   " +
                            $"  <th>Yearly Profit</th> " +
                            $"  <th>Monthly Profit</th> " +
                            $"  <th>Average Yearly Growth</th> " +
                            $"  <th>Average Monthly Growth</th> " +
                            $"  <th>Profit %</th> " +
                            $"  <th>Profit $</th> " +
                            $"  <th>Time Frame $</th> " +
                            $"  <th>Link</th> " +
                            $" </tr> ";
            var tableRowsMessage = "";
            foreach (var item in dictionary)
            {
                if (item == null)
                    continue;

                item.ProfitSummaryPercentage?.SetTotalProfit();
                var link = $"https://stockperformance.azurewebsites.net/?symbol={item.Symbol}";
                tableRowsMessage += tableRowsMessage;
                tableRowsMessage += $"  <tr> ";
                tableRowsMessage += $"  <th>{item.Symbol}</th>   ";
                tableRowsMessage += $"  <th>{item.ProfitSummaryPercentage?.TotalYearlyProfit?.ToString()}</th> ";
                tableRowsMessage += $"  <th>{item.ProfitSummaryPercentage?.TotalMonthlyProfit?.ToString()}</th> ";
                var yearlyGrowth = item.ProfitSummaryPercentage?.YearlyGrowthSpeeds;
                var monthlyGrowth = item.ProfitSummaryPercentage?.MonthlyGrowthSpeeds;
                var yearlyGrowthAvg = yearlyGrowth?.Count != 0? yearlyGrowth?.Average(a => a.Rate) : 0;
                var monthlyGrowthAvg = monthlyGrowth?.Count != 0? monthlyGrowth?.Average(a => a.Rate) : 0;
                tableRowsMessage += $"  <th>{yearlyGrowthAvg}</th> ";
                tableRowsMessage += $"  <th>{monthlyGrowthAvg}</th> ";
                tableRowsMessage += $"  <th>{item.ProfitInPercentage}</th> ";
                tableRowsMessage += $"  <th>{item.ProfitInDollar}</th> ";
                tableRowsMessage += $"  <th>{item.SearchDetail?.SettingDate} to {item.SearchDetail?.SearchSetup?.EndingYear}</th> ";
                tableRowsMessage += $"  <th> <a href={link}>{item.Symbol}</a></th>";
                tableRowsMessage += $" </tr> ";
            }

            var tableMessage = textTable + tableColumnsMessage + tableRowsMessage + $"</table>";

            var emailStartMessage = $@"Dear {email.FistName}, " +
                $"\n This is your stock performance reports for {DateTime.Now.ToLongDateString()}." +
                $"\n Based on our analysis, these are growing stocks in recent years:";

            var fullMessage = htmlHead + emailStartMessage + tableMessage + "\n</body>\n</html>";

            emailMessage = new MailMessage(
                        "stockperformance2023@gmail.com",
                        email.EmailAddress,
                        $"Stock Performance Report on {DateTime.Now.ToLongDateString()}",
                        body:fullMessage);

            emailMessage.IsBodyHtml = true;

            success = true;
        }
    }
}
