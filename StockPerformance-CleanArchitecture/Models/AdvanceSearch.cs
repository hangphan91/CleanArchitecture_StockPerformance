﻿using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using StockPerformance_CleanArchitecture.Models.Settings;

namespace StockPerformance_CleanArchitecture.Models
{
    public class AdvanceSearch
	{
        public SearchDetail SearchDetail { get; set; }
        public int Count { get; set; } = -1;
        public List<string> Symbols { get; set; }
        public SettingDate StartDate { get; set; } = new SettingDate(2020, DateTime.Now.Month, DateTime.Now.Day);
        public SettingDate EndDate { get; set; } = new SettingDate(2023, DateTime.Now.Month, DateTime.Now.Day);
        public bool WillPerformSearch { get; set; }

        public AdvanceSearch()
		{
            SearchDetail = new SearchDetail();
            Symbols = new List<string>();
            Count = -1;
		}
	}
}

