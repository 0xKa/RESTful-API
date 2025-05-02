using System;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

internal class Program
{
    static void Main()
    {
        var pptApp = new PowerPoint.Application();
        var presentation = pptApp.Presentations.Add(Microsoft.Office.Core.MsoTriState.msoTrue);

        // Add a slide
        var slide = presentation.Slides.Add(1, PowerPoint.PpSlideLayout.ppLayoutText);
        slide.Shapes[1].TextFrame.TextRange.Text = "Title Goes Here";
        slide.Shapes[2].TextFrame.TextRange.Text = "Content goes here...";

        string savePath = @"F:\temp\ms-api-powerpoint.pptx";
        presentation.SaveAs(savePath, PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
        presentation.Close();
        pptApp.Quit();

        Console.WriteLine($"PowerPoint saved to: {savePath}");
    }
}
