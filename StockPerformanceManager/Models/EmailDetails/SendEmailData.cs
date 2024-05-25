namespace  StockPerformance_CleanArchitecture.Models.EmailDetails
{
    public class SendEmailData
    {
        public List<StockPerformanceResponse> StockPerformanceResponses { get; private set; }
        public int MaxCount { get; private set; } 
        public List<EmailContact>   EmailContacts { get; private set; }
        public bool IsProfitable { get; set; }
        public SendEmailData(List<StockPerformanceResponse> stockPerformances, int count, List<EmailContact> contacts, bool isProfitable)
        {
            EmailContacts = contacts;
            MaxCount = count;
            StockPerformanceResponses = stockPerformances;
            IsProfitable = isProfitable;
        }
    }
}