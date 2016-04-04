using Bot.Interceptors.Interfaces;
using BotApplication.Strategies.State.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    class Program
    {
        private readonly IEnumerable<IInterceptor> interceptors;

        public Program(
            IEnumerable<IInterceptor> interceptors)
        {
            this.interceptors = interceptors;
        }

        public void Run()
        {
            foreach (var interceptor in interceptors)
            {
                interceptor.BeginInterception();
            }
        }
    }
}
