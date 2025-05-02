using System;
using System.Runtime.InteropServices;

internal class Program
{
    // Define the structure to receive power status info
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_POWER_STATUS
    {
        public byte ACLineStatus;      // 0 = offline, 1 = online
        public byte BatteryFlag;       // battery charge status
        public byte BatteryLifePercent; // 0–100 (%)
        public byte Reserved1;
        public int BatteryLifeTime;     // seconds
        public int BatteryFullLifeTime; // seconds
    }

    // Import the function from kernel32.dll
    [DllImport("kernel32.dll")]
    public static extern bool GetSystemPowerStatus(out SYSTEM_POWER_STATUS sps);

    static void Main(string[] args)
    {
        SYSTEM_POWER_STATUS status;

        if (GetSystemPowerStatus(out status))
        {
            Console.WriteLine("AC Power Status: " + (status.ACLineStatus == 1 ? "Plugged In" : "Running on Battery"));
            Console.WriteLine("Battery Life Percent: " + status.BatteryLifePercent + "%");
            Console.WriteLine("Battery Status: " + GetBatteryStatus(status.BatteryFlag));

            if (status.BatteryLifeTime != -1)
                Console.WriteLine("Battery Life Remaining: " + TimeSpan.FromSeconds(status.BatteryLifeTime));
            else
                Console.WriteLine("Battery Life Remaining: Unknown");
        }
        else
        {
            Console.WriteLine("Failed to retrieve battery info.");
        }

        string GetBatteryStatus(byte batteryFlag)
        {
            if (batteryFlag == 0xFF)
                return "Unknown";
            if ((batteryFlag & 0x80) != 0)
                return "No Battery";
            if ((batteryFlag & 0x08) != 0)
                return "Charging";
            if ((batteryFlag & 0x04) != 0)
                return "Critical – (Battery < 5%)";
            if ((batteryFlag & 0x02) != 0)
                return "Low – (Battery < 33%)";
            if ((batteryFlag & 0x01) != 0)
                return "High – (Battery > 66%)";

            return "Battery status not flagged.";
        }
    }

}
