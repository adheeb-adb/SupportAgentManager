using SupportAgentManager.Domain.Dto.Base;

namespace SupportAgentManager.Domain.Dto
{
    public class ChatWindow
    {
        public string Id { get; set; }

        public string ClientName { get; set; }

        public SupportAgentDto AssignedAgent { get; set; }

        public bool IsActive { get; set; } = true;

        public int UnpolledCount { get; set; }
    }
}
