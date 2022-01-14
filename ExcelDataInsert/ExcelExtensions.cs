using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace KSCIRC.ExcelDataInsert
{
    public static class ExcelExtensions
    {
        public static string[] GetHeaderColumns(this ExcelWorksheet sheet)
        {
            var columnNames = new List<string>();

            foreach (var firstRowCell in sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column])
            {
                columnNames.Add(firstRowCell
                    .Text
                    .ToLower()
                    .Trim()
                    .Replace(" ", ""));
            }

            return columnNames.ToArray();
        }

        public static Dictionary<string, int> ReadColumnNumbers(this string[] headers)
        {
            return headers
                .Select((v, i) => new { Key = v, Value = i + 1 }) // + 1 here because EPP uses column numbers, not indexes
                .ToDictionary(o => o.Key, o => o.Value);
        }
    }
}