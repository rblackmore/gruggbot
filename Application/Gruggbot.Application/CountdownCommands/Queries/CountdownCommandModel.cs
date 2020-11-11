namespace Gruggbot.Application.CountdownCommands.Queries
{
    using System;
    using Gruggbot.DomainModel;

    public class CountdownCommandModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public DateTime EndDate { get; set; }

        public string Event { get; set; }

        internal static CountdownCommandModel ToModel(Command command)
        {
            return new CountdownCommandModel
            {
                ID = command.ID,
                Name = command.Name,
                Summary = command.Summary,
                EndDate = (command as CountdownCommand).EndDate,
                Event = (command as CountdownCommand).Event,
            };
        }
    }
}
