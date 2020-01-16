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
    /// <summary>
    /// Commands Handler from Discord Servers.
    /// </summary>
    /// <remarks>
    /// When commands are executed by a user, from a Discord Server,
    /// this class is used to handle what command is executed.
    /// </remarks>
    public class CommandHandler
    {
        // Setip fields to bet se inside the constructor.
        private readonly ConfigurationBuilders _builders;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public CommandHandler(IServiceProvider services)
        {
            // Juice up the fields with Dependecy Injection services.
            _builders = services.GetRequiredService<ConfigurationBuilders>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILogger<CommandHandler>>();
            _serviceProvider = services;

            // Take action when we execute a command.
            _commands.CommandExecuted += CommandExecutedAsync;

            // Take action when we receive a message.
            // (So we can process it, and see if it is a valid command.)
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
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
            char prefix = char.Parse("!");

            // Determine if the message has a valid prefix, and adjust argPos based on Prefix.
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            // Execute command if one is found that matches.
            await _commands.ExecuteAsync(context, argPos, _serviceProvider);
        }

        /// <summary>
        /// Executed Command Information.
        /// </summary>
        /// <remarks>
        /// Logs the provided information for the executed command,
        /// displays the information inside the console application,
        /// ran on the client or cloud service.
        /// </remarks>
        /// <param name="command">The command.</param>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // If a command isn't found, log that info to the console, and exit this method.
            if (!command.IsSpecified)
            {
                _logger.LogWarning($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }

            // Log Success to the console and exit this method.
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                return;
            }
            // Failure scenario, let's let the user know.
            _logger.LogError($"Sorry, {context.User.Username} ... something went wrong -> [{result}]!");
            //await context.Channel.SendMessageAsync($"Sorry, {context.User.Username} ... something went wrong -> [{result}]!");
            await context.Channel.SendMessageAsync($"[{command.Value.Name}] isn't available right now, try again later.. {context.User.Username}");
        }
    }
}
