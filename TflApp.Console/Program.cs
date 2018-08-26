using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TflApp.Console.Entity;
using TflApp.Console.Repository;
using TflApp.Console.Service;
using TflApp.Console.Utils;
using Serilog;

namespace TflApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .WriteTo.File("tflapp.log")
                .CreateLogger();
       
            try
            {
                var services = new ServiceCollection();
                Configure(services);
                var serviceProvider = services.BuildServiceProvider();
                var app = serviceProvider.GetService<App>();
                app.ShowRoadStatusAsync(args).Wait();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error");
                System.Console.WriteLine("An unexpected error has occurred.");
                Environment.Exit((int)ExitCode.UnknownError);
            }
           
        }

        private static void Configure(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            services.AddOptions();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings.BaseApiUrl.Length == 0 || appSettings.ApplicationID.Length == 0 || appSettings.ApplicationKey.Length == 0)
            {
                throw new Exception("There are missing configurations from AppSettings.json file.");
            }

            Uri tflApiEndPoint = new Uri(appSettings.BaseApiUrl);
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = tflApiEndPoint
            };

            ServicePointManager.FindServicePoint(tflApiEndPoint).ConnectionLeaseTimeout = 60000;

            services.AddScoped<IRoadRepository, RoadRepository>();
            services.AddScoped<IRoadService, RoadService>();
            services.AddScoped<IConsoleWriterUtil, ConsoleWriterUtil>();
            services.AddScoped<IExitCodeUtil, ExitCodeUtil>();
            services.AddSingleton<HttpClient>(httpClient);

            services.AddLogging(configure => configure.AddSerilog())
               .AddTransient<App>();


            services.AddScoped<App>();
        }
    }
}
