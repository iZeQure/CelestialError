using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevNet.Services
{
    public class LoggingService
    {
        // Declared Fields, used later in class.
        private readonly ILogger logger;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;

        public LoggingService(IServiceProvider services)
        {
            // Get Services we need via Dependecy Injection.
            // Assign and declare them aboce.
            discord = services.GetRequiredService<DiscordSocketClient>();
            commands = services.GetRequiredService<CommandService>();
            logger = services.GetRequiredService<ILogger<LoggingService>>();

            discord.Ready += OnReadyAsync;
            discord.Log += OnLogAsync;
            commands.Log += OnLogAsync;
        }

        public Task OnReadyAsync()
        {
            logger.LogInformation($"Connected as -> [{discord.CurrentUser}].");
            logger.LogInformation($"We are on [{discord.Guilds.Count}] servers.");
            return Task.CompletedTask;
        }

        public Task OnLogAsync(LogMessage message)
        {
            string logText = $"{message.Source}: {message.Exception?.ToString() ?? message.Message}";

            switch (message.Severity.ToString())
            {
                case "Critical":
                {
                    logger.LogCritical(logText);
                    break;
                }
                case "Warning":
                {
                    logger.LogWarning(logText);
                    break;
                }
                case "Info":
                {
                    logger.LogInformation(logText);
                    break;
                }
                case "Verbose":
                {
                    logger.LogInformation(logText);
                    break;
                }
                case "Debug":
                {
                    logger.LogDebug(logText);
                    break;
                }
                case "Error":
                {
                    logger.LogError(logText);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
