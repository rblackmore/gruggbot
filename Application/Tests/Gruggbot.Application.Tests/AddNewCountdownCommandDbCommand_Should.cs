namespace Gruggbot.Application.Tests
{
    using System.Linq;

    using AutoFixture;
    using Gruggbot.Application.CountdownCommands.DbCommands;
    using Gruggbot.Data;
    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AddNewCountdownCommandDbCommand_Should
    {
        [TestMethod]
        public void AddNewCountdownCommandDbCommand_Execute_AddsOneCountdownCommand()
        {
            // Setup.
            var dbName =
                nameof(this.AddNewCountdownCommandDbCommand_Execute_AddsOneCountdownCommand);

            var optionsBuilder = new DbContextOptionsBuilder<GruggbotContext>()
                .UseInMemoryDatabase(dbName);

            using var context = new GruggbotContext(optionsBuilder.Options);

            var addNewCountdownCommandDbCommand =
                new AddNewCountdownCommandDbCommand(context);

            // Act.
            var addCommand = new Fixture().Create<AddNewCountdownCommandModel>();

            addNewCountdownCommandDbCommand.Execute(addCommand);

            // Assert.
            using var newContext = new GruggbotContext(optionsBuilder.Options);
            Assert.IsTrue(newContext.CountdownCommands.Any());
        }

        [TestMethod]
        public void AddNewCountdownCommandDbCommand_Execute_CountdownCommandHasAliases()
        {
            // Setup.
            var dbName =
                nameof(this.AddNewCountdownCommandDbCommand_Execute_CountdownCommandHasAliases);

            var optionsBuilder = new DbContextOptionsBuilder<GruggbotContext>()
                .UseInMemoryDatabase(dbName);

            using var context = new GruggbotContext(optionsBuilder.Options);

            var addNewCountdownCommandDbCommand =
                new AddNewCountdownCommandDbCommand(context);

            // Act.
            var addCommand = new Fixture().Create<AddNewCountdownCommandModel>();

            addNewCountdownCommandDbCommand.Execute(addCommand);

            // Assert.
            using var newContext = new GruggbotContext(optionsBuilder.Options);
            CountdownCommand subject = newContext.CountdownCommands
                .Include(c => c.Aliases)
                .First();

            Assert.IsTrue(subject.Aliases.Count > 0);
        }

        [TestMethod]
        public void AddNewCountdownCommandDbCommand_Execute_CountdownCommandHasMessages()
        {
            // Setup.
            var dbName =
                nameof(this.AddNewCountdownCommandDbCommand_Execute_CountdownCommandHasMessages);

            var optionsBuilder = new DbContextOptionsBuilder<GruggbotContext>()
                .UseInMemoryDatabase(dbName);

            using var context = new GruggbotContext(optionsBuilder.Options);

            var addNewCountdownCommandDbCommand =
                new AddNewCountdownCommandDbCommand(context);

            // Act.
            var addCommand = new Fixture().Create<AddNewCountdownCommandModel>();

            addNewCountdownCommandDbCommand.Execute(addCommand);

            // Assert.
            using var newContext = new GruggbotContext(optionsBuilder.Options);
            CountdownCommand subject = newContext.CountdownCommands
                .Include(c => c.Messages)
                .First();

            Assert.IsTrue(subject.Messages.Count > 0);
        }
    }
}