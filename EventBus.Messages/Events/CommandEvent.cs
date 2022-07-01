using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class CommandEvent : BaseEvent
    {        
        public int ProductId { get; set; }
        
        public int BidAmount { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set; }

        public string EmailId { get; set; }

        public int BidderId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
