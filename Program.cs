using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AdventOfCode
{
    /// <summary>
    /// The application class
    /// </summary>
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// The application entry point
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>The status code</returns>
        static int Main(string[] args)
        {
            int returnCode = -1;
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                   .WithParsed(o =>
                   {
                       RegisterServices();
                       bool result = ServiceProvider.GetRequiredService<ProblemRunner>().Run(
                           o.Year, o.Day, o.Part);
                       DisposeServices();
                       returnCode = result ? 0 : 1;
                   });
            return returnCode;
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(configure =>
            {
                configure.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                });
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

        private class CommandLineOptions
        {
            [Option('y', "year", Required = false, HelpText = "The year of the problems to execute. All years if omitted")]
            public int? Year { get; set; }

            [Option('d', "day", Required = false, HelpText = "The day of the problems to execute. All days if omitted")]
            public int? Day { get; set; }

            [Option('p', "part", Required = false, HelpText = "The part numbers to execute (1 or 2). All parts if omitted")]
            public int? Part { get; set; }
        }
    }
}
