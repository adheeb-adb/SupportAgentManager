using System;
using System.Collections.Generic;
using System.Text;

namespace SupportAgentManager.Domain.Constants
{
    /// <summary>
    ///  This class defines the event names.
    /// </summary>
    public static class EventNames
    {
        public static readonly string AgentAssignedToChatEvent = "AGENT_ASSIGNED_TO_CHAT";
        public static readonly string ChatClosedEvent = "CHAT_CLOSED";
    }
}
