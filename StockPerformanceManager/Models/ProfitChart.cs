using System.Data;
using System.Text.Json;
using FusionCharts.DataEngine;
using FusionCharts.Visualization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using StockPerformance_CleanArchitecture.Models;

namespace FusionChartsRazorSamples.Pages
{
    public class ProfitChart : PageModel
    {
        // create a public property. OnGet method() set the chart configuration json in this property.
        // When the page is being loaded, OnGet method will be  invoked
        public string ChartJsonForPerformance { get; internal set; }
        public string ChartJsonForPerformanceHistory { get; internal set; }
        public string ChartJsonForStockHistory { get; internal set; }
        public string ChartJsonForDepositAndProfit { get; set; }

        public List<StockPerformanceResponse> Responses { get; set; }
        public StockPerformanceResponse Response { get; set; }
        public string Symbol { get; set; }
        private string _jsonFileName = "name1.json";

        public ProfitChart(StockPerformanceResponse response)
        {
            Symbol = response.Symbol;
            Response = response;

            // CreateChartForMultiLines(monthlyProfits, symbolSummaries, stockLedgerDetails);
            OnGet();
            OnGetStockHistoryChart();
        }

        public ProfitChart(List<StockPerformanceResponse> responses)
        {
            Responses = responses;
            OnGetHistory();
        }

        public void OnGetStockHistoryChart()
        {
            if (!Response.SymbolSummaries.Any())
                return;

            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table
            ChartData.Columns.Add("Date", typeof(System.String));
            ChartData.Columns.Add("Closing Price", typeof(System.Double));
            // Add rows to data table

            decimal total = 0;
            foreach (var item in Response.SymbolSummaries)
            {
                ChartData.Rows.Add(item.Date.ToShortDateString(), item.ClosingPrice);
            }

            // Create static source with this data table
            StaticSource source = new StaticSource(ChartData);
            // Create instance of DataModel class
            DataModel model = new DataModel();
            // Add DataSource to the DataModel
            model.DataSources.Add(source);
            // Instantiate Column Chart
            Charts.ColumnChart column = new Charts.ColumnChart("stock_price_history");
            // Set Chart's width and height
            column.Width.Pixel(800);
            column.Height.Pixel(500);
            // Set DataModel instance as the data source of the chart
            column.Data.Source = model;
            // Set Chart Title
            column.Caption.Text = $"Historical closing price of {Symbol}";
            // Set chart sub title
            var ordered = Response.SymbolSummaries.OrderByDescending(a => a.Date);
            var maxDate = ordered.Max(a => a.Date);
            var minDate = ordered.Min(a => a.Date);
            column.SubCaption.Text = $"{minDate.ToShortDateString()} - {maxDate.ToShortDateString()}";
            // hide chart Legend
            column.Legend.Show = false;
            // set XAxis Text
            column.XAxis.Text = "Date";
            // Set YAxis title
            column.YAxis.Text = "Closing Price";
            // set chart theme
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
            // set chart rendering json
            ChartJsonForStockHistory = column.Render();
        }

        private void OnGetHistory()
        {
            if (!Responses.Any())
                return;
            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table
            ChartData.Columns.Add("Date", typeof(System.String));
            ChartData.Columns.Add("Profit %", typeof(System.Double));
            // Add rows to data table

            decimal total = 0;
            foreach (var item in Responses)
            {
                ChartData.Rows.Add(item.Symbol + "-" + item.StartDate.Month
                    + "/" + item.StartDate.Year, item.ProfitInPercentage);
            }
            // Create static source with this data table
            StaticSource source = new StaticSource(ChartData);
            // Create instance of DataModel class
            DataModel model = new DataModel();
            // Add DataSource to the DataModel
            model.DataSources.Add(source);
            // Instantiate Column Chart
            Charts.ColumnChart column = new Charts.ColumnChart("profit_chart_by_symbol");
            // Set Chart's width and height
            column.Width.Pixel(800);
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
            column.YAxis.Text = "Profits %";
            // set chart theme
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
            // set chart rendering json
            ChartJsonForPerformanceHistory = column.Render();
        }


        public void OnGet()
        {
            if (!Response.ProfitSummaryInDollar.MonthlyProfits.Any())
                return;

            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table
            ChartData.Columns.Add("Date", typeof(System.String));
            ChartData.Columns.Add("Profit", typeof(System.Double));
            // Add rows to data table

            decimal total = 0;
            foreach (var item in Response.ProfitSummaryInDollar.MonthlyProfits)
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
            column.Width.Pixel(800);
            column.Height.Pixel(500);
            // Set DataModel instance as the data source of the chart
            column.Data.Source = model;
            // Set Chart Title
            column.Caption.Text = $"Profit By Year for {Symbol}";
            // Set chart sub title
            var ordered = Response.ProfitSummaryInDollar.MonthlyProfits.OrderByDescending(a => a.Year).ThenByDescending(a => a.Month);
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
            ChartJsonForPerformance = column.Render();
        }

        public string CreateChartForMultiLines()
        {
            return CreateCombineChart();
        }

        public string CreateCombineChart()
        {
            var data = GetDataTable();

            DataModel model = new DataModel();
            //JsonFileSource jsonFileSource = new JsonFileSource("https://raw.githubusercontent.com/poushali-guha-12/SampleData/master/scrollcombidy2d.json");
            JsonFileSource jsonFileSource = new JsonFileSource(_jsonFileName);

            // Add json source in datasources store of model
            //  model.DataSources.Add(jsonFileSource);
          
            // Add json source in datasources store of model
            model.DataSources.Add(jsonFileSource);
            // initialize combination chart object
            Charts.CombinationChart combiChart = new Charts.CombinationChart("mscombidy2d");
            // set model as data source
            combiChart.Data.Source = model;

            // provide field name, which should be rendered as line column
            combiChart.Data.ColumnPlots("Accumulated Deposit");
            // provide field name, which should be rendered as spline area plot
            combiChart.Data.SplineAreaPlots("Profits");
            // provide field name, which should be rendered as spline plot
            combiChart.Data.SplinePlots("ProfitInPercentage");
            // set parentAxis
            combiChart.Data.SecondaryYAxisAsParent("ProfitInPercentage");
            // Set XAxis caption
            combiChart.XAxis.Text = "Timeline";
            // Set YAxis caption
            combiChart.PrimaryYAxis.Text = "Amount (in USD)";
            // enable dual y
            combiChart.DualY = true;
            // set secondary y axis text
            combiChart.SecondaryYAxis.Text = "Profit %";
            // set chart caption
            combiChart.Caption.Text = $"Accumulated Deposit Amount and Profit for {Symbol}";
            // Set chart sub caption
            combiChart.SubCaption.Text = $"From {Response.StartDate}- {Response.CreatedTime.ToShortDateString()}";
            // set width, height
            combiChart.Width.Pixel(800);
            combiChart.Height.Pixel(500);
            // set theme
            combiChart.ThemeName = FusionChartsTheme.ThemeName.FUSION;

            ChartJsonForDepositAndProfit = combiChart.Render();

            return ChartJsonForDepositAndProfit;
        }

        private DataTable GetDataTable()
        {
            DataTable ChartData = new DataTable();

            ChartData.Columns.Add("date");
            ChartData.Columns.Add("Total Profit $");
            ChartData.Columns.Add("Total Deposit");
            ChartData.Columns.Add("Profit %");

            var totalProfit = (decimal)0;
            var dataList = new List<DataCollection>();

            foreach (var item in Response.DepositLedgers)
            {
                var date = item.Date.Month + "_" + item.Date.Year;

                totalProfit = Response.ProfitSummaryInDollar.MonthlyProfits
                    .Where(a => new DateTime(a.Year, a.Month, 1).AddMonths(1).AddDays(-1) <= item.Date)
                    .Sum(a => a.Amount);

                var valueInDeposit = Response.DepositLedgers
                    .Where(a => a.Date <= item.Date)
                    .Sum(a => a.Amount);

                var profitInPercentage = totalProfit * 100 / valueInDeposit;
                dataList.Add(new DataCollection
                {
                    Date = date,
                    ProfitInPercentage = profitInPercentage,
                    Profits = totalProfit,
                    Deposits = valueInDeposit,
                });
                ChartData.Rows.Add(date ,totalProfit, valueInDeposit, profitInPercentage);
            }

            System.IO.File.WriteAllText(_jsonFileName, System.Text.Json.JsonSerializer.Serialize(dataList));
            return ChartData;
        }
    }

    public class DataCollection
    {
        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("Deposits")]
        public decimal Deposits { get; set; }
        [JsonProperty("Profits $")]
        public decimal Profits { get; set; }
        [JsonProperty("Profit %")]
        public decimal ProfitInPercentage { get; set; }
    }
}

