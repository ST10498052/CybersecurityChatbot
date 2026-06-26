using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    public static class ResponseHandler
    {
        private static readonly Random Rng = new Random();

        // Single responses (exact/contains match)
        private static readonly Dictionary<string, string> Responses =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            
                { "how are you",
              "I am running perfectly and ready to help keep you cyber-safe!" },

            { "what's your purpose",
              "My purpose is to educate you on cybersecurity threats so you can stay " +
              "safe online. Ask me about phishing, passwords, malware, and more!" },

            { "what can i ask you about",
              "You can ask me about:\n" +
              "  • Password safety\n  • Phishing scams\n  • Safe browsing\n" +
              "  • Malware and ransomware\n  • Social engineering\n" +
              "  • Two-factor authentication\n  • VPNs and public Wi-Fi\n" +
              "  • Scams\n  • Privacy\n  Type 'menu' to see these options." },

            { "hello", "Hey there! How can I help you stay safe online today?" },
            { "hi",    "Hi! I am here to boost your cybersecurity knowledge." },
            { "help",  "Type 'menu' to see all topics I can assist with." },

            { "menu",
              "CYBERSECURITY BOT MENU (PART 2 FEATURES)\n\n" +
              "📚 LEARNING TOPICS:\n" +
              "  1. password\n" +
              "  2. phishing\n" +
              "  3. safe browsing\n" +
              "  4. malware\n" +
              "  5. social engineering\n" +
              "  6. two-factor\n" +
              "  7. vpn\n" +
              "  8. public wi-fi\n" +
              "  9. scam\n" +
              "  10. privacy\n" +
              "  11. ransomware\n" +
              "  12. identity theft\n" +
              "  13. data breach\n" +
              "  14. social media safety\n" +
              "  15. online shopping\n\n" +

              "🧠 SMART FEATURES:\n" +
              "  • history → view conversation log\n" +
              "  • quiz → start cybersecurity quiz\n" +
              "  • add task <task> → add a security task\n" +
              "  • view tasks → see your tasks\n\n" +

              "💬 EXTRA COMMANDS:\n" +
              "  • menu → show this menu\n" +
              "  • quit / exit / bye → close chatbot"
            },

            { "password",
              "PASSWORD SAFETY TIPS:\n" +
              "  • Use at least 12 characters mixing letters, numbers and symbols.\n" +
              "  • Never reuse passwords across different sites.\n" +
              "  • Use a reputable password manager such as Bitwarden or 1Password.\n" +
              "  • Change passwords immediately if a breach is suspected.\n" +
              "  • Avoid obvious choices like '123456' or your pet's name." },

            { "safe browsing",
              "SAFE BROWSING TIPS:\n" +
              "  • Always check for 'https://' and a padlock icon before entering data.\n" +
              "  • Keep your browser and extensions up to date.\n" +
              "  • Use an ad blocker to reduce exposure to malicious ads.\n" +
              "  • Avoid downloading files from untrusted websites.\n" +
              "  • Enable pop-up blocking in your browser settings." },

            { "social engineering",
              "SOCIAL ENGINEERING:\n" +
              "  • Attackers manipulate people psychologically to gain access.\n" +
              "  • Be skeptical of urgent requests for personal information.\n" +
              "  • Verify the identity of callers before sharing any details.\n" +
              "  • Legitimate IT staff will never ask for your password.\n" +
              "  • Trust your instincts — if something feels off, it probably is." },

            { "two-factor",
              "TWO-FACTOR AUTHENTICATION (2FA):\n" +
              "  • 2FA adds a second layer of security beyond your password.\n" +
              "  • Use an authenticator app like Google Authenticator over SMS.\n" +
              "  • Enable 2FA on all important accounts: email, banking, social media.\n" +
              "  • Store backup codes securely in case you lose your device." },

            { "vpn",
              "VPNs (Virtual Private Networks):\n" +
              "  • A VPN encrypts your internet traffic, hiding it from eavesdroppers.\n" +
              "  • Always use a VPN on public Wi-Fi networks.\n" +
              "  • Choose a no-log VPN provider with a good privacy policy.\n" +
              "  • Free VPNs often monetise your data — proceed with caution." },

            { "public wi-fi",
              "PUBLIC WI-FI DANGERS:\n" +
              "  • Public Wi-Fi is unsecured and attackers can intercept your traffic.\n" +
              "  • Avoid logging into banking or sensitive accounts on public networks.\n" +
              "  • Use a VPN whenever connecting to public Wi-Fi.\n" +
              "  • Forget the network after use so your device won't auto-reconnect." },

            { "privacy",
              "PRIVACY TIPS:\n" +
              "  • Review app permissions regularly — limit access to camera/location.\n" +
              "  • Use privacy-focused browsers like Firefox or Brave.\n" +
              "  • Opt out of data sharing in app settings wherever possible.\n" +
              "  • Be mindful of what you share on social media.\n" +
              "  • Use encrypted messaging apps like Signal for sensitive chats." },

            { "ransomware",
              "RANSOMWARE PROTECTION:\n" +
              "  • Ransomware encrypts your files and demands payment to unlock them.\n" +
              "  • Back up data regularly to an offline or cloud location.\n" +
              "  • Never pay the ransom — it does not guarantee file recovery.\n" +
              "  • Keep your OS and software fully patched and up to date.\n" +
              "  • Use endpoint protection software with ransomware detection." },
            {
                "identity theft",
                "Identity theft occurs when criminals steal your personal information and use it to commit fraud."},
            {
                "password manager",
                "Password managers generate and store strong passwords securely so you don't have to remember them all."},
            {
                "data breach",
                "A data breach occurs when sensitive information is exposed or stolen from an organisation."},
            {
                "online shopping",
                "Always shop from reputable websites and verify HTTPS before entering payment information."},
            {
                "social media safety",
                "Review your privacy settings regularly and avoid sharing sensitive personal information publicly."}
        };

        // Topics with multiple random responses
        private static readonly Dictionary<string, List<string>> RandomResponses =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "phishing", new List<string> {
                "PHISHING TIP: Phishing emails pretend to be from trusted sources like " +
                "banks or SARS. Always verify the sender's email address carefully.",

                "PHISHING TIP: Never click suspicious links in emails. Hover over them " +
                "first to preview the actual URL before clicking.",

                "PHISHING TIP: Legitimate organisations will NEVER ask for your password " +
                "via email. If they do, it's a scam.",

                "PHISHING TIP: Look for spelling errors and mismatched domains in emails " +
                "— these are common signs of phishing attempts.",

                "PHISHING TIP: When in doubt, go directly to the organisation's official " +
                "website instead of clicking any email links." }},

            { "scam", new List<string> {
                "SCAM ALERT: If an offer sounds too good to be true, it almost certainly is. " +
                "Never send money to strangers online.",

                "SCAM ALERT: Romance scams are on the rise in South Africa. Be cautious of " +
                "people you meet online who quickly ask for financial help.",

                "SCAM ALERT: SARS and banks will never demand immediate payment via phone. " +
                "Hang up and call the official number to verify.",

                "SCAM ALERT: Never share OTP (one-time PIN) codes with anyone — not even " +
                "someone claiming to be from your bank.",

                "SCAM ALERT: Job scams often ask for upfront fees. Legitimate employers " +
                "never ask you to pay to get a job." }},

            { "malware", new List<string> {
                "MALWARE TIP: Install reputable antivirus software and keep it updated " +
                "to catch the latest threats.",

                "MALWARE TIP: Do not open email attachments from unknown senders — " +
                "they are a common malware delivery method.",

                "MALWARE TIP: Back up your data regularly to an external or cloud " +
                "location so you can recover if infected.",

                "MALWARE TIP: If you suspect infection, disconnect from the network " +
                "immediately to prevent spread.",

                "MALWARE TIP: Avoid downloading software from unofficial websites — " +
                "always use official sources or app stores." }}
        };

        // Keywords that map to a topic key
        private static readonly Dictionary<string, string> KeywordMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "password",          "password" },
            { "passwords",         "password" },
            { "passphrase",        "password" },
            { "phishing",          "phishing" },
            { "phish",             "phishing" },
            { "scam",              "scam" },
            { "scams",             "scam" },
            { "fraud",             "scam" },
            { "privacy",           "privacy" },
            { "private",           "privacy" },
            { "personal data",     "privacy" },
            { "malware",           "malware" },
            { "virus",             "malware" },
            { "ransomware",        "ransomware" },
            { "safe browsing",     "safe browsing" },
            { "browsing",          "safe browsing" },
            { "social engineering","social engineering" },
            { "two-factor",        "two-factor" },
            { "2fa",               "two-factor" },
            { "vpn",               "vpn" },
            { "public wi-fi",      "public wi-fi" },
            { "wifi",              "public wi-fi" },
            { "wi-fi",             "public wi-fi" },
        };

        public static (string response, string topic) GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (null, null);

            // Check exact/contains matches first
            foreach (var entry in Responses.OrderByDescending(e => e.Key.Length))
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    return (entry.Value, entry.Key.ToLower());
            }

            // Check keyword map → random responses
            foreach (var kw in KeywordMap.OrderByDescending(k => k.Key.Length))
            {
                if (input.IndexOf(kw.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string topic = kw.Value;
                    if (RandomResponses.TryGetValue(topic, out var list))
                        return (list[Rng.Next(list.Count)], topic);
                    if (Responses.TryGetValue(topic, out var resp))
                        return (resp, topic);
                }
            }

            return (null, null);
        }

        public static string DefaultResponse()
            => "I'm not sure I understand. Could you rephrase? " +
               "(Type 'menu' to see available topics.)";
    }
}