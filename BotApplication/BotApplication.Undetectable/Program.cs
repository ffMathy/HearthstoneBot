using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Autofac;
using Autofac.Builder;
using BotApplication.Helpers;
using BotApplication.Interceptors;
using BotApplication.Strategies;
using BotApplication.Strategies.Interfaces;
using BotApplication.Strategies.State;
using Tesseract;

namespace BotApplication
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int value);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            SetProcessDpiAwareness(2);
            AllocConsole();

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly, typeof(ICard).Assembly)
                .AsSelf()
                .AsImplementedInterfaces();

            RegisterSingleInstanceType<OcrHelper>(builder);
            RegisterSingleInstanceType<AggregateInterceptor>(builder);
            RegisterSingleInstanceType<GameState>(builder);
            RegisterSingleInstanceType<Logger>(builder);

            builder.Register(c =>
                {
                    var engine = new TesseractEngine(Environment.CurrentDirectory, "hearthstone", EngineMode.Default);
                    engine.SetVariable("tessedit_char_whitelist", "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    return engine;
                })
                .AsSelf()
                .SingleInstance();

            var container = builder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainWindow>());
        }

        private static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterSingleInstanceType<T>(ContainerBuilder builder)
        {
            return builder.RegisterType<T>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
