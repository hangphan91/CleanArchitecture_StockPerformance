using System.Text;
using Exporter;
using HP.PersonalStocks.Mgr.Helpers;
using StockPerformance_CleanArchitecture.Models;
using StockPerformance_CleanArchitecture.Models.ProfitDetails;

namespace StockPerformance_CleanArchitecture.Formatters
{
    public class PerformanceResultFormatter
    {
        public PerformanceResultFormatter()
        {
        }

        public static string GetSettingTableHTML(SearchDetail searchDetail)
        {
            if (searchDetail == null)
                return "";

            var table = DataTableFormatter.GetSettingDataTable(searchDetail);
            return HTMLExport.ExportDatatableToTableHtml(table);
        }

        public static string GetStockPerformanceResponseTableHTML(List<StockPerformanceResponse> list)
        {
            var table = DataTableFormatter.GetStockPerformanceReponseDataTable(list);
            return HTMLExport.ExportDatatableToTableHtml(table);
        }

        public static string ExportDataTableToExcelFormatAndGetFile(SearchDetail searchDetail)
        {
            if (searchDetail == null)
                return "";

            var dt = DataTableFormatter.GetSettingDataTable(searchDetail);
            string filePath = ExcelExport.ExportToExcel(dt);
            return filePath;
        }

        public static string ExportDataTableToExcelFormatAndGetFile(List<StockPerformanceResponse> list)
        {
            if (list == null || list.Count ==0)
                return "";

            var dt = DataTableFormatter.GetStockPerformanceReponseDataTable(list);
            string filePath = ExcelExport.ExportToExcel(dt);
            return filePath;
        }

        public static string ExportDataTableToExcelFormatAndGetFile(StockPerformanceResponse response)
        {
            if (response == null)
                return "";

            var dt = DataTableFormatter.GetStockPerformanceReponseDataTable
                (new List<StockPerformanceResponse> { response });
            var exports = ExcelExport.GetExcelExports(dt);

            dt = DataTableFormatter.GetSettingDataTable(response.SearchDetail);
            var addedExports = ExcelExport.GetExcelExports(dt, exports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetMonthlyProfitDataTable(response.ProfitSummaryPercentage.MonthlyProfits, "%");
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetMonthlyProfitDataTable(response.ProfitSummaryInDollar.MonthlyProfits, "$");
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetYearlyProfitDataTable(response.ProfitSummaryPercentage.YearlyProfits, "%");
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetYearlyProfitDataTable(response.ProfitSummaryInDollar.YearlyProfits, "$");
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetDepositLedgerDataTable(response.DepositLedgers);
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            dt = DataTableFormatter.GetStockLedgerDataTable(response.StockLedgerDetails);
            addedExports = ExcelExport.GetExcelExports(dt, addedExports.Max(a => a.RowNumber));
            exports.AddRange(addedExports);

            string filePath = ExcelExport.WriteToExcel(exports);

            return filePath;
        }

        internal static string GetAllHTMLs(StockPerformanceResponse response)
        {
            var table = DataTableFormatter.GetSettingDataTable(response.SearchDetail);
            var html = HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetMonthlyProfitDataTable(response.ProfitSummaryPercentage.MonthlyProfits, "%");
            html += HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetMonthlyProfitDataTable(response.ProfitSummaryInDollar.MonthlyProfits, "$");
            html += HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetYearlyProfitDataTable(response.ProfitSummaryPercentage.YearlyProfits, "%");
            html += HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetYearlyProfitDataTable(response.ProfitSummaryInDollar.YearlyProfits, "$");
            html += HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetDepositLedgerDataTable(response.DepositLedgers);
            html += HTMLExport.ExportDatatableToTableHtml(table);

            table = DataTableFormatter.GetStockLedgerDataTable(response.StockLedgerDetails);
            html += HTMLExport.ExportDatatableToTableHtml(table);

            return html;

        }

        public static string WrapHTML(string body)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html >");
            strHTMLBuilder.Append("<head>");
            strHTMLBuilder.Append("</head>");
            strHTMLBuilder.Append("<body>");
            strHTMLBuilder.Append(body);
            strHTMLBuilder.Append("</body>");
            strHTMLBuilder.Append("</html>");
            string Htmltext = strHTMLBuilder.ToString();
            return Htmltext;
        }
    }
}

