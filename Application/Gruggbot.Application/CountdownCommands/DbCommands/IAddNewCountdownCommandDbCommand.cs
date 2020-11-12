namespace Gruggbot.Application.CountdownCommands.DbCommands
{
    public interface IAddNewCountdownCommandDbCommand
    {
        void Execute(AddNewCountdownCommandModel model);
    }
}
