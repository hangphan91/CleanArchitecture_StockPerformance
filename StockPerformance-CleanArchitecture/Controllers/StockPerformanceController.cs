using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Web.Helpers;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Exporter;
using FusionCharts.Visualization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Common;
using StockPerformance_CleanArchitecture.Formatters;
using StockPerformance_CleanArchitecture.Helpers;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;
using SwiftExcel;
using Sheet = SwiftExcel.Sheet;

namespace StockPerformance_CleanArchitecture.Controllers
{
    public class StockPerformanceController : Controller
    {
        private readonly ILogger<StockPerformanceController> _logger;
        private readonly SearchDetailManager _searchDetailManager;
        private ConcurrentBag<StockPerformanceResponse> _response;

        public StockPerformanceController(ILogger<StockPerformanceController> logger)
        {
            _logger = logger;
            _searchDetailManager = ManagerHelper.SearchDetailManager;
            _response = new ConcurrentBag<StockPerformanceResponse>();
        }

        public async Task<IActionResult> StockPerformance(string json)
        {
            SearchDetail? searchDetail = null;

            if (!string.IsNullOrWhiteSpace(json))
                searchDetail = JsonSerializer.Deserialize<SearchDetail>(json);

            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);

            return View(response);
        }

        public async Task<IActionResult> StockPerformanceFromSelectedDetail(SearchDetail searchDetail, bool useDefaultSetting)
        {
            var response = await _searchDetailManager.GetStockPerformanceResponse(searchDetail);

            return View("StockPerformance", response);
        }

        public IActionResult StockPerformanceHistory()
        {
            var history = _searchDetailManager.GetStockPerformanceHistory();
            return View(history);
        }

        public async Task<IActionResult> AdvanceSearch(bool searchAll)
        {
            var advancedSearchResult = await _searchDetailManager.PerformAdvanceSearch(searchAll);

            return View(advancedSearchResult);
        }


        public async Task<IActionResult> SeachAll()
        {
            var advancedSearchResult = await _searchDetailManager.PerformAdvanceSearchForAll();

            return View(advancedSearchResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> OnPostDownloadFile()
        {
            var response = CachedHelper.GetAllCache()
                .OrderByDescending(a => a.CreatedTime)
                .FirstOrDefault();
            if (response == null)
                return Ok();
            CreateAndDownloadFile(response.GetSaveFileStockPerformanceSetting());
            return CreateAndDownloadFile(response.GetSaveFilePerformanceResultTableForAll());
        }

        public IActionResult CreateAndDownloadFile(string filePath)
        {
            var result = File(System.IO.File.OpenRead(filePath), "application/vnd.ms-excel", Path.GetFileName(filePath));
            System.IO.File.Delete(filePath);
            Console.WriteLine(filePath);
            return result;
        }

        private IActionResult CreateAndDownloadFile()
        {
            var response = CachedHelper.GetAllCache()
                .OrderByDescending(a => a.CreatedTime)
                .FirstOrDefault();

            if (response == null)
                return Ok();

            string filePath = WriteDataIntoFile(response);

            if (System.IO.File.Exists(filePath))
            {
                var result = File(System.IO.File.OpenRead(filePath), "application/octet-stream", Path.GetFileName(filePath));
                System.IO.File.Delete(filePath);
                return result;
            }

            return NotFound();
        }


        private static string WriteDataIntoFile(StockPerformanceResponse? response)
        {
            var depositRules = new List<Models.Settings.DepositRule>
            {
                response.SearchDetail.DepositRule
            };
            var tradingRules = new List<Models.Settings.TradingRule>
            {
                response.SearchDetail.TradingRule
            };
            var searchSetUps = new List<SearchInitialSetup>
            {
                response.SearchDetail.SearchSetup
            };

            var settingDates = new List<Models.Settings.SettingDate>
            {
                response.SearchDetail.SettingDate
            };

            var profitPercentage = new List<ProfitSummaryPercentage>
            {
                response.ProfitSummaryPercentage
            };

            var profitDollar = new List<ProfitSummaryInDollar>
            {
                response.ProfitSummaryInDollar
            };

            var responses = new List<StockPerformanceResponse> { response };

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Report{DateTime.Now:yyyy-MM-dd HH-mm-ss}.xlsx");
            var beginRowNumber = 1;

            var exports = ExcelOperations.ExportToExcel(response.StockLedgerDetails, beginRowNumber);
            exports.AddRange(ExcelOperations.ExportToExcel(response.DepositLedgers, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(response.SymbolSummaries, exports.Max(a => a.RowNumber)));

            exports.AddRange(ExcelOperations.ExportToExcel(depositRules, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(tradingRules, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(searchSetUps, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(settingDates, exports.Max(a => a.RowNumber)));

            var monthlyProfitInDollar = response.ProfitSummaryInDollar.MonthlyProfits;
            var yearlyProfitInDollar = response.ProfitSummaryInDollar.YearlyBalanceHoldings;

            var monthlyProfitInPercentage = response.ProfitSummaryPercentage.MonthlyProfits;
            var yearlyProfitInPercentage = response.ProfitSummaryPercentage.MonthlyProfits;

            exports.AddRange(ExcelOperations.ExportToExcel(monthlyProfitInDollar, exports.Max(a => a.RowNumber), "$"));
            exports.AddRange(ExcelOperations.ExportToExcel(yearlyProfitInDollar, exports.Max(a => a.RowNumber), "%"));
            exports.AddRange(ExcelOperations.ExportToExcel(monthlyProfitInPercentage, exports.Max(a => a.RowNumber), "$"));
            exports.AddRange(ExcelOperations.ExportToExcel(yearlyProfitInPercentage, exports.Max(a => a.RowNumber), "%"));

            exports.AddRange(ExcelOperations.ExportToExcel(profitDollar, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(profitPercentage, exports.Max(a => a.RowNumber)));
            exports.AddRange(ExcelOperations.ExportToExcel(responses, exports.Max(a => a.RowNumber)));

            var sheet = new Sheet();
            sheet.Name = "Report";

            using (var ew = new ExcelWriter(filePath, sheet))
            {
                foreach (var item in exports)
                {
                    try
                    {
                        ew.Write(item.Value, item.ColumnNumber, item.RowNumber);
                    }
                    catch (Exception ex)
                    {
                        Console.Write($"Failed on {nameof(item.Value)} {item.Value}" +
                            $" {item.RowNumber} {item.ColumnNumber}. {ex.Message}"); ;
                    }
                }
            }

            return filePath;
        }
    }
}

