using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.DomainModel
{
    public class CommandAlias
    {
        public int ID { get; set; }
        public int CommandId { get; set; }
        public string Alias { get; set; }
    }
}
