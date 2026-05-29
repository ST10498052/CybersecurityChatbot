using System;

namespace CybersecurityChatbot
{
    public class ChatBot
    {
        private readonly UserMemory _memory = new UserMemory();
        private string _lastTopic = string.Empty;

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