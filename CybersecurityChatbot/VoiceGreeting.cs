using System;
using System.Runtime.InteropServices;
using System.Media;
using System;
using System.Runtime.InteropServices;
using System.Media;

namespace CybersecurityChatbot
{
    public static class VoiceGreeting
    {
        public static void PlayGreeting()
        {
            string path = System.IO.Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "Waveroom Online Record Sun Apr 12 2026 18_7_8 microphone.wav");

            using (SoundPlayer player = new SoundPlayer(path))
            {
                player.Load();
                player.PlaySync();
            }
        }
    }
}