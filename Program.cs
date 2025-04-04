using System;
using System.IO;
using System.Media;
using NAudio.Wave;  // Import NAudio for audio playback
using System.Threading;

class CybersecurityChatbot
{
    static void Main()
    {
        Console.Title = "Cybersecurity Awareness Bot";
        Console.OutputEncoding = System.Text.Encoding.UTF8; // Support special characters

        PlayAudio("Welcome.wav"); // Play the welcome audio using NAudio

        DisplayAsciiArt(); // Show ASCII art logo

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nWhat is your name? ");
        string userName = Console.ReadLine();
        Console.ResetColor();

        Console.Clear();
        DisplayHeader($"Welcome, {userName}!", ConsoleColor.Green); 
        DisplayLine('*', 50); // Divider

        RunChatbot(userName); // Start chatbot interaction
    }

    // Plays the Audio Greeting using NAudio//
    static void PlayAudio(string fileName)
    {
        try
        {
            string filePath = @"C:\Users\RC_Student_lab\source\repos\CybersecurityChatbot\Welcome.wav"; // Correct the path to your audio file
            if (File.Exists(filePath))
            {
                // Use NAudio to play the audio file
                using (var audioFileReader = new AudioFileReader(filePath))
                using (var waveOut = new WaveOutEvent())
                {
                    waveOut.Init(audioFileReader);  // Initialize playback with audio file
                    waveOut.Play();  // Start playing the audio

                    // Wait until playback finishes
                    // Chatbot will continue running after
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        // Sleep briefly while audio is playing
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

    // Displaying ASCII Art Logo
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

    // Run the chatbot conversation with user
    static void RunChatbot(string userName)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            TypeText($"\n{userName}, how can I help you today? ");
            Console.ResetColor();
            string userInput = Console.ReadLine().ToLower().Trim(); // Convert user input to lowercase and trim leading/trailing spaces

            // Validate input: Check for empty input
            if (string.IsNullOrWhiteSpace(userInput))
            {
                TypeText("\nI didn't quite understand that. Could you please enter a valid question or request?");
            }
            // Exit condition
            else if (userInput.Contains("exit") || userInput.Contains("bye"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TypeText("\nGoodbye! Stay safe online!");
                Console.ResetColor();
                isRunning = false;  // Exit the chatbot loop
            }
            // Respond to various cybersecurity questions
            else if (userInput.Contains("how are you") || userInput.Contains("how are you doing"))
            {
                RespondHowAreYou();
            }
            else if (userInput.Contains("what's your purpose") || userInput.Contains("what do you do"))
            {
                RespondPurpose();
            }
            else if (userInput.Contains("what can i ask you about"))
            {
                RespondTopics();
            }
            else if (userInput.Contains("password safety"))
            {
                RespondPasswordSafety();
            }
            else if (userInput.Contains("phishing"))
            {
                RespondPhishing();
            }
            else if (userInput.Contains("safe browsing"))
            {
                RespondSafeBrowsing();
            }
            else
            {
                TypeText("\nI didn't quite understand that. Could you rephrase your question or ask something related to cybersecurity?");
            }
        }
    }

    // Responds to the "How are you?" question
    static void RespondHowAreYou()
    {
        TypeText("\nI'm just a chatbot, but I'm doing great! Thanks for asking. How can I help you with cybersecurity today?");
    }

    // Responds to the "What's your purpose?" question
    static void RespondPurpose()
    {
        TypeText("\nI'm here to help you stay safe online! I can provide tips on password safety, phishing prevention, and safe browsing.");
    }

    // Responds to the "What can I ask you about?" question
    static void RespondTopics()
    {
        TypeText("\nYou can ask me about:\n- Password Safety\n- Phishing\n- Safe Browsing\n- General Cybersecurity Tips");
    }

    // Responds with advice on password safety
    static void RespondPasswordSafety()
    {
        TypeText("\nHere are some password safety tips:\n- Use long, complex passwords that are hard to guess.\n- Never use the same password for multiple accounts.\n- Consider using a password manager to keep track of your passwords securely.\n- Enable two-factor authentication whenever possible.");
    }

    // Responds with advice on phishing
    static void RespondPhishing()
    {
        TypeText("\nPhishing is a cyber attack where attackers try to trick you into revealing personal information. Here's how to stay safe:\n- Don't click on links in unsolicited emails or messages.\n- Always verify the sender's email address before opening attachments or links.\n- Look for signs of suspicious emails, like poor grammar or generic greetings.");
    }

    // Responds with tips on safe browsing
    static void RespondSafeBrowsing()
    {
        TypeText("\nTo browse the internet safely, you should:\n- Use HTTPS websites when possible to ensure secure communication.\n- Avoid downloading files from untrusted sources.\n- Keep your browser and security software up to date.\n- Be cautious about sharing personal information online.");
    }

    // Display a header with specific color
    static void DisplayHeader(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"=== {text} ===");
        Console.ResetColor();
    }

    // Display a line of a given symbol for dividers
    static void DisplayLine(char symbol, int length)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(new string(symbol, length));
        Console.ResetColor();
    }

    // Simulate typing effect with a delay
    static void TypeText(string text)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(50); // Adjust the typing speed
        }
        Console.WriteLine(); // Move to the next line after typing
    }
}
