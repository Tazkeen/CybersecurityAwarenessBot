using System;
using System.IO;
using System.Media;
using NAudio.Wave;
using System.Threading;
using System.Collections.Generic;

class CybersecurityChatbot
{
    static Random random = new Random();
    static string lastTopic = "";
    static string favoriteTopic = "";

    static Dictionary<string, Action> topicResponders = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
    {
        { "password", RespondPasswordSafety },
        { "phishing", RespondPhishing },
        { "browsing", RespondSafeBrowsing },
        { "safe browsing", RespondSafeBrowsing },
        { "password safety", RespondPasswordSafety }
    };

    static void Main()
    {
        Console.Title = "Cybersecurity Awareness Bot";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PlayAudio("Welcome.wav");
        DisplayAsciiArt();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nWhat is your name? ");
        string userName = Console.ReadLine();
        Console.ResetColor();

        Console.Clear();
        DisplayHeader($"Welcome, {userName}!", ConsoleColor.Green);
        DisplayLine('*', 50);

        RunChatbot(userName);
    }

    static void PlayAudio(string fileName)
    {
        try
        {
            string filePath = @"C:\Users\RC_Student_lab\source\repos\CybersecurityChatbot\Welcome.wav";
            if (File.Exists(filePath))
            {
                using (var audioFileReader = new AudioFileReader(filePath))
                using (var waveOut = new WaveOutEvent())
                {
                    waveOut.Init(audioFileReader);
                    waveOut.Play();

                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                Console.WriteLine("Audio file not found. Skipping sound...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound: {ex.Message}");
        }
    }

    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(@"
  ██████╗  █████╗ ██████╗  
  ██╔══██╗██╔══██╗██╔══██╗ 
  ██║    ║███████║██║████║ 
  ██║  ██║██╔══██║██║══██║ 
  ██████╔╝██║  ██║██████╔╝ 
  ╚═════╝ ╚═╝  ╚═╝╚═════╝  
  C.A.B - Cybersecurity Awareness Bot
");
        Console.ResetColor();
    }

    static void RunChatbot(string userName)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            TypeText($"\n{userName}, how can I help you today? ");
            Console.ResetColor();
            string userInput = Console.ReadLine().ToLower().Trim();

            // New numeric-only input validation
            if (int.TryParse(userInput, out _) || double.TryParse(userInput, out _))
            {
                TypeText("\nThat looks like a number. Please enter a valid question or topic.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(userInput))
            {
                TypeText("\nI didn't quite understand that. Could you please enter a valid question or request?");
                continue;
            }

            if (DetectAndRespondToSentiment(userInput)) continue;

            if (userInput.Contains("exit") || userInput.Contains("bye"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TypeText("\nGoodbye! Stay safe online!");
                Console.ResetColor();
                isRunning = false;
                continue;
            }

            if (userInput.Contains("i'm interested in") || userInput.Contains("my favorite topic is"))
            {
                foreach (var topic in topicResponders.Keys)
                {
                    if (userInput.Contains(topic))
                    {
                        favoriteTopic = topic;
                        TypeText($"\nGreat! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online.");
                        break;
                    }
                }
                if (string.IsNullOrEmpty(favoriteTopic))
                    TypeText("\nThanks for sharing! I'll try to remember that.");
                continue;
            }

            if (userInput.Contains("remind me my topic") || userInput.Contains("what's my favorite"))
            {
                if (!string.IsNullOrEmpty(favoriteTopic))
                {
                    TypeText($"\nYou told me you're interested in {favoriteTopic}. That's a smart choice!");
                }
                else
                {
                    TypeText("\nYou haven't told me your favorite topic yet. Feel free to share it!");
                }
                continue;
            }

            bool isFollowUp = userInput.Contains("another") || userInput.Contains("more") ||
                              userInput.Contains("explain") || userInput.Contains("tell me again") ||
                              userInput.Contains("i'm confused");

            if (isFollowUp)
            {
                if (!string.IsNullOrEmpty(lastTopic) && topicResponders.ContainsKey(lastTopic))
                {
                    topicResponders[lastTopic].Invoke();
                    continue;
                }
                else
                {
                    TypeText("\nCould you clarify what topic you're referring to? You can ask about password safety, phishing, or safe browsing.");
                    continue;
                }
            }

            if (userInput.Contains("how are you") || userInput.Contains("how are you doing"))
            {
                RespondHowAreYou();
                continue;
            }

            if (userInput.Contains("what's your purpose") || userInput.Contains("what do you do"))
            {
                RespondPurpose();
                continue;
            }

            if (userInput.Contains("what can i ask you about"))
            {
                RespondTopics();
                continue;
            }

            // Topic-based detection
            bool matchedTopic = false;
            foreach (var topic in topicResponders.Keys)
            {
                if (userInput.Contains(topic))
                {
                    topicResponders[topic].Invoke();
                    lastTopic = topic;
                    matchedTopic = true;
                    break;
                }
            }

            if (!matchedTopic)
            {
                TypeText("\nI didn't quite understand that. Could you rephrase your question or ask something related to cybersecurity?");
            }
        }
    }

    static bool DetectAndRespondToSentiment(string input)
    {
        if (input.Contains("worried") || input.Contains("anxious") || input.Contains("scared"))
        {
            TypeText("\nIt's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.");
            RespondPhishing();
            lastTopic = "phishing";
            return true;
        }
        else if (input.Contains("curious") || input.Contains("interested"))
        {
            TypeText("\nI'm glad you're curious! Cybersecurity knowledge is power. What would you like to learn more about?");
            return true;
        }
        else if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("overwhelmed"))
        {
            TypeText("\nNo worries, these topics can be tricky. I'm here to help — feel free to ask for explanations or examples anytime.");
            return true;
        }
        return false;
    }

    static void RespondHowAreYou()
    {
        TypeText("\nI'm just a chatbot, but I'm doing great! Thanks for asking. How can I help you with cybersecurity today?");
    }

    static void RespondPurpose()
    {
        TypeText("\nI'm here to help you stay safe online! I can provide tips on password safety, phishing prevention, and safe browsing.");
    }

    static void RespondTopics()
    {
        TypeText("\nYou can ask me about:\n- Password Safety\n- Phishing\n- Safe Browsing");
    }

    static void RespondPasswordSafety()
    {
        List<string> passwordTips = new List<string>
        {
            "Use a mix of uppercase, lowercase, numbers, and special characters in your passwords.",
            "Never reuse passwords across different accounts – if one gets compromised, others could too.",
            "Use a password manager to generate and store complex passwords securely.",
            "Avoid using easily guessable info like birthdays or pet names in your passwords.",
            "Enable two-factor authentication (2FA) for an added layer of security."
        };

        string selectedTip = passwordTips[random.Next(passwordTips.Count)];
        TypeText($"\nPassword Safety Tip: {selectedTip}");
    }

    static void RespondPhishing()
    {
        List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Check the sender's email address closely – phishing emails often use misspelled or lookalike domains.",
            "Avoid clicking on suspicious links or downloading attachments from unknown senders.",
            "Phishing messages often create a sense of urgency. Stay calm and verify the message before acting.",
            "Look for generic greetings like 'Dear Customer' – legitimate companies usually address you by name."
        };

        string selectedTip = phishingTips[random.Next(phishingTips.Count)];
        TypeText($"\nPhishing Tip: {selectedTip}");
    }

    static void RespondSafeBrowsing()
    {
        List<string> browsingTips = new List<string>
        {
            "Always look for 'https://' in the URL to ensure a secure connection.",
            "Avoid clicking on pop-ups or ads that seem too good to be true.",
            "Use an up-to-date antivirus and keep your browser patched.",
            "Don’t enter sensitive information on unfamiliar websites.",
            "Be cautious when using public Wi-Fi – avoid logging into bank accounts or private systems."
        };

        string selectedTip = browsingTips[random.Next(browsingTips.Count)];
        TypeText($"\nSafe Browsing Tip: {selectedTip}");
    }

    static void DisplayHeader(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"=== {text} ===");
        Console.ResetColor();
    }

    static void DisplayLine(char symbol, int length)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(new string(symbol, length));
        Console.ResetColor();
    }

    static void TypeText(string text)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(50);
        }
        Console.WriteLine();
    }
}

