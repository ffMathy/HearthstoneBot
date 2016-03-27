using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using BotApplication.Cards.Interfaces;
using BotApplication.Interceptors;
using Tesseract;

namespace BotApplication
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AllocConsole();

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof (Program).Assembly)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterType<AggregateInterceptor>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c => new TesseractEngine(Environment.CurrentDirectory, "hearthstone", EngineMode.Default))
                .AsSelf()
                .SingleInstance();

            var container = builder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainWindow>());
        }
    }
}
