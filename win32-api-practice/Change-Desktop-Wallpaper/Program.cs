using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

internal class Program
{
    //import the SystemParametersInfo function from user32.dll
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

    //constants for each function
    public static readonly uint SPI_SETDESKWALLPAPER = 0x14;
    public static readonly uint SPIF_UPDATEINIFILE = 0x01;
    public static readonly uint SPIF_SENDCHANGE = 0x02;

    static void Main(string[] args)
    {
        //string[] wallpapers = Directory.GetFiles(@"C:\Users\DELL\Pictures\Wallpapers", "*.jpg");
        //OR
        List<string> wallpapers = new List<string>
        {
            @"C:\Users\DELL\Pictures\Wallpapers\drgn-bg.jpg",
            @"C:\Users\DELL\Pictures\Wallpapers\astrnt-bg.jpg",
            @"C:\Users\DELL\Pictures\Wallpapers\skulls-bg.jpg",
            @"C:\Users\DELL\Pictures\Wallpapers\dark-water-bg.jpg",
            @"C:\Users\DELL\Pictures\Wallpapers\Black-Aesthetic-bg.jpg",
        };

        SetWallpaper(wallpapers[1]);

        //Cycle Wallpapers Every Few Seconds
        /*while (true)
        {
            foreach (string wallpaper in wallpapers)
            {
                SetWallpaper(wallpaper);
                Thread.Sleep(5000); //5 sec delay
            }
        }*/
    }

    public static void SetWallpaper(string path)
    {
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        Console.WriteLine("Wallpaper Changed.");
    }

}