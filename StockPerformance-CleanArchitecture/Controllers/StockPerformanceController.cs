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
using HP.PersonalStocks.Mgr.Helpers;
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
            var filePath = response.GetSaveFilePerformanceResultTableForAll();

            return await CreateAndDownloadFile(filePath);
        }

        [HttpGet]
        public async Task<IActionResult> OnPostDownloadFileForAdvanceSearch()
        {
            var responses = CachedHelper.GetAllCache()
                .OrderByDescending(a => a.CreatedTime)
                .ToList();

            if (responses == null || responses?.Count == 0)
                return Ok();

            var filePath =  PerformanceResultFormatter.ExportDataTableToExcelFormatAndGetFile(responses);

            return await CreateAndDownloadFile(filePath);
        }

        public async Task< IActionResult> CreateAndDownloadFile(string filePath)
        {
           // var result = File(System.IO.File.OpenRead(filePath), "application/vnd.ms-excel", Path.GetFileName(filePath));
            Console.WriteLine(filePath);

            Byte[] buffer = await System.IO.File.ReadAllBytesAsync(filePath);
            System.IO.File.Delete(filePath);

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}

