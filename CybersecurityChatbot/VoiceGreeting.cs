using System;
using System.Runtime.InteropServices;
using System.Media;

namespace CybersecurityChatbot
{
    public static class VoiceGreeting
    {
        public static void PlayGreeting()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try // the wav sound will play
                {
                    using (SoundPlayer player = new SoundPlayer(
                        @"C:\Users\Student\source\repos\Cybersecurity Awareness Bot\Cybersecurity Awareness Bot\Waveroom Online Record Sun Apr 12 2026 18_7_8 microphone.wav"))
                    {
                        player.Load();
                        player.PlaySync();
                    }
                }
                catch
                {
                    Console.WriteLine("Voice greeting could not be played.");
                }
            }
        }
    }
}