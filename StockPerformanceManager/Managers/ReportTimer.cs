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
    private static string _txtFileName = GetFilePath();
    public ConcurrentBag<DateTime> _lastSend = new ConcurrentBag<DateTime> { DateTime.MinValue };

    private static string GetFilePath()
    {
        string fileName = $"sentEmails.txt";

        var basedPath = AppDomain.CurrentDomain.BaseDirectory;
        //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var uri = new Uri(basedPath);
        Console.WriteLine("basedPath" + basedPath);

        var formattedBasedPath = CrossPlatform.PathCombine(uri.LocalPath);
        string filePath = Path.Combine(formattedBasedPath, fileName);

        return filePath;
    }

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
                        },
                        new Email
                        {
                            EmailAddress = "fisayoayodele01@gmail.com",
                            FirstName = "Joseph"
                        },
                        new Email
                        {
                            EmailAddress = "hanahphan2@gmail.com",
                            FirstName = "Ha"
                        }
                    };

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
        _timer = new System.Timers.Timer(100 * 1000);//every 5 min
        _timer.Elapsed += OnTimedEventStart;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public async Task GetResponses()
    {
        var date = DateTime.Now.AddYears(-4);
        var searchDetailManager = new SearchDetailManager();
        var result = await searchDetailManager.PerformAdvanceSearch(true, date);

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
            await SendEmail();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task SendEmail()
    {
        var willSend = (DateTime.Now - _lastSend.First()).TotalDays >= 1;

        if (!willSend)
            return;

        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday
            || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            return;

        if (_responses.Count == 0 && willSend)
            await GetResponses();

        var sendEmailData =
             SendEmailEngine.GetToSendEmailList(_responses.Select(r => r).ToList(), _emails.Select(e => e).ToList());

        foreach (SendEmailData sendEmail in sendEmailData)
        {
            if (sendEmail.StockPerformanceResponses.Count >= sendEmail.MaxCount)
            {
                _lastSend = new ConcurrentBag<DateTime> { DateTime.Now };
                SendEmailEngine.CreateAndSendEmail(sendEmail.StockPerformanceResponses, sendEmail.EmailContacts, sendEmail.IsProfitable);
                SentDates = new ConcurrentBag<DateTime>() { DateTime.Now };
            }
        }
    }
}
