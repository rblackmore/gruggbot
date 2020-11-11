namespace Gruggbot.Application.CountdownCommands.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using Gruggbot.Application.Interfaces;
    using Gruggbot.DomainModel;

    public class GetCountdownCommandsQuery : IGetCountdownCommandsQuery
    {
        private IGruggbotContext context;

        public GetCountdownCommandsQuery(IGruggbotContext context)
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
