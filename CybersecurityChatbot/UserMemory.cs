using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class UserMemory
    {
        public string UserName { get; set; } = "User";
        public string FavouriteTopic { get; set; } = string.Empty;
        public List<string> TopicsDiscussed { get; set; } = new List<string>();

        public void RecordTopic(string topic)
        {
            if (!TopicsDiscussed.Contains(topic))
                TopicsDiscussed.Add(topic);
            FavouriteTopic = topic;
        }

        public string GetMemoryHint()
        {
            if (TopicsDiscussed.Count == 0) return string.Empty;

            return TopicsDiscussed.Count == 1
                ? $"By the way, since you're interested in {TopicsDiscussed[0]}, " +
                  $"make sure to review those tips regularly!"
                : $"You've asked about {string.Join(", ", TopicsDiscussed)} — " +
                  $"great effort learning about cybersecurity, {UserName}!";
        }
    }
}