using BotApplication.Strategies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    class DummyLogger : ILogger
    {
        public void LogDebugEvent(string text)
        {
        }

        public void LogGameEvent(string text)
        {
        }
    }
}
