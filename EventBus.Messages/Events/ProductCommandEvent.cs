using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class ProductCommandEvent : BaseEvent
    {        
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }
        
        public string Category { get; set; }
                
        public double StartingPrice { get; set; }
        
        public DateTime BidEndDate { get; set; }
    }
}
