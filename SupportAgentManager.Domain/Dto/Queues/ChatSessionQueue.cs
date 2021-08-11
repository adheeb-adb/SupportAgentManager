using System.Collections.Generic;

namespace SupportAgentManager.Domain.Dto.Queues
{
    public class ChatSessionQueue
    {
        public bool IsAvailable { get; set; }

        public int QueueCapacity { get; set; }

        public List<ChatWindow> ChatQueue { get; set; }
    }
}
