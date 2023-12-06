namespace StockPerformanceCalculator.Models
{
    public class DateDetail
	{
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public DateDetail(int year, int month, int date)
        {
            Year = year;
            Month = month;
            Day = date;
        }

        public DateDetail()
        {
            Year = 2020;
            Month = DateTime.Now.Month;
            Day = DateTime.Now.Day;
        }

        public override string ToString()
        {
            return $"{Month}/{Day}/{Year}";
        }
    }
}

