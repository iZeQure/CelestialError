﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;
using DevNet.Services;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.Addons.Interactive;
using System.Configuration;
using DevNet.Modules;
using System.Diagnostics;
using System.IO;

namespace DevNet
{
    /// <summary>
    /// Runs discord client, while injecting dependencies.
    /// </summary>
    public class Program
    {
        //internal static ConfigurationBuilders confBuilder;

        // Setup our fields
        private ConfigurationBuilders builders;
        private DiscordSocketClient client;
        private static string _logLevel;

        /// <summary>
        /// Initializing Client 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Count() != 0)
            {
                _logLevel = args[0];
            }
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/devnet.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Builds Configuration file.
        /// </summary>
        public Program()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("config.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            builders = new ConfigurationBuilders
            {
                ConnString = ConfigurationManager.ConnectionStrings["CelestialError"]?.ConnectionString,
                SmsMessageKey = ConfigurationManager.AppSettings["SmsMessageKey"]
            };
        }

        /// <summary>
        /// Starts discord client.
        /// </summary>
        /// <returns>
        /// returns nothing, awaiting commands to be executed. 
        /// </returns>
        public async Task MainAsync()
        {
            RoleModule roleModule = new RoleModule();

            using ServiceProvider services = ConfigureServices();

            var socketConfig = new DiscordSocketConfig
            {
                ExclusiveBulkDelete = true
            };
            client = new DiscordSocketClient(socketConfig);

            DiscordSocketClient getClient = services.GetRequiredService<DiscordSocketClient>();
            client = getClient;

            client.UserJoined += roleModule.UserJoined;

            // Setup loggingservice and ready event. 
            services.GetRequiredService<LoggingService>();

            //await client.LoginAsync(TokenType.Bot, config["Token"]);
            await client.LoginAsync(TokenType.Bot, token: $"{Environment.GetEnvironmentVariable("BotToken")}", true);
            await client.StartAsync();

            // Get the command handler class here.
            // Initialize async method to start things up.
            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            // Create a task, that completes after a delay.
            // -1 to wait indefinitely.
            await Task.Delay(-1);
        }

        /// <summary>
        /// Configuration of service dependensies. 
        /// </summary>
        /// <returns>
        /// Return a ServiceCollection.
        /// </returns>
        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<ConfigurationBuilders>()
                .AddLogging(configure => configure.AddSerilog());

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
