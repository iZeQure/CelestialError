using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevNet.Services
{
    public class CommandHandler
    {
        // Setip fields to bet se inside the constructor.
        private readonly CommandService commands;
        private readonly DiscordSocketClient client;
        private readonly IServiceProvider iServices;
        private readonly ILogger logger;

        public CommandHandler(IServiceProvider services)
        {
            // Juice up the fields with Dependecy Injection services.
            commands = services.GetRequiredService<CommandService>();
            client = services.GetRequiredService<DiscordSocketClient>();
            logger = services.GetRequiredService<ILogger<CommandHandler>>();
            iServices = services;

            // Take action when we execute a command.
            commands.CommandExecuted += CommandExecutedAsync;

            // Take action when we receive a message.
            // (So we can process it, and see if it is a valid command.)
            client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), iServices);
        }

        /// <summary>
        /// Where the magic begins.
        /// Takes actions upon recieving messages.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ensure we don't process system / bot messages.
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            // Return message, if the message isn't by an User.
            if (message.Source != MessageSource.User)
            {
                return;
            }

            // Set argument position away from the prefix we set.
            var argPos = 0;

            // Get Prefix from the configuration file.
            //char prefix = char.Parse(config["Prefix"]);
            char prefix = char.Parse(ConfigurationManager.AppSettings["commandPrefix"]);

            // Determine if the message has a valid prefix, and adjust argPos based on Prefix.
            if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(client, message);

            // Execute command if one is found that matches.
            await commands.ExecuteAsync(context, argPos, iServices);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // If a command isn't found, log that info to the console, and exit this method.
            if (!command.IsSpecified)
            {
                logger.LogWarning($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }

            // Log Success to the console and exit this method.
            if (result.IsSuccess)
            {
                logger.LogInformation($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                return;
            }
            // Failure scenario, let's let the user know.
            logger.LogError($"Sorry, {context.User.Username} ... something went wrong -> [{result}]!");
            //await context.Channel.SendMessageAsync($"Sorry, {context.User.Username} ... something went wrong -> [{result}]!");
            await context.Channel.SendMessageAsync($"[{command.Value.Name}] isn't available right now, try again later.. {context.User.Username}");
        }
    }
}
