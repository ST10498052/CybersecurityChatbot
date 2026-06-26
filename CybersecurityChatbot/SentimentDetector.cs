using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public enum Sentiment { Neutral, Worried, Curious, Frustrated }

    public static class SentimentDetector
    {
        private static readonly Dictionary<Sentiment, List<string>> Keywords =
            new Dictionary<Sentiment, List<string>>
        {
            { Sentiment.Worried, new List<string> {
                "worried", "scared", "afraid", "anxious", "nervous",
                "concerned", "fear", "frightened", "unsafe", "danger" }},

            { Sentiment.Curious, new List<string> {
                "curious", "wondering", "interested", "want to know",
                "tell me more", "explain", "how does", "what is", "why" }},

            { Sentiment.Frustrated, new List<string> {
                "frustrated", "annoyed", "confused", "don't understand",
                "makes no sense", "this is hard", "difficult", "stuck", "lost" }}
        };

        public static Sentiment Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Sentiment.Neutral;

            foreach (var entry in Keywords)
            {
                foreach (var word in entry.Value)
                {
                    if (input.IndexOf(word,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return entry.Key;
                    }
                }
            }

            return Sentiment.Neutral;
        }

        public static string GetSentimentResponse(Sentiment sentiment, string userName)
        {
            return sentiment switch
            {
                Sentiment.Worried =>
                    $"I understand you're feeling worried, {userName}. That's completely " +
                    "understandable — cybersecurity can feel overwhelming. Let me help you.",

                Sentiment.Curious =>
                    $"I love your curiosity, {userName}! Asking questions is the first " +
                    "step to staying safe online.",

                Sentiment.Frustrated =>
                    $"I'm sorry you're finding this confusing, {userName}. Let's take it " +
                    "step by step — I'm here to help.",

                _ => string.Empty
            };
        }
    }
}