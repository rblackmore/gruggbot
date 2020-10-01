using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Helpers
{
    public static class MessageContentCheckHelper
    {
        internal static bool HasPrefix(DiscordSocketClient client, SocketUserMessage msg, char prefix, out int argPos)
        {
            //First character of command after prefix
            argPos = 0;

            bool hasPrefix = msg.HasCharPrefix(prefix, ref argPos);
            bool hasMentionPrefix = msg.HasMentionPrefix(client.CurrentUser, ref argPos);

            if (!hasPrefix && !hasMentionPrefix)
                return false;

            return true;
        }
    }
}
