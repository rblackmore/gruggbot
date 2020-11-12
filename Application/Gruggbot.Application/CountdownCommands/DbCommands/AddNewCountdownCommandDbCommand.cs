namespace Gruggbot.Application.CountdownCommands.DbCommands
{
    using Gruggbot.Application.Interfaces;
    using Gruggbot.Data;

    public class AddNewCountdownCommandDbCommand : IAddNewCountdownCommandDbCommand
    {
        private GruggbotContext context;

        public AddNewCountdownCommandDbCommand(GruggbotContext context)
        {
            this.context = context;
        }

        public void Execute(AddNewCountdownCommandModel model)
        {
            var domainModel = model.ToDomainModel();

            this.context.CountdownCommands.Add(domainModel);

            this.context.SaveChanges();
        }
    }
}
