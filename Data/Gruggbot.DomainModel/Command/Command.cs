namespace Gruggbot.DomainModel
{
    using System.Collections.Generic;

    public class Command
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public List<CommandAlias> Aliases { get; set; }

        public List<CommandMessage> Messages { get; set; }
    }
}
