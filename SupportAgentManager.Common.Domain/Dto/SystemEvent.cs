using System;
using System.Collections.Generic;
using System.Text;

namespace SupportAgentManager.Common.Domain.Dto
{
    public class SystemEvent
    {
        public string EventName { get; set; }

        public object EventPayload { get; set; }
    }
}
