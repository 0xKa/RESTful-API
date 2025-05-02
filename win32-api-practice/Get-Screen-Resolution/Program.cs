using System;
using System.Runtime.InteropServices;

internal class Program
{
    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    static void Main(string[] args)
    {
        int screenWidth = GetSystemMetrics(0);
        int screenHeight = GetSystemMetrics(1);


        Console.WriteLine($"Screen Resolution: {screenWidth}x{screenHeight}");
    }
}