using StockPerformanceCalculator.Models;

namespace StockPerformance_CleanArchitecture.Models.Settings
{
    public class SettingDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public SettingDate(int year, int month, int date)
        {
            Year = year;
            Month = month;
            Day = date;
        }
        public SettingDate()
        {
            Year = 2020;
            Month = DateTime.Now.Month;
            Day = DateTime.Now.Day;
        }

        internal DateDetail Map()
        {
            return new DateDetail
            {
                Day = Day,
                Year = Year,
                Month = Month,
            };
        }

        internal DateOnly MapDateOnly()
        {
            return new DateOnly(Year, Month, Day);
        }

        public override string ToString()
        {
            return $"{Month}/{Day}/{Year}";
        }

        internal bool IsSame(SettingDate settingDate)
        {
            if (settingDate == null)
                return false;

            return settingDate.Day == Day
                && settingDate.Month == Month
                && settingDate.Year == Year;
        }
    }
}

