using System.Data;
using System.Text;
using SwiftExcel;

namespace Exporter
{
    public class ExcelExport
    {
        public string Value { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public ExcelExport(string value, int colNumber,int rowNumber)
        {
            Value = value;
            ColumnNumber = colNumber;
            RowNumber = rowNumber;
        }

        public static string WriteToExcel(List<ExcelExport> exports)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
               $"Report{DateTime.Now:yyyy-MM-dd HH-mm-ss}.xlsx");
            var sheet = new SwiftExcel.Sheet
            {
                Name = "Report",
                WrapText = true,
            };
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

        public static string ExportToExcel(DataTable dt)
        {
            var exports = new List<ExcelExport>();
            var beginRowNumber = 1;
            StringBuilder strHTMLBuilder = new StringBuilder();
            var columnNumber = 1;
            foreach (DataColumn myColumn in dt.Columns)
            {
                exports.Add(new ExcelExport(myColumn.ColumnName, columnNumber, beginRowNumber));
                columnNumber += 1;
            }
            beginRowNumber += 1;
            foreach (DataRow myRow in dt.Rows)
            {
                columnNumber = 1;
                foreach (DataColumn myColumn in dt.Columns)
                {
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    exports.Add(new ExcelExport(myRow[myColumn.ColumnName]?.ToString(), columnNumber, beginRowNumber));
                    columnNumber += 1;
                }
                beginRowNumber += 1;
            }
            var filePath = ExcelExport.WriteToExcel(exports);
            return filePath;
        }

        public static List<ExcelExport> GetExcelExports(DataTable dt, int exitingRowCount = 1)
        {
            var exports = new List<ExcelExport>();
            var beginRowNumber = 2 + exitingRowCount;
            StringBuilder strHTMLBuilder = new StringBuilder();
            var columnNumber = 1;

            exports.Add(new ExcelExport(dt.TableName, 1, beginRowNumber));
            beginRowNumber += 1;

            foreach (DataColumn myColumn in dt.Columns)
            {
                exports.Add(new ExcelExport(myColumn.ColumnName, columnNumber, beginRowNumber));
                columnNumber += 1;
            }
            beginRowNumber += 1;
            foreach (DataRow myRow in dt.Rows)
            {
                columnNumber = 1;
                foreach (DataColumn myColumn in dt.Columns)
                {
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    exports.Add(new ExcelExport(myRow[myColumn.ColumnName]?.ToString(), columnNumber, beginRowNumber));
                    columnNumber += 1;
                }
                beginRowNumber += 1;
            }
            return exports;
        }
    }
}

