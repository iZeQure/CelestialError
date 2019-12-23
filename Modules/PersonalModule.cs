using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevNet.Modules
{
    [Group("me")]
    public class PersonalModule : ModuleBase
    {
        [Command]
        public async Task MeDefaultAsync()
        {
        }
    }
}
