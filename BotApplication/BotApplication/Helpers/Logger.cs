using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using BotApplication.Helpers.Interfaces;

namespace BotApplication.Helpers
{
    class Logger: ILogger
    {
        private readonly SpeechSynthesizer _synthesizer;

        public Logger()
        {
            _synthesizer = new SpeechSynthesizer();
        }

        public void LogGameEvent(string text)
        {
            Console.WriteLine(text);
            Debug.WriteLine(text);
            _synthesizer.SpeakAsync(text);
        }
    }
}
