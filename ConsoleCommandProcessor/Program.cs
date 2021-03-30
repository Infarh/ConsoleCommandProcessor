using System;
using ConsoleCommandProcessor.Commands;
using ConsoleCommandProcessor.Configuration;
using ConsoleCommandProcessor.Workers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleCommandProcessor
{
    class Program
    {
        private static IHost __Hosting;

        public static IHost Hosting => __Hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
           .CreateDefaultBuilder(args)
           .ConfigureServices(ConfigureServices)
           .UseConsoleLifetime()
        ;

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.Configure<ApplicationConfig>(host.Configuration.GetSection("Configuration"));
            services.AddHostedService<UserDialog>();

            services.AddCommands();
        }

        public static IServiceProvider Services => Hosting.Services;

        static void Main(string[] args)
        {
            using var host = Hosting;

            try
            {
                host.Run();
            }
            catch (OperationCanceledException)
            {

            }

            Console.WriteLine("Complete");
        }
    }
}
