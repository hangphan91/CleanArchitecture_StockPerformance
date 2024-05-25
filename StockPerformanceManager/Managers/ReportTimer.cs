using System.Collections.Concurrent;
using System.Timers;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using EntityDefinitions;
using SharpCompress;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.EmailDetails;

namespace StockPerformance_CleanArchitecture.Managers;

public class ReportTimer
{
    private bool IsSent { set; get; }
    private static System.Timers.Timer _timer;
    public static ConcurrentBag<string> _symbols = new ConcurrentBag<string>();
    public static ConcurrentBag<DateTime> SentDates = new ConcurrentBag<DateTime>();
    public static ConcurrentBag<StockPerformanceResponse> _responses = new ConcurrentBag<StockPerformanceResponse>();
    public static ConcurrentBag<Email> _emails = new ConcurrentBag<Email>
                    {
                        new Email
                        {
                            EmailAddress = "stockperformance2023@gmail.com",
                            FirstName = "Love"
                        },
                        new Email
                        {
                            EmailAddress = "cristian.g.navarrete@gmail.com",
                            FirstName = "Cristian"
                        }
                    };

    private object obj;
    public ReportTimer()
    {
        Start();
    }

    public void Start()
    {
        SetTimer();
    }
    private void SetTimer()
    {
        // Create a timer with a two second interval.
        _timer = new System.Timers.Timer(10 * 1000);//every 5 min
        _timer.Elapsed += OnTimedEventStart;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public async Task GetResponses()
    {
        var searchDetailManager = new SearchDetailManager();
        var result = await searchDetailManager.PerformAdvanceSearch(true);

        if (result == null)
            return;

        foreach (var item in result.StockPerformanceResponses)
        {
            item.ProfitSummaryPercentage?.SetTotalProfit();
            _responses.Add(item);
        }
    }

    private async void OnTimedEventStart(Object source, ElapsedEventArgs e)
    {
        try
        {
            var now = DateTime.Now;

            if (SentDates.Any(d => d >= now.AddDays(-7)))
                return;

            SentDates.Add(now);
            _responses = new ConcurrentBag<StockPerformanceResponse>();
            var dates = SentDates.Where(d => d > now.AddDays(-9));

            SentDates = new ConcurrentBag<DateTime>();
            dates.ForEach(d => SentDates.Add(d));

            if (SentDates.Count > 0)
                await SendEmail();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task SendEmail()
    {
        if (_responses.Count == 0)
            await GetResponses();

        var sendEmailData =
             SendEmailEngine.GetToSendEmailList(_responses.Select(r => r).ToList(), _emails.Select(e => e).ToList());

        foreach (SendEmailData sendEmail in sendEmailData)
        {
            if (sendEmail.StockPerformanceResponses.Count >= sendEmail.MaxCount)
            {
                SendEmailEngine.CreateAndSendEmail(sendEmail.StockPerformanceResponses, sendEmail.EmailContacts, sendEmail.IsProfitable);
            }
        }


    }
}
