namespace Gruggbot.Core.Configuration
{
    public class BotConfiguration
    {
        public const string Bot = "Bot";

        public string Token { get; set; }

        public CommandHandlerConfiguration Commands { get; set; }

    }
}
