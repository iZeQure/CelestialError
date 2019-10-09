using DevNet.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevNet.Modules
{
    [Group("admin")]
    [RequireBotPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase
    {
        [Group("sms")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class SmSModule : ModuleBase
        {
            [Command]
            [Summary("Send custom message to recipient as sms.")]
            public async Task DefaultMessageAsync(string phoneNr, [Remainder]string message)
            {
                SMSService smsService = new SMSService($"https://");

                // Old method..
                //Regex regex = new Regex(@"^\d$");

                if ((phoneNr != "" || phoneNr.Length != 0) && (message != "" || message.Length != 0))
                {
                    // Old method..
                    //Regex isMatchedReg = new Regex("^[0-9]+$");

                    if (Regex.IsMatch(phoneNr, "^[0-9]*$"))
                    {
                        smsService.SendSms(phoneNr, message);
                        await Context.User.SendMessageAsync($"Successfully Sent message to {phoneNr} with message: {message} - by {Context.User.Username}");
                    }
                    else
                    {
                        await ReplyAsync($"Number containing invalid characters.");
                    }
                }
                return;
            }
        }

        [Group("clean")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public class CleanModule : ModuleBase
        {
            // [prefix]admin clean
            [Command]
            [Summary("Delete Message. [Deletes latest message in channel.]")]
            public async Task DefaultCleanAsync()
            {
                await Context.Channel.DeleteMessageAsync(Context.Message);
                //await Context.Channel.SendMessageAsync($"Successfully Deleted Latest messasge.");
            }

            [Command("messages", RunMode = RunMode.Async)]
            [Alias("msg", "bulk")]
            [Summary("Bulk Delete Messages. [2 / 100]")]
            public async Task CleanBulkAsync(int count)
            {
                // Add a constant delay for Task's.
                const int delay = 3000;

                // Get messages from channel, which command is executed in.
                IEnumerable<IMessage> getMessages = await Context.Channel.GetMessagesAsync(count + 1).FlattenAsync();

                switch (count)
                {
                    case int messageCounter when (messageCounter >= 2 && messageCounter <= 100):
                        await ((ITextChannel)Context.Channel).DeleteMessagesAsync(getMessages);
                        // Reply to User who executed command.
                        IUserMessage userMessage = await ReplyAsync($"Cleaned [{Context.Channel.Name}] for {Context.User.Username}.");
                        await Task.Delay(delay);
                        await userMessage.DeleteAsync();
                        break;

                    default:
                        // Give user error.
                        await ((ITextChannel)Context.Channel).DeleteMessageAsync(1);
                        IUserMessage errorUserMessage = await ReplyAsync($"{Context.User.Username} choose between 2 - 100.");
                        await Task.Delay(delay);
                        await errorUserMessage.DeleteAsync();
                        break;
                }
            }
        }

        [Group("emoji")]
        [RequireBotPermission(GuildPermission.ManageEmojis)]
        public class EmojiModule : ModuleBase
        {
            //[Command("add")]
            //[Summary("Add a new emoji to the guild.")]
            //public async Task AddEmojiAsync(string emojiName, Image emojiImage, bool isEmojiAnimated)
            //{

            //}
        }

        [Command("ban")]
        [Summary("Bans a user, specified by days & a reason.")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public Task BanAsync(IGuildUser user, int pruneDays, string banReason) =>
            Context.Guild.AddBanAsync(user, pruneDays, banReason);
    }
}
