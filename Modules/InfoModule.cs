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

            [Command("user")]
            [Alias("usr")]
            [Summary("Register User with LDAP Information from AD.")]
            public async Task RegisterUserInformation(string userNickName)
            {
                jsonHandler.TokenName = "serverEmoji.heart.name";
                jsonHandler.FilePath = "emoji.json";

                MySQLQuery createQuery = new MySQLQuery();
                string[] userInformation = new string[2];

                try
                {
                    userInformation = createQuery.GetUserInformationByNickName(userNickName);

                    IGuild guild = Context.Guild;
                    // Debugging
                    //var user = await guild.GetUserAsync(151363325987389440);
                    // BOT ID
                    //var user = await guild.GetUserAsync(Context.Client.CurrentUser.Id);
                    // LIVE
                    var user = await guild.GetUserAsync(Context.User.Id);

                    if (userInformation != null && user != null)
                    {
                        await user.ModifyAsync(x =>
                        {
                            x.Nickname = $"{userInformation[0]} {userInformation[1]}";
                        });

                        // Debugging
                        //await Context.Channel.SendMessageAsync($"Debugging: Registered {userNickName} for {user.Nickname}. {jsonHandler.GetJsonToken()}");
                        // Live
                        await Context.Channel.SendMessageAsync($"Successfully registered {userNickName} for {Context.User.Mention}. {jsonHandler.GetJsonToken()}");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"{userNickName} cannot be found.");
                        return;
                    }
                }
                catch (NullReferenceException ex)
                {
                    logMessage = new LogMessage(
                        LogSeverity.Error,
                        ex.Message,
                        $"Source: {ex.Source} => {ex.InnerException}"
                    );

                    await Context.Channel.SendMessageAsync($"Could not process command.. make sure you have internet or try again later.");
                }
                catch (HttpException ex)
                {
                    Debug.WriteLine(logMessage = new LogMessage(
                        LogSeverity.Error,
                        ex.Message,
                        $"Code: {ex.HttpCode} => {ex.Data}."
                    ));

                    await Context.Channel.SendMessageAsync($"Could not process command.. make sure you have internet or try again later.");
                }
                catch (Exception ex)
                {
                    logMessage = new LogMessage(
                        LogSeverity.Critical,
                        ex.Message,
                        $"Source: {ex.Source} => {ex.InnerException}"
                    );

                    await Context.Channel.SendMessageAsync($"Unspecified Exception. Contact Developer.");
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