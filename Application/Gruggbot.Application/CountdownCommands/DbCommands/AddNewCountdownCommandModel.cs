namespace Gruggbot.Application.CountdownCommands.DbCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Gruggbot.DomainModel;

    public class AddNewCountdownCommandModel
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public DateTime EndDate { get; set; }

        public string Event { get; set; }

        public List<string> Aliases { get; set; }

        public Dictionary<int, string> Messages { get; set; }

        internal CountdownCommand ToDomainModel()
        {
            var aliases =
                new List<CommandAlias>(this.Aliases
                    .Select(a => new CommandAlias { Alias = a }));

            var messages =
                new List<CommandMessage>(this.Messages
                    .Select(m => new CommandMessage { Sequence = m.Key, Text = m.Value }));

            return new CountdownCommand
            {
                Name = this.Name,
                Summary = this.Summary,
                EndDate = this.EndDate,
                Event = this.Event,
                Aliases = aliases,
                Messages = messages,
            };
        }
    }
}
