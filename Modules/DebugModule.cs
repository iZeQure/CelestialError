﻿using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevNet.Modules
{
    [Group("debug")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class DebugModule : InteractiveBase
    {
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
    }
}
