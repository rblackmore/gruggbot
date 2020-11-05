using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.DomainModel
{
    public class CommandMessageImage : CommandMessage
    {
        public int ImageId { get; set; }
        public ImageDetails Image { get; set; }
    }
}
