using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.DomainModel
{
    public class Command
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<CommandAlias> Aliases { get; set; }
        public List<CommandMessage> Messages { get; set; }
    }
}
