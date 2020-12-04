using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdventOfCode
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }

        static int Main(string[] args)
        {
            if (args == null || args.Length != 3 || 
                !int.TryParse(args[0], out int year) ||
                !int.TryParse(args[1], out int day) ||
                !int.TryParse(args[2], out int part) || (part != 1 && part != 2))
            {
                PrintUsage();
                return -1;
            }

            RegisterServices();
            bool result = ServiceProvider.GetRequiredService<ProblemRunner>().Run(year, day, part);
            DisposeServices();
            return result ? 0 : 1;
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
            });
            services.AddSingleton<ProblemFactory>();
            services.AddSingleton<ProblemRunner>();
            ServiceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (ServiceProvider == null)
            {
                return;
            }
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Advent of Code 2020");
            Console.WriteLine("Usage:");
            Console.WriteLine("{0} year day part", Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("    year:     The year of the problem to execute");
            Console.WriteLine("    day:      The day of the problem to execute");
            Console.WriteLine("    part:     The part number of the problem to execute (1 or 2)");
        }
    }
}
