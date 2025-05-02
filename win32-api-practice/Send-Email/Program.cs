using System;
using Outlook = Microsoft.Office.Interop.Outlook;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = "Test Email from C#";
            mailItem.To = "reda0x04@gmail.com";
            mailItem.Body = "Yo, this is a test email sent from a C# application using Outlook Interop.";
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;

            // Don't display if you want to send automatically
            mailItem.Send();

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
