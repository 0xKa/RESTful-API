using System;
using Word = Microsoft.Office.Interop.Word;

internal class Program
{
    static void Main()
    {
        try
        {
            // Start Word application
            var wordApp = new Word.Application();
            wordApp.Visible = false; // Set to true to show Word UI

            // Create a new document
            Word.Document doc = wordApp.Documents.Add();

            // Add text
            Word.Paragraph para = doc.Content.Paragraphs.Add();
            para.Range.Text = "Yo, this is a Word document created using C# and Word Interop!";
            para.Range.InsertParagraphAfter();

            // Save the document
            string savePath = @"F:\temp\ms_word_api";
            doc.SaveAs2(savePath);
            doc.Close();
            wordApp.Quit();

            Console.WriteLine($"Document created and saved at: {savePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
