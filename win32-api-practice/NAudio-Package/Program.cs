using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static MMDeviceEnumerator enumerator;
    static MMDevice device;

    static void Main(string[] args)
    {
        Console.WriteLine("Audio Control Console Application");
        Console.WriteLine("Press '+' to increase volume, '-' to decrease volume, 's' to set a specific volume, 'q' to quit.");

        // Initialize audio device enumerator and get the default audio endpoint
        enumerator = new MMDeviceEnumerator();
        device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        while (true)
        {
            // Get the user input for volume control
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.OemPlus || key == ConsoleKey.Add) // '+' Key
            {
                AdjustVolume(0.05f); // Increase volume by 5%
            }
            else if (key == ConsoleKey.OemMinus || key == ConsoleKey.Subtract) // '-' Key
            {
                AdjustVolume(-0.05f); // Decrease volume by 5%
            }
            else if (key == ConsoleKey.S) // 's' Key
            {
                SetSpecificVolume(); // Set a specific volume
            }
            else if (key == ConsoleKey.Q) // 'q' Key
            {
                Console.WriteLine("Exiting program...");
                break;
            }
        }
    }

    // Adjust the volume by a given percentage (+ or -)
    static void AdjustVolume(float adjustment)
    {
        float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;
        float newVolume = currentVolume + adjustment;

        // Clamp the new volume to a valid range [0.0, 1.0]
        newVolume = Clamp(newVolume, 0.0f, 1.0f);

        // Set the new volume level
        device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

        // Display the updated volume level
        Console.WriteLine($"Volume adjusted to: {newVolume * 100:F0}%");
    }

    // Clamp the volume value between 0.0 and 1.0
    static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    // Set a specific volume level (from 0 to 100 percent)
    static void SetSpecificVolume()
    {
        Console.Write("Enter the desired volume percentage (0-100): ");
        string input = Console.ReadLine();

        if (float.TryParse(input, out float percentage))
        {
            if (percentage >= 0 && percentage <= 100)
            {
                // Convert percentage to scalar value between 0.0 and 1.0
                float newVolume = percentage / 100.0f;

                // Clamp volume to the valid range [0.0, 1.0]
                newVolume = Clamp(newVolume, 0.0f, 1.0f);

                // Set the new volume
                device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

                // Display the updated volume level
                Console.WriteLine($"Volume set to: {newVolume * 100:F0}%");
            }
            else
            {
                Console.WriteLine("Please enter a valid percentage between 0 and 100.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
}
