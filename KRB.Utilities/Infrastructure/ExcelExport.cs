using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KRB.Utilities.Infrastructure;

public class ExcelExport<T>
{
    public ExcelExport(List<T> data)
    {
        using (var spreadsheetDocument = SpreadsheetDocument.Create("Output.xlsx", SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            var sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);

            var headerRow = new Row();

            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(property.Name)
                };
                headerRow.AppendChild(cell);
            }

            worksheetPart.Worksheet.GetFirstChild<SheetData>().AppendChild(headerRow);

            foreach (var item in data)
            {
                var newRow = new Row();
                foreach (var property in type.GetProperties())
                {
                    var value = property.GetValue(item)?.ToString() ?? "";
                    var cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(value)
                    };
                    newRow.AppendChild(cell);
                }
                worksheetPart.Worksheet.GetFirstChild<SheetData>().AppendChild(newRow);
            }
        }
    }
}
