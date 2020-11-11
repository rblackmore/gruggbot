namespace Gruggbot.Application.Interfaces
{
    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;

    public interface IGruggbotContext
    {
        DbSet<Command> Commands { get; }

        DbSet<CountdownCommand> CountdownCommands { get; }
    }
}
