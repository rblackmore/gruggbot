using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Games")]
    public class GeneralGamesModule : ModuleBase
    {
        public async Task Roll(int max = 6, int min = 1)
        {
            Random rando = new Random();

            await Context.Channel.SendMessageAsync(rando.Next(min, max).ToString());
        }
    }
}
