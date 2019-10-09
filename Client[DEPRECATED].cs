using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DevNet.Services;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Logging;

namespace DevNet
{
    public class Client
    {
        //// Setup our fields
        //private readonly IConfiguration configuration;
        //private DiscordSocketClient emptyClient;

        //public Client()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //        .AddJsonFile(path: "config.json");

        //    configuration = builder.Build();
        //}

        //public async Task ClientAsync()
        //{
        //    using (var services = ConfigureServices())
        //    {
        //        var client = services.GetRequiredService<DiscordSocketClient>();
        //        emptyClient = client;

        //        // Setup loggingservice and ready event.
        //        services.GetRequiredService<LoggingService>();

        //        await client.LoginAsync(TokenType.Bot, configuration["Token"]);
        //        await client.StartAsync();

        //        // Get the command handler class here.
        //        // Initialize async method to start things up.
        //        await services.GetRequiredService<CommandHandler>().InitializeAsync();

        //        // Create a task, that completes after a delay.
        //        // -1 to wait indefinitely.
        //        await Task.Delay(-1);
        //    }
        //}

        //private ServiceProvider ConfigureServices()
        //{
        //    var services = new ServiceCollection()
        //        .AddSingleton(configuration)
        //        .AddSingleton<DiscordSocketClient>()
        //        .AddSingleton<CommandService>()
        //        .AddSingleton<CommandHandler>()
        //        .AddSingleton<LoggingService>()
        //        .AddLogging(configure => configure.AddSerilog());


        //    if (!string.IsNullOrEmpty(logLevel.ToLower()))
        //    {
        //        switch (logLevel.ToLower())
        //        {
        //            case "info":
        //                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
        //                break;
        //            case "error":
        //                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
        //                break;
        //            case "debug":
        //                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
        //                break;
        //            default:
        //                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
        //    }

        //    var serviceProvider = services.BuildServiceProvider();
        //    return serviceProvider;
        //}
    }
}
