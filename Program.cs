using System;
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

namespace DevNet
{
    /// <summary>
    /// Runs discord client, while injecting dependencies.
    /// </summary>
    public class Program
    {
        // Setup our fields
        private readonly IConfiguration config;
        private DiscordSocketClient client = new DiscordSocketClient();
        private static string logLevel;

        /// <summary>
        /// Initializing Client 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Count() != 0)
            {
                logLevel = args[0];
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            config = builder.Build();
        }

        /// <summary>
        /// Starts discord client.
        /// </summary>
        /// <returns>
        /// returns nothing, awaiting commands to be executed.
        /// </returns>
        public async Task MainAsync()
        {
            using (ServiceProvider services = ConfigureServices())
            {
                var getClient = services.GetRequiredService<DiscordSocketClient>();
                client = getClient;

                // Setup loggingservice and ready event.
                services.GetRequiredService<LoggingService>();

                await client.LoginAsync(TokenType.Bot, config["Token"]);
                await client.StartAsync();

                // Get the command handler class here.
                // Initialize async method to start things up.
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                // Create a task, that completes after a delay.
                // -1 to wait indefinitely.
                await Task.Delay(-1); 
            }
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
                .AddSingleton(config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<InteractiveService>()
                .AddLogging(configure => configure.AddSerilog());

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
