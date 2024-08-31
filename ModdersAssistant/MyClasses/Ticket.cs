using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdersAssistant.MyClasses
{
    public class Ticket
    {
        public string title;
        public TicketType type = TicketType.NewFeature;
        public bool complete;
        public string description;

        public override string ToString() {
            return $"{StringUtils.GetTicketTypeName(type)} Ticket '{title}'";
        }
    }
}
