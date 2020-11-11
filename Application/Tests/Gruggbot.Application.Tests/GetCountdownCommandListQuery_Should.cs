using AutoFixture;
using Gruggbot.Application.CountdownCommands.Queries;
using Gruggbot.Application.Interfaces;
using Gruggbot.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Gruggbot.Application.Tests
{
    [TestClass]
    public class GetCountdownCommandListQuery_Should
    {

        private GetCountdownCommandsQuery getCountdownCommandListQuery;

        [TestInitialize]
        public void Setup()
        {
            Fixture specimens = new Fixture();
            
            var countdownCommands = 
                specimens.CreateMany<CountdownCommand>(5).AsQueryable();

            var commandMock = new Mock<DbSet<CountdownCommand>>();

            commandMock.As<IQueryable<CountdownCommand>>()
                .Setup(m => m.Provider).Returns(countdownCommands.Provider);

            commandMock.As<IQueryable<CountdownCommand>>()
                .Setup(m => m.Expression).Returns(countdownCommands.Expression);

            commandMock.As<IQueryable<CountdownCommand>>()
                .Setup(m => m.ElementType).Returns(countdownCommands.ElementType);

            commandMock.As<IQueryable<CountdownCommand>>()
                .Setup(m => m.GetEnumerator()).Returns(countdownCommands.GetEnumerator());

            var mockContext = new Mock<IGruggbotContext>();
            mockContext.Setup(c => c.CountdownCommands).Returns(commandMock.Object);

            this.getCountdownCommandListQuery = new GetCountdownCommandsQuery(mockContext.Object);
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
