using System;

namespace CybersecurityChatbot
{
    public class ChatBot
    {
        private readonly UserMemory _memory = new UserMemory();
        private string _lastTopic = string.Empty;
        private readonly ActivityLogger _logger = new ActivityLogger();
        private readonly TaskAssistant _taskAssistant = new TaskAssistant();
        private readonly QuizManager _quizManager = new QuizManager();
        private string _pendingReminderTask = string.Empty;
        private bool _quizActive = false;

        public string UserName
        {
            get => _memory.UserName;
            set => _memory.UserName = value;
        }

        // Returns the bot's greeting message after name is set
        public string GetWelcomeMessage(string name)
        {
            _memory.UserName = name;
            return $"Nice to meet you, {name}! I'm your Cybersecurity Awareness Bot. " +
                   "Type 'menu' to see what I can help with, or ask me anything " +
                   "about staying safe online!";
        }

        // Main method called by the GUI on each message
        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Your message was empty. Please type something. " +
                       "(Try: 'What is phishing?' or type 'menu'.)";

            _logger.Log($"User asked: {input}");

            if (input.StartsWith("add task ", StringComparison.OrdinalIgnoreCase))
            {
                string task = input.Substring(9).Trim();

                _taskAssistant.AddTask(task);

                // trigger reminder question
                _pendingReminderTask = task;

                return $"Task added: {task}\n\nWould you like a reminder for this task? (yes/no)";
            }

            if (input.Equals("view tasks",
                StringComparison.OrdinalIgnoreCase))
            {
                return _taskAssistant.ViewTasks();
            }

            if (_quizActive)
            {
                string quizResponse = _quizManager.SubmitAnswer(input);

                if (quizResponse.Contains("QUIZ COMPLETE"))
                {
                    _quizActive = false;
                    _quizManager.Reset();
                }

                return quizResponse;
            }

            if (IsHistoryRequest(input))
            {
                return _logger.GetHistory();
            }

            if (input.Equals("quiz", StringComparison.OrdinalIgnoreCase))
            {
                _quizActive = true;
                return _quizManager.GetNextQuestion();
            }

            // HANDLE REMINDER RESPONSE FLOW
            if (!string.IsNullOrEmpty(_pendingReminderTask))
            {
                if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    return "How many minutes from now should I remind you?";
                }

                if (int.TryParse(input, out int minutes))
                {
                    _taskAssistant.SetReminder(_pendingReminderTask, minutes);

                    string task = _pendingReminderTask;
                    _pendingReminderTask = string.Empty;

                    return $"✅ Okay! I will remind you about '{task}' in {minutes} minutes.";
                }

                if (input.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    _pendingReminderTask = string.Empty;
                    return "Okay, no reminder set.";
                }
            }

            // Sentiment detection — prepend empathetic response if detected
            Sentiment sentiment = SentimentDetector.Detect(input);
            string sentimentPrefix = SentimentDetector.GetSentimentResponse(
                sentiment, _memory.UserName);

            // Conversation flow — handle follow-ups
            string followUp = HandleFollowUp(input);
            if (followUp != null)
                return string.IsNullOrEmpty(sentimentPrefix)
                    ? followUp
                    : $"{sentimentPrefix}\n\n{followUp}";

            // Get topic response
            var (response, topic) = ResponseHandler.GetResponse(input);

            if (response != null)
            {
                if (!string.IsNullOrEmpty(topic))
                {
                    _memory.RecordTopic(topic);
                    _lastTopic = topic;
                }

                string memoryHint = _memory.GetMemoryHint();
                string full = string.IsNullOrEmpty(sentimentPrefix)
                    ? response
                    : $"{sentimentPrefix}\n\n{response}";

                return string.IsNullOrEmpty(memoryHint)
                    ? full
                    : $"{full}\n\n💡 {memoryHint}";
            }

            // Default fallback
            string fallback = ResponseHandler.DefaultResponse();
            return string.IsNullOrEmpty(sentimentPrefix)
                ? fallback
                : $"{sentimentPrefix}\n\n{fallback}";
        }

        private bool IsHistoryRequest(string input)
        {
            string lower = input.ToLower().Trim();

            return lower.Contains("history") ||
                   lower.Contains("activity log") ||
                   lower.Contains("show activity") ||
                   lower.Contains("show history") ||
                   lower.Contains("my activity") ||
                   lower.Contains("what have i asked") ||
                   lower.Contains("previous") ||
                   lower.Contains("chat log") ||
                   lower.Contains("chat history") ||
                   lower.Contains("what is my chat history") ||
                   lower.Contains("conversation history");
        }

        private string HandleFollowUp(string input)
        {
            string lower = input.ToLower().Trim();

            bool isFollowUp =
                lower.Contains("tell me more") ||
                lower.Contains("give me another") ||
                lower.Contains("explain more") ||
                lower.Contains("more info") ||
                lower.Contains("what else") ||
                lower.Contains("another tip");

            if (!isFollowUp || string.IsNullOrEmpty(_lastTopic))
                return null;

            var (response, _) = ResponseHandler.GetResponse(_lastTopic);
            return response != null
                ? $"Here's more on {_lastTopic}:\n\n{response}"
                : $"I don't have more details on {_lastTopic} right now. " +
                  "Try asking about another topic!";
        }

        public string GetGoodbyeMessage()
            => $"Goodbye, {_memory.UserName}! Remember: stay alert, stay secure. 🛡️";
    }
}