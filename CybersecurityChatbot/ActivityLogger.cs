using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class ActivityLogger
    {
        private readonly List<string> _history = new();

        public void Log(string activity)
        {
            _history.Add(activity);
        }

        public string GetHistory()
        {
            if (_history.Count == 0)
                return "No activity has been recorded yet.";

            return string.Join("\n", _history);
        }
    }
}