// <copyright file="SocketMessageExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Extensions
{
    using Discord.Commands;
    using Discord.WebSocket;

    public static class SocketMessageExtensions
    {
        internal static bool TryCastSocketUserMessage(this SocketMessage message,
            out SocketUserMessage userMessage)
        {
            bool isUserMessage = false;

            userMessage = message as SocketUserMessage;

            if (userMessage != null)
                isUserMessage = true;

            return isUserMessage;
        }

        internal static bool IsUserMentioned(this SocketUserMessage userMessage, SocketUser user)
        {
            foreach (SocketUser mentionedUser in userMessage.MentionedUsers)
            {
                if (mentionedUser.Id == user.Id)
                    return true;
            }

            return false;
        }

        internal static bool HasPrefix(this SocketUserMessage userMessage, SocketUser user,
            char prefix, out int argPos)
        {
            argPos = -1;

            var hasCharPrefix = userMessage.HasCharPrefix(prefix, ref argPos);
            var hasMentionPrefix = userMessage.HasMentionPrefix(user, ref argPos);

            return hasCharPrefix || hasMentionPrefix;
        }
    }
}