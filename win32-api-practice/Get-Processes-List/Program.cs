using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("psapi.dll", SetLastError = true)]
    public static extern bool EnumProcesses([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)][In][Out] uint[] processIds, uint arraySizeBytes, out uint bytesReturned);

    [DllImport("psapi.dll", SetLastError = true)]
    public static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, uint size);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_MEMORY_COUNTERS
    {
        public uint cb;
        public uint PageFaultCount;
        public uint PeakWorkingSetSize;
        public uint WorkingSetSize;
        public uint QuotaPeakPagedPoolUsage;
        public uint QuotaPagedPoolUsage;
        public uint QuotaPeakNonPagedPoolUsage;
        public uint QuotaNonPagedPoolUsage;
        public uint PagefileUsage;
        public uint PeakPagefileUsage;
    }

    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int PROCESS_VM_READ = 0x0010;

    static void Main()
    {
        uint[] processIds = new uint[1024];
        uint bytesReturned;

        if (EnumProcesses(processIds, (uint)processIds.Length * sizeof(uint), out bytesReturned))
        {
            Console.WriteLine("Number of processes: {0}", bytesReturned / sizeof(uint));

            for (int i = 0; i < bytesReturned / sizeof(uint); i++)
            {
                uint pid = processIds[i];
                IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, pid);

                if (processHandle != IntPtr.Zero)
                {
                    PROCESS_MEMORY_COUNTERS memCounters;
                    if (GetProcessMemoryInfo(processHandle, out memCounters, (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS))))
                    {
                        string processName = "Unknown";
                        try
                        {
                            Process proc = Process.GetProcessById((int)pid);
                            processName = proc.ProcessName;
                        }
                        catch (Exception)
                        {
                            // Process might have exited or access denied
                        }

                        Console.WriteLine($"Process ID: {pid}, Name: {processName} - Memory Usage: {memCounters.WorkingSetSize / 1024} KB");
                    }

                    CloseHandle(processHandle);
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to enumerate processes.");
        }
    }
}


/* another version, 
 * (Must Rebuild the project as "x64", not "AnyCPU" or "x86" becasue we are on a 64-bit system. )

using System;
using System.Text;
using System.Runtime.InteropServices;

//note: Rebuild the project as "x64", not "AnyCPU" or "x86" becasue we are on a 64-bit system.
internal class Program
{
    const int MAX_PATH = 260;
    const uint PROCESS_QUERY_INFORMATION = 0x0400;
    const uint PROCESS_VM_READ = 0x0010;

    [DllImport("psapi.dll", SetLastError = true)]
    static extern bool EnumProcesses([Out] uint[] lpidProcess, uint cb, out uint lpcbNeeded);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

    [DllImport("psapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, int nSize);

    [DllImport("psapi.dll", SetLastError = true)]
    static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, uint size);

    [DllImport("kernel32.dll")]
    static extern bool CloseHandle(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential, Size = 72)]
    public struct PROCESS_MEMORY_COUNTERS
    {
        public uint cb;
        public uint PageFaultCount;
        public ulong PeakWorkingSetSize;
        public ulong WorkingSetSize;
        public ulong QuotaPeakPagedPoolUsage;
        public ulong QuotaPagedPoolUsage;
        public ulong QuotaPeakNonPagedPoolUsage;
        public ulong QuotaNonPagedPoolUsage;
        public ulong PagefileUsage;
        public ulong PeakPagefileUsage;
    }

    static void Main()
    {
        uint[] processIds = new uint[1024];
        uint bytesReturned;

        if (EnumProcesses(processIds, (uint)(processIds.Length * sizeof(uint)), out bytesReturned))
        {
            int processCount = (int)(bytesReturned / sizeof(uint));
            Console.WriteLine($"{"PID",-8} {"Name",-25} {"Memory (KB)",15}");

            for (int i = 0; i < processCount; i++)
            {
                uint pid = processIds[i];
                IntPtr hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, pid);

                if (hProcess != IntPtr.Zero)
                {
                    StringBuilder name = new StringBuilder(MAX_PATH);
                    GetModuleBaseName(hProcess, IntPtr.Zero, name, MAX_PATH);

                    PROCESS_MEMORY_COUNTERS mem;
                    mem.cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS));

                    if (GetProcessMemoryInfo(hProcess, out mem, mem.cb))
                    {
                        ulong memKB = mem.WorkingSetSize / 1024;
                        Console.WriteLine($"{pid,-8} {name,-25} {memKB,15:N0}");
                    }

                    CloseHandle(hProcess);
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to enumerate processes.");
        }
    }
}

 */