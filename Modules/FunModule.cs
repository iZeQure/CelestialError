using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DevNet.Services;
using Discord;
using Discord.Commands;

namespace DevNet.Modules
{
    public class FunModule: ModuleBase
    {
        // Create instance of JsonHandler object.
        private JsonHandler jsonHandler = new JsonHandler();

        [Command("ping")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task PingPongAsync()
        {
            // Set Values for ping emoji.
            jsonHandler.TokenName = "serverEmoji.pingPong.name";
            jsonHandler.FilePath = "emoji.json";

            // Get JsonEmojiToken from json handler.
            string pingPongEmoji = jsonHandler.GetJsonToken();

            IGuild guild = Context.Guild;
            var user = await guild.GetUserAsync(213631805834657794);

            // Create embeded builder.
            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = Color.Blue,
                ThumbnailUrl = Context.User.GetAvatarUrl()
            };

            embed
                .AddField("Inline", "Debug", true)
                .AddField("Not Inline", "Debug", false)
                .WithFooter(footer => footer.Text = $"{user}")
                .WithCurrentTimestamp();

            // Reply User in text channel.
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }        
    }
}
