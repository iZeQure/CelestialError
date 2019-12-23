using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevNet.Services;
using Discord.WebSocket;
using System.Linq;
using System.Diagnostics;

namespace DevNet.Modules
{
    [Group("debug")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class DebugModule : InteractiveBase
    {
        [Command("ldap", RunMode = RunMode.Async)]
        public async Task Debug_Ldap(string commonName)
        {
            LdapService ldap = new LdapService();

            if (commonName != "" && commonName.Length != 0)
            {
                var getInfo = ldap.LdapConnection(commonName);
                if (getInfo != null)
                {
                    await ReplyAsync($"Information: {getInfo[0]} {getInfo[1]}");
                }
            }
            else
            {
                await ReplyAsync($"Missing parameter: Cannot be null.");
            }
        }

        [Command("debugPagination")]
        [Alias("dp")]
        [Summary("Command used for debugging commands for testing phases.")]
        public async Task Debug_PaginatorAsync()
        {
            var pages = new[]
            {
                "Page 1\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Page 2\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Page 3\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Page 4\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                "Page 5\n" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };

            await PagedReplyAsync(pages, false);
        }

        [Command("debugReplyDelete")]
        [Alias("del")]
        [Summary("Deletes a message by a specified amount of time.")]
        public async Task<RuntimeResult> Debug_DeleteAfterAsync(int userTimer)
        {
            await ReplyAndDeleteAsync($"This message will be deleted in {userTimer} seconds.", timeout: TimeSpan.FromSeconds(userTimer));
            return Ok();
        }

        [Command("debugNext", RunMode = RunMode.Async)]
        public async Task Debug_ReplyTimeoutAsync()
        {
            await ReplyAsync($"What is 2+2?");
            var response = await NextMessageAsync();
            if (response != null)
            {
                await ReplyAsync($"You replied: {response.Content}");
            }
            else
            {
                await ReplyAsync($"You did no reply before the timeout.");
            }
        }

        [Command("channel", RunMode = RunMode.Async)]
        public async Task CreateCustomChannelAsync(string channelName, int channelMaxEntries)
        {
            IGuild guild = Context.Guild;
            IUserMessage msg = Context.Message;
            IChannel channel = Context.Channel;

            TimeSpan channelTimer = TimeSpan.FromMinutes(2);

            var channels = await guild.GetChannelsAsync();
            var targetChannel = channels.FirstOrDefault(target => target.Name == "create-new-channel");

            if (channel.Name != targetChannel.Name)
            {
                await Context.Channel.DeleteMessageAsync(msg);
                return;
            }

            var categories = await guild.GetCategoriesAsync();
            var targetCategory = categories.FirstOrDefault(target => target.Name == "CustomChannel");

            if (targetCategory == null) return;

            var createVoiceChannel = await guild.CreateVoiceChannelAsync(channelName, properties =>
            {
                properties.CategoryId = targetCategory.Id;
                properties.UserLimit = channelMaxEntries;
            });

            await DeleteMessageAfterCommand(msg);
        }

        public async Task DeleteMessageAfterCommand(IUserMessage msg)
        {
            var getMessages = await msg.Channel.GetMessagesAsync(1).FlattenAsync();

            foreach (IUserMessage message in getMessages)
            {
                await message.DeleteAsync();
            }
            await Task.CompletedTask;
        }
    }
}
