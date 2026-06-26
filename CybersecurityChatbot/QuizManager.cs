using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> _questions = new();
        private int _currentIndex = 0;
        private int _score = 0;

        public QuizManager()
        {
            // ── MULTIPLE CHOICE ─────────────────────────────

            _questions.Add(new QuizQuestion(
                "What does phishing mean?",
                new[] { "A fishing sport", "Stealing sensitive data using fake messages", "A virus type", "Firewall protection" },
                2
            ));

            _questions.Add(new QuizQuestion(
                "What is 2FA used for?",
                new[] { "Extra login security", "Deleting viruses", "Faster internet", "Encrypting files" },
                1
            ));

            _questions.Add(new QuizQuestion(
                "What is a strong password feature?",
                new[] { "Your name", "123456", "Mix of letters, numbers, symbols", "Your birth year" },
                3
            ));

            _questions.Add(new QuizQuestion(
                "What should you do with suspicious emails?",
                new[] { "Click links", "Ignore or verify sender", "Reply immediately", "Forward to friends" },
                2
            ));

            _questions.Add(new QuizQuestion(
                "What is malware?",
                new[] { "Hardware device", "Software designed to harm systems", "Internet speed tool", "Email service" },
                2
            ));

            _questions.Add(new QuizQuestion(
                "What is a VPN used for?",
                new[] { "Speeding up games", "Encrypting internet traffic", "Deleting viruses", "Opening emails" },
                2
            ));

            // ── TRUE / FALSE ─────────────────────────────

            _questions.Add(new QuizQuestion(
                "True or False: You should share your OTP with trusted websites.",
                new[] { "True", "False" },
                2
            ));

            _questions.Add(new QuizQuestion(
                "True or False: Public Wi-Fi can be unsafe.",
                new[] { "True", "False" },
                1
            ));

            _questions.Add(new QuizQuestion(
                "True or False: Antivirus software helps protect your device.",
                new[] { "True", "False" },
                1
            ));

            _questions.Add(new QuizQuestion(
                "True or False: It is safe to reuse the same password everywhere.",
                new[] { "True", "False" },
                2
            ));
        }

        public bool HasNextQuestion()
        {
            return _currentIndex < _questions.Count;
        }

        public string GetNextQuestion()
        {
            var q = _questions[_currentIndex];

            string optionsText = "";

            for (int i = 0; i < q.Options.Length; i++)
            {
                optionsText += $"{i + 1}. {q.Options[i]}\n";
            }

            return
                $"QUESTION {_currentIndex + 1} / {_questions.Count}\n\n" +
                $"{q.Question}\n\n" +
                optionsText +
                $"\nEnter 1 - {q.Options.Length}";
        }

        public string SubmitAnswer(string input)
        {
            if (!int.TryParse(input, out int answer))
                return "Please enter a valid option number.";

            var q = _questions[_currentIndex];

            bool isCorrect = answer == q.CorrectIndex;
            string resultMessage;

            // ── FEEDBACK PER QUESTION ─────────────────────────
            if (isCorrect)
            {
                _score++;
                resultMessage =
                    "✅ Correct!\n" +
                    GetExplanation(q);
            }
            else
            {
                resultMessage =
                    $"❌ Wrong!\n" +
                    $"Correct answer: {q.Options[q.CorrectIndex - 1]}\n" +
                    GetExplanation(q);
            }

            _currentIndex++;

            // If more questions → show next one
            if (HasNextQuestion())
            {
                return resultMessage + "\n\n" + GetNextQuestion();
            }

            // End of quiz → final score
            return resultMessage + "\n\n" + GetFinalScore();
        }

        private string GetExplanation(QuizQuestion q)
        {
            if (q.Question.ToLower().Contains("phishing"))
                return "Phishing tricks users into giving away sensitive information like passwords.";

            if (q.Question.ToLower().Contains("2fa"))
                return "2FA adds an extra security layer beyond just a password.";

            if (q.Question.ToLower().Contains("password"))
                return "Strong passwords prevent attackers from easily guessing or cracking accounts.";

            if (q.Question.ToLower().Contains("vpn"))
                return "VPNs encrypt your internet traffic and protect your privacy.";

            return "Understanding this helps improve your overall cybersecurity awareness.";
        }

        public string GetFinalScore()
        {
            return
                "QUIZ COMPLETE!\n\n" +
                $"Your Score: {_score} / {_questions.Count}\n\n" +
                GetPerformanceMessage();
        }

        private string GetPerformanceMessage()
        {
            double percent = (double)_score / _questions.Count * 100;

            if (percent >= 80)
                return "Excellent cybersecurity knowledge! 🛡️";
            if (percent >= 50)
                return "Good effort! Keep learning to improve.";
            return "Keep studying cybersecurity basics and try again!";
        }

        public void Reset()
        {
            _currentIndex = 0;
            _score = 0;
        }

        private class QuizQuestion
        {
            public string Question { get; }
            public string[] Options { get; }
            public int CorrectIndex { get; }

            public QuizQuestion(string question, string[] options, int correctIndex)
            {
                Question = question;
                Options = options;
                CorrectIndex = correctIndex;
            }
        }
    }
}