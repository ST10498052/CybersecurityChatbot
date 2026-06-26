using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class TaskAssistant
    {
        private readonly List<string> _tasks = new();

        // store reminder info
        public Dictionary<string, int> TaskReminders { get; private set; }
            = new Dictionary<string, int>();

        public void AddTask(string task)
        {
            _tasks.Add(task);
        }

        public string ViewTasks()
        {
            if (_tasks.Count == 0)
                return "You currently have no cybersecurity tasks.";

            return string.Join("\n", _tasks);
        }

        public bool TaskExists(string task)
        {
            return _tasks.Contains(task);
        }

        public void SetReminder(string task, int minutes)
        {
            TaskReminders[task] = minutes;
        }
    }
}