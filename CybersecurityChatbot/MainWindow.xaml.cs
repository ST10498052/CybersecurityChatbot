using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private readonly ChatBot _bot = new ChatBot();
        private bool _chatStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            // Play voice greeting on load
            VoiceGreeting.PlayGreeting();
            AppendBotMessage("Hello! Please enter your name above to begin.");
        }

        // ── Name entry ──────────────────────────────────────────────
        private void StartChat_Click(object sender, RoutedEventArgs e)
            => BeginChat();

        private void NameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BeginChat();
        }

        private void BeginChat()
        {
            string name = NameInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                AppendSystemMessage("⚠  Please enter your name to start.");
                return;
            }

            _bot.UserName = name;
            string welcome = _bot.GetWelcomeMessage(name);

            NamePanel.Visibility = Visibility.Collapsed;
            InputArea.IsEnabled = true;
            _chatStarted = true;

            AppendUserMessage(name, "joined the chat");
            AppendBotMessage(welcome);
            MessageInput.Focus();
            StatusBar.Text = $"Chatting as {name}  •  Type 'menu' to see all topics";
        }

        // ── Message sending ─────────────────────────────────────────
        private void Send_Click(object sender, RoutedEventArgs e)
            => SendMessage();

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMessage();
        }

        private void SendMessage()
        {
            if (!_chatStarted) return;

            string input = MessageInput.Text.Trim();
            MessageInput.Clear();

            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(_bot.UserName, input);

            // Quit check
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("bye", StringComparison.OrdinalIgnoreCase))
            {
                AppendBotMessage(_bot.GetGoodbyeMessage());
                InputArea.IsEnabled = false;
                StatusBar.Text = "Session ended. Close the window to exit.";
                return;
            }

            string response = _bot.ProcessInput(input);
            AppendBotMessage(response);
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            if (_chatStarted)
                AppendBotMessage(_bot.GetGoodbyeMessage());
            Application.Current.Shutdown();
        }

        // ── Chat display helpers ────────────────────────────────────
        private void AppendBotMessage(string message)
        {
            AppendText("🤖 Bot: ", Colors.Cyan, bold: true);
            AppendText(message + "\n\n", Colors.LightGreen);
            ScrollToBottom();
        }

        private void AppendUserMessage(string name, string message)
        {
            AppendText($"👤 {name}: ", Colors.Yellow, bold: true);
            AppendText(message + "\n", Color.FromRgb(200, 200, 200));
            ScrollToBottom();
        }

        private void AppendSystemMessage(string message)
        {
            AppendText($"⚠  {message}\n", Colors.OrangeRed);
            ScrollToBottom();
        }

        private void AppendText(string text, Color color, bool bold = false)
        {
            var para = new Paragraph(new Run(text)
            {
                Foreground = new SolidColorBrush(color),
                FontWeight = bold ? FontWeights.Bold : FontWeights.Normal
            })
            {
                Margin = new Thickness(0)
            };
            ChatBox.Document.Blocks.Add(para);
        }

        private void ScrollToBottom()
        {
            ChatScroll.ScrollToEnd();
        }
    }
}