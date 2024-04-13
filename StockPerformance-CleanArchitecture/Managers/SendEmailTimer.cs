using System.Collections.Concurrent;
using System.Timers;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using EntityDefinitions;
using HP.PersonalStocks.Mgr.Helpers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.EmailDetails;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailTimer
    {
        public ConcurrentBag<StockPerformanceResponse> stockPerformanceResponses;
        public ConcurrentBag<Email> emails;
        public SendEmailTimer()
        {
            stockPerformanceResponses = new ConcurrentBag<StockPerformanceResponse>();
            emails = new ConcurrentBag<Email>();
            Start();
        }
        private static System.Timers.Timer aTimer;

        public void Start()
        {
            SetTimer();
        }

        public void AddResponse(StockPerformanceResponse response, List<Email> toSendEmails)
        {
            if (!stockPerformanceResponses.Any(e => e.Symbol == response.Symbol))
                stockPerformanceResponses.Add(response);           

            toSendEmails.ForEach(a =>
            {
                if (!emails.Any(e => e.EmailAddress == a.EmailAddress))
                    emails.Add(a);
            });
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(5*60000);//24*1000*60*60);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEventStart;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEventStart(Object source, ElapsedEventArgs e)
        {
           var sendEmailData =
                SendEmailEngine.GetToSendEmailList(
                    stockPerformanceResponses.Select(a => a).ToList(),
                    emails.Select(a => a).ToList());
            
            foreach (SendEmailData sendEmail in sendEmailData)
            {
                if (sendEmail.StockPerformanceResponses.Count >= sendEmail.MaxCount)
                {
                    SendEmailEngine.CreateAndSendEmail(sendEmail.StockPerformanceResponses, sendEmail.EmailContacts, sendEmail.IsProfitable);
                    sendEmail.StockPerformanceResponses.ForEach(response => 
                    {
                        if(response != null)
                            stockPerformanceResponses.TryTake(out response);
                    });
                }
            }
        }        
    }
}

