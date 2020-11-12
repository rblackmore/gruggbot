namespace Gruggbot.Application.CountdownCommands.DbQueries
{
    using System.Collections.Generic;
    using System.Linq;

    using Gruggbot.Application.Interfaces;
    using Gruggbot.Data;
    using Gruggbot.DomainModel;

    public class GetCountdownCommandsQuery : IGetCountdownCommandsQuery
    {
        private GruggbotContext context;

        public GetCountdownCommandsQuery(GruggbotContext context)
        {
            this.context = context;
        }

        public IEnumerable<CountdownCommandModel> Execute()
        {
            var countdownCommands = this.context.CountdownCommands;

            var modelList = new List<CountdownCommandModel>();

            foreach (var command in countdownCommands)
            {
                modelList.Add(CountdownCommandModel.ToModel(command));
            }

            return modelList.AsEnumerable();
        }
    }
}
