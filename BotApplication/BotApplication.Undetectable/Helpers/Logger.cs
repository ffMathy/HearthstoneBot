using System;
using System.Speech.Synthesis;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Strategies
{
    public class Logger: ILogger
    {
        private readonly SpeechSynthesizer _synthesizer;

        public Logger()
        {
            _synthesizer = new SpeechSynthesizer();
        }

        public void LogGameEvent(string text)
        {
            LogDebugEvent(text);
            _synthesizer.SpeakAsync(text);
        }

        public void LogDebugEvent(string text)
        {
            Console.WriteLine(text);
        }
    }
}
