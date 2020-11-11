namespace Gruggbot.Application.CountdownCommands.Queries
{
    using System.Collections.Generic;

    public interface IGetCountdownCommandsQuery
    {
        IEnumerable<CountdownCommandModel> Execute();
    }
}
