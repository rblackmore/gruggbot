namespace Gruggbot.Core.Helpers
{
    using Discord.WebSocket;

    public static class SocketMessageExtensions
    {
        internal static bool TryCastSocketUserMessage(this SocketMessage message, out SocketUserMessage userMessage)
        {
            bool isUserMessage = false;

            userMessage = message as SocketUserMessage;

            if (userMessage != null)
                isUserMessage = true;

            return isUserMessage;
        }

        internal static bool IsUserMentioned(this SocketUserMessage message, SocketUser user)
        {
            foreach (SocketUser mentionedUser in message.MentionedUsers)
            {
                if (mentionedUser.Id == user.Id)
                    return true;
            }

            return false;
        }
    }
}
