using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevNet.Data;
using System.Diagnostics;
using DevNet.Services;
using Discord.WebSocket;
using Discord.Net;

namespace DevNet.Modules
{
    [Group("information")]
    [Alias("info")]
    public class InfoModule : ModuleBase
    {
        [Command]
        public async Task CommandInformation()
        {
            await Context.Channel.SendMessageAsync();
        }

        [Group("register")]
        [Alias("reg")]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        public class RegisterModule : ModuleBase
        {
            private JsonHandler jsonHandler = new JsonHandler();
            private LogMessage logMessage;

            [Command("account")]
            [Alias("konto", "acc")]
            [Summary("Register User with LDAP Information from AD.")]
            public async Task RegisterUserInformation(string userNickName)
            {
                IGuild guild = Context.Guild;

                jsonHandler.TokenName = "serverEmoji.heart.name";
                jsonHandler.FilePath = "emoji.json";

                MySQLQuery createQuery = new MySQLQuery();
                string[] userInformation = new string[2];

                try
                {
                    userInformation = createQuery.GetUserInformationByNickName(userNickName);
                    var user = await guild.GetUserAsync(Context.User.Id);

                    if (userInformation != null && user != null)
                    {
                        await user.ModifyAsync(x =>
                        {
                            x.Nickname = $"{userInformation[0]} {userInformation[1]}";
                        });

                        await Context.Channel.SendMessageAsync($"Successfully registered {userNickName} for {Context.User.Mention}. {jsonHandler.GetJsonToken()}");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"{userNickName} cannot be found.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logMessage = new LogMessage(
                        LogSeverity.Critical,
                        ex.Message,
                        $"Source: {ex.Source} => {ex.InnerException}"
                    );
                    return;
                }
            }
        }

        [Group("invite")]
        [Alias("inv")]
        [RequireUserPermission(GuildPermission.CreateInstantInvite)]
        public class InviteModule : ModuleBase
        {
            [Command]
            public async Task DefaulCreateInstantInvite()
            {
                IGuild channel = Context.Guild;

                var voiceChannel = await channel.GetVoiceChannelAsync(Context.Channel.Id);

                var inv = await voiceChannel.CreateInviteAsync(1, 5, false, true);

                // OOF!
            }
        }
    }
}