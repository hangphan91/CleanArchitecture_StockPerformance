
using System.Collections;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Spreadsheet;
using FastMember;
using SpreadsheetLight;
using SwiftExcel;
using Color = System.Drawing.Color;

namespace Exporter;
public class ExcelOperations
{
    /// <summary>
    /// Create a DataTable from <see>
    ///     <cref>{T}</cref>
    /// </see>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(IReadOnlyList<T> sender)
    {
        DataTable table = new();
        using var reader = ObjectReader.Create(sender);
        table.Load(reader);

        return table;
    }

    private static bool IsCollectionType(Type type)
    {
        if (type == null)
            return false;
        return (type.GetInterface(nameof(IEnumerable)) != null) || (type.GetInterface(nameof(IList)) != null);
    }

    public static List<ExcelExport> ExportToExcel<T>(IReadOnlyList<T> list, int rowNumber, string unit = "")
    {
        var exports = new List<ExcelExport>();
        rowNumber += 2;
        try
        {
            Type t = typeof(T);
            // Get the public properties.
            PropertyInfo[] myPropertyInfo = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Get the properties of 'Type' class object.
            myPropertyInfo = t.GetProperties();

            var unitText = string.IsNullOrWhiteSpace(unit) ? "" : $"({unit})";

            exports.Add(new ExcelExport(t.Name + unitText, 1, rowNumber));

            rowNumber += 1;
            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                exports.Add(new ExcelExport(myPropertyInfo[i].Name, i + 1, rowNumber));
            }
            rowNumber += 1;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < myPropertyInfo.Length; j++)
                {
                    bool isCollection = IsCollectionType(myPropertyInfo[j]?.GetValue(list[i])?.GetType());

                    var value = isCollection ? "" : myPropertyInfo[j].GetValue(list[i])?.ToString();

                    exports.Add(new ExcelExport(value, j + 1, rowNumber + i));
                }
            }

        }
        catch (Exception exception)
        {
            /*
             * Basic reason for failure
             * 1. Developer error
             * 2. User has file open in Excel but here that is not possible because of the file name
             */
            return exports;
        }

        return exports;
    }
    /// <summary>
    /// Style for first row in the Excel file
    /// </summary>
    public static SLStyle HeaderStye(SLDocument document)
    {

        SLStyle headerStyle = document.CreateStyle();

        headerStyle.Font.Bold = true;
        headerStyle.Font.FontColor = Color.White;
        headerStyle.Fill.SetPattern(
            PatternValues.LightGray,
            SLThemeColorIndexValues.Accent1Color,
            SLThemeColorIndexValues.Accent5Color);

        return headerStyle;
    }
}
