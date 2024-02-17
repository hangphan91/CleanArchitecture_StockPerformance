using System.Collections.Concurrent;
using System.Timers;
using StockPerformance_CleanArchitecture.Models;

namespace StockPerformance_CleanArchitecture.Managers
{
    public class SendEmailTimer
    {
        public ConcurrentBag<StockPerformanceResponse> stockPerformanceResponses;
        public ConcurrentBag<EntityDefinitions.Email> emails;
        public SendEmailTimer()
        {
            stockPerformanceResponses = new ConcurrentBag<StockPerformanceResponse>();
            emails = new ConcurrentBag<EntityDefinitions.Email>();
            Start();
        }
        private static System.Timers.Timer aTimer;

        public void Start()
        {
            SetTimer();
        }

        public void AddResponse(StockPerformanceResponse response, List<EntityDefinitions.Email> toSendEmails)
        {
            if (response.ProfitInPercentage > 20 &&
                stockPerformanceResponses.All(a => a.Symbol != response.Symbol))
                stockPerformanceResponses.Add(response);

            toSendEmails.ForEach(a => emails.Add(a));
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(60000);//24*1000*60*60);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEventStart;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEventStart(Object source, ElapsedEventArgs e)
        {
            var responses = stockPerformanceResponses.Select(a => a).ToList();
            var emailsTosend = emails.Select(a => a).ToList();
            // uhaphan only send email at 5 am and on week day
            var isWeekDay = !(DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Sunday);

            // if (DateTime.Now.Hour == 5 && isWeekDay && responses?.Count > 0)
            if (responses?.Count > 10)
            {
                SendEmailEngine.CreateAndSendEmail(responses, emailsTosend);
                stockPerformanceResponses.Clear();
            }
        }
    }
}

