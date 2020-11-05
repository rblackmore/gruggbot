using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.DomainModel
{
    public class CountdownCommand : Command
    {
        public DateTime EndDate { get; set; }
        public string Event { get; set; }

        public string ConstructCountdownMessage()
        {
            // TODO: Construct this message based on template message stored in CommandMessages List.
            return "{event} will be released in {countdown} on {date} at {time}";
        }
    }
}
