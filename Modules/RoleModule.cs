using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet.Modules
{
    public class RoleModule : ModuleBase
    {
        public async Task UserJoined(SocketGuildUser arg)
        {
            /*
             Role Divider ID's
             */
            // MEMBER ID                ~ 658317894161268764

            // Basic Member Role ID     ~ 655067634412421151

            var guild = arg.Guild;
            ulong[] userRoles = 
                {
                    655067634412421151,
                    658317894161268764
                };

            //var role = guild.GetRole(userRoles[0]);

            if (userRoles == null) return;

            if (!guild.CurrentUser.GuildPermissions.Has(GuildPermission.ManageRoles)) return;

            foreach (var role in userRoles)
            {
                var checkRole = guild.GetRole(role);

                if (checkRole == null) return;

                await arg.AddRoleAsync(checkRole);
            }
        }
    }
}
