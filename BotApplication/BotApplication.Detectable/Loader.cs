//using Autofac;
//using BotApplication.Strategies.Interfaces;
//using Autofac.Builder;

using Bot.Interceptors;
using System;
using System.Runtime.CompilerServices;

using MeGameState = BotApplication.Strategies.State.GameState;

namespace Bot
{
    public class Loader
    {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        public static void Load()
        {
            var gameState = new MeGameState(new DummyLogger());

            var program = new Program(new[]
            {
                new GameStateInterceptor(gameState)
            });
            program.Run();
        }

        private static void Alert(string alert)
        {
            DialogManager.Get().ShowMessageOfTheDay(alert);
        }

        public static void Unload()
        {
        }
    }
}