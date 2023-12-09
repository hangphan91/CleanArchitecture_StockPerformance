using System.Data;
using FusionCharts.DataEngine;
using FusionCharts.Visualization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace FusionChartsRazorSamples.Pages
{
    public class ProfitChart : PageModel
    {
        // create a public property. OnGet method() set the chart configuration json in this property.
        // When the page is being loaded, OnGet method will be  invoked
        public string ChartJson { get; internal set; }
        public List<StockPerformanceResponse> Responses { get; set; }
        public List<MonthlyProfit> MonthlyProfits { get; set; }
        public string Symbol { get; set; }
        public ProfitChart(List<MonthlyProfit> monthlyProfits, string symbol)
        {
            Symbol = symbol;
            MonthlyProfits = monthlyProfits;
            OnGet();
        }

        public ProfitChart(List<StockPerformanceResponse> responses)
        {
            Responses = responses;
            OnGetHistory();
        }

        private void OnGetHistory()
        {
            if (!Responses.Any())
                return;
            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table
            ChartData.Columns.Add("Date", typeof(System.String));
            ChartData.Columns.Add("Profit", typeof(System.Double));
            // Add rows to data table

            decimal total = 0;
            foreach (var item in Responses)
                ChartData.Rows.Add(item.Symbol + "-" + item.StartDate.Month
                    + "/" + item.StartDate.Year, item.ProfitInPercentage);

            // Create static source with this data table
            StaticSource source = new StaticSource(ChartData);
            // Create instance of DataModel class
            DataModel model = new DataModel();
            // Add DataSource to the DataModel
            model.DataSources.Add(source);
            // Instantiate Column Chart
            Charts.ColumnChart column = new Charts.ColumnChart("profit_chart_by_symbol");
            // Set Chart's width and height
            column.Width.Pixel(700);
            column.Height.Pixel(400);
            // Set DataModel instance as the data source of the chart
            column.Data.Source = model;
            // Set Chart Title
            column.Caption.Text = $"Profit by symbol based on search setting in % by month";
            // Set chart sub title

            // column.SubCaption.Text = $"{minDate} - {maxDate}";
            // hide chart Legend
            column.Legend.Show = false;
            // set XAxis Text
            column.XAxis.Text = "Date";
            // Set YAxis title
            column.YAxis.Text = "Profits";
            // set chart theme
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
            // set chart rendering json
            ChartJson = column.Render();
        }


        public void OnGet()
        {
            if (!MonthlyProfits.Any())
                return;

            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table
            ChartData.Columns.Add("Date", typeof(System.String));
            ChartData.Columns.Add("Profit", typeof(System.Double));
            // Add rows to data table

            decimal total = 0;
            foreach (var item in MonthlyProfits)
            {
                total += item.Amount;
                ChartData.Rows.Add(item.Month + "/" + item.Year, total);

            }

            // Create static source with this data table
            StaticSource source = new StaticSource(ChartData);
            // Create instance of DataModel class
            DataModel model = new DataModel();
            // Add DataSource to the DataModel
            model.DataSources.Add(source);
            // Instantiate Column Chart
            Charts.ColumnChart column = new Charts.ColumnChart("profit_chart_monthly");
            // Set Chart's width and height
            column.Width.Pixel(700);
            column.Height.Pixel(400);
            // Set DataModel instance as the data source of the chart
            column.Data.Source = model;
            // Set Chart Title
            column.Caption.Text = $"Profit By Year for {Symbol}";
            // Set chart sub title
            var ordered = MonthlyProfits.OrderByDescending(a => a.Year).ThenByDescending(a => a.Month);
            var maxDate = ordered.FirstOrDefault()?.Month + "/" + ordered.FirstOrDefault()?.Year;
            var minDate = ordered.LastOrDefault()?.Month + "/" + ordered.LastOrDefault()?.Year;
            column.SubCaption.Text = $"{minDate} - {maxDate}";
            // hide chart Legend
            column.Legend.Show = false;
            // set XAxis Text
            column.XAxis.Text = "Date";
            // Set YAxis title
            column.YAxis.Text = "Profits";
            // set chart theme
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
            // set chart rendering json
            ChartJson = column.Render();
        }
    }
}