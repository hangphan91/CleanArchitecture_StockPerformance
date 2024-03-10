using System;
using System.Data;
using System.Text;

namespace Exporter
{
    public class HTMLExport
    {
        public HTMLExport()
        {
        }
        public static string ExportDatatableToTableHtml(DataTable dt)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append($"<h3>{dt.TableName}</h3>");
            strHTMLBuilder.Append("<table border=\"1\" class=\"table table-bordered table-responsive table-hover\">");
            strHTMLBuilder.Append("<tr >");
            foreach (DataColumn myColumn in dt.Columns)
            {
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append(myColumn.ColumnName);
                strHTMLBuilder.Append("</td>");
            }
            strHTMLBuilder.Append("</tr>");
            foreach (DataRow myRow in dt.Rows)
            {
                strHTMLBuilder.Append("<tr >");
                foreach (DataColumn myColumn in dt.Columns)
                {
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    strHTMLBuilder.Append("</td>");
                }
                strHTMLBuilder.Append("</tr>");
            }
            //Close tags.
            strHTMLBuilder.Append("</table>");
            strHTMLBuilder.Append("<br>");

            string Htmltext = strHTMLBuilder.ToString();
            return Htmltext;
        }
    }
}

