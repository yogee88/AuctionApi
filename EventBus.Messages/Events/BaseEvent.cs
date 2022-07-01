using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BaseEvent
    {
        public Guid Id { get; set; }

        public DateTime QueueCreatedDate { get; set; }

        public BaseEvent()
        {
            Id = Guid.NewGuid();
            QueueCreatedDate = DateTime.Now;
        }
    }
}
