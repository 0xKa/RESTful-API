using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml;

class Program
{
    static void Main()
    {
        string wordPath = @"F:\temp\Generated.docx";
        string excelPath = @"F:\temp\Generated.xlsx";
        string pptPath = @"F:\temp\Generated.pptx";

        CreateWord(wordPath);
        CreateExcel(excelPath);
        CreatePowerPoint(pptPath);

        Console.WriteLine("✅ All files created.");
    }

    // 📄 Create Word (.docx)
    public static void CreateWord(string filePath)
    {
        using (var wordDoc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
        {
            var mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = new Body();

            var para = new Paragraph(new A.Run(new A.Text("Hello, this is a Word document created with Open XML SDK!")));
            body.Append(para);
            mainPart.Document.Append(body);
        }

        Console.WriteLine("✅ Word file created.");
    }

    // 📊 Create Excel (.xlsx)
    public static void CreateExcel(string filePath)
    {
        using (var spreadsheet = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = spreadsheet.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            sheetData.Append(new Row(new Cell[] { CreateCell("Name"), CreateCell("Score") }));
            sheetData.Append(new Row(new Cell[] { CreateCell("Reda"), CreateCell("100") }));

            workbookPart.Workbook.Save();
        }

        Console.WriteLine("✅ Excel file created.");
    }

    private static Cell CreateCell(string value)
    {
        return new Cell
        {
            DataType = CellValues.String,
            CellValue = new CellValue(value)
        };
    }

    // 📽️ Create PowerPoint (.pptx)
    public static void CreatePowerPoint(string filePath)
    {
        using (var presentationDoc = PresentationDocument.Create(filePath, PresentationDocumentType.Presentation))
        {
            var presentationPart = presentationDoc.AddPresentationPart();
            presentationPart.Presentation = new Presentation();

            var slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.Slide = new Slide(new CommonSlideData(new ShapeTree()));

            var slideIdList = presentationPart.Presentation.AppendChild(new SlideIdList());
            slideIdList.Append(new SlideId
            {
                Id = 256U,
                RelationshipId = presentationPart.GetIdOfPart(slidePart)
            });

            slidePart.Slide.Save();
            presentationPart.Presentation.Save();
        }

        Console.WriteLine("✅ PowerPoint file created.");
    }
}
