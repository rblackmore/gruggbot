namespace Gruggbot.Application.CountdownCommands.DbQueries
{
    using System.Collections.Generic;

    public interface IGetCountdownCommandsQuery
    {
        IEnumerable<CountdownCommandModel> Execute();
    }
}
