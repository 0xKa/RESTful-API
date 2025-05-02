using System;
using System.Runtime.InteropServices;

internal class Program
{
    // Import MessageBox function
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main(string[] args)
    {
        // Show a message box
        MessageBox(IntPtr.Zero, "Yo, This is Win32 API!", "MessageBox Example", 0 + 64) // OK btn + Information icon
        ;

        //note: this is a MessageBox from the windows OS not the console app,
        //the console app communicate with the OS to show it
    }
}

/* Common type Flags:
Value	Meaning
- 0	     - OK button only
- 1	     - OK + Cancel
- 2	     - Abort + Retry + Ignore
- 3	     - Yes + No + Cancel
- 4	     - Yes + No
- 5	     - Retry + Cancel
- 16     - Critical icon
- 32     - Question icon
- 48     - Exclamation icon
- 64     - Information icon
 */

