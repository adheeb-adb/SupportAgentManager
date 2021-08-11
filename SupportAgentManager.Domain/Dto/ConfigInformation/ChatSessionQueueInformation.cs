namespace SupportAgentManager.Domain.Dto.ConfigInformation
{
    public class ChatSessionQueueInformation
    {
        public double QueueCapacityCoefficient { get; set; }

        public int AllowedUnpolledCount { get; set; }

        public int ManageChatQueueInterval { get; set; }
    }
}
