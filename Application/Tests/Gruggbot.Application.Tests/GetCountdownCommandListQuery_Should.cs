namespace Gruggbot.Application.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoFixture;
    using Gruggbot.Application.CountdownCommands.DbQueries;
    using Gruggbot.Application.Interfaces;
    using Gruggbot.Data;
    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class GetCountdownCommandListQuery_Should
    {
        private GetCountdownCommandsQuery getCountdownCommandListQuery;

        private GruggbotContext gruggbotContext;

        [TestInitialize]
        public void Setup()
        {
            Fixture specimens = new Fixture();

            var countdownCommands =
                specimens.CreateMany<CountdownCommand>(5).AsEnumerable();

            var optionsBuilder = new DbContextOptionsBuilder<GruggbotContext>()
                .UseInMemoryDatabase("GruggbotInMemory");

            this.gruggbotContext = new GruggbotContext(optionsBuilder.Options);

            this.gruggbotContext.CountdownCommands.AddRange(countdownCommands);

            this.getCountdownCommandListQuery =
                new GetCountdownCommandsQuery(this.gruggbotContext);
        }

        [TestMethod]
        public void GetCountdownCommandListQuery_Execute_ListCountdownCommandModels()
        {
            // Act.
            var countdownCommands = this.getCountdownCommandListQuery.Execute();

            // Assert.
            Assert.IsInstanceOfType(countdownCommands, typeof(IEnumerable<CountdownCommandModel>));
        }
    }
}
