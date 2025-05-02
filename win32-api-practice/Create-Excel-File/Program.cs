using System;
using Excel = Microsoft.Office.Interop.Excel;

internal class Program
{
    static void Main()
    {
        var excelApp = new Excel.Application();
        excelApp.Visible = false;

        var workbook = excelApp.Workbooks.Add();
        Excel._Worksheet worksheet = workbook.Sheets[1];
        worksheet.Name = "Sheet1";

        worksheet.Cells[1, 1] = "Name";
        worksheet.Cells[1, 2] = "Score";
        worksheet.Cells[2, 1] = "Reda";
        worksheet.Cells[2, 2] = 100;

        string savePath = @"F:\temp\ms-api-excel.xlsx";
        workbook.SaveAs(savePath);
        workbook.Close();
        excelApp.Quit();

        Console.WriteLine($"Excel file saved to: {savePath}");
    }
}
