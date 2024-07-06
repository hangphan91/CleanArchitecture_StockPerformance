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
        _timer = new System.Timers.Timer(100 * 1000);//every 5 min
        _timer.Elapsed += OnTimedEventStart;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public async Task GetResponses()
    {
        var date = DateTime.Now.AddYears(-4);
        SentDates.Add(date);
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

    private void ReadSentDates()
    {
        String line;
        try
        {
            //Pass the file path and file name to the StreamReader constructor
            using (StreamReader sr = new StreamReader(_txtFileName))
            {
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    Console.WriteLine(line);
                    var strings = line.Split(",");
                    foreach (var item in strings)
                    {
                        var date = DateTimeOffset.Parse(item).DateTime;
                        SentDates.Add(date);
                    }
                    //Read the next line
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            //close the file
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }

    private async Task SendEmail()
    {
        if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            return;

        if (SentDates.Select(a => a.Date == DateTime.Now.Date).Any())
            return;

        if (DateTime.Now.Hour != 1)
            return;

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
            else if (sendEmail.IsProfitable)
            {
                SentDates = new ConcurrentBag<DateTime>() { DateTime.Now };
            }
        }

        System.IO.File.AppendAllText(_txtFileName, string.Join(",", SentDates.Distinct().Select(a => a).ToList()));
    }
}
