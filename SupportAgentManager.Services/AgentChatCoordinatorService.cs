using System.Linq;
using System.Threading.Tasks;
using SupportAgentManager.Common.Domain.Contracts.CloudStorage;
using SupportAgentManager.Common.Domain.Dto;
using SupportAgentManager.Common.Domain.Dto.CloudStorage;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto;
using SupportAgentManager.Domain.Dto.Base;

namespace SupportAgentManager.Services
{
    public class AgentChatCoordinatorService : IAgentChatCoordinatorService
    {
        private readonly IAgentQueueService _agentQueueService;
        private readonly IChatSessionQueueService _chatSessionQueueService;
        private readonly IChatSessionService _chatSessionService;
        private readonly IAzureQueueService _azureQueueService;
        private readonly CloudStorageInformation _cloudStorageInformation;

        public AgentChatCoordinatorService(
            IAgentQueueService agentQueueService,
            IChatSessionQueueService chatSessionQueueService,
            IChatSessionService chatSessionService,
            IAzureQueueService azureQueueService,
            CloudStorageInformation cloudStorageInformation)
        {
            _agentQueueService = agentQueueService;
            _chatSessionQueueService = chatSessionQueueService;
            _chatSessionService = chatSessionService;
            _azureQueueService = azureQueueService;
            _cloudStorageInformation = cloudStorageInformation;
        }

        public async Task AssignAvailableAgentToNextChatAsync()
        {
            var availableAgent = _agentQueueService.GetNextAvailableAgent();
            var nextQueuedChat = _chatSessionQueueService.GetNextChatForAgent();

            if (availableAgent != null && nextQueuedChat != null)
            {
                await AssignAgentToChat(availableAgent, nextQueuedChat);
            }
        }

        public async Task AssignAgentsToChatsAsync()
        {
            SupportAgentDto availableAgent = _agentQueueService.GetNextAvailableAgent();
            ChatWindow nextQueuedChat = _chatSessionQueueService.GetNextChatForAgent();

            while (availableAgent != null && nextQueuedChat != null)
            {
                await AssignAgentToChat(availableAgent, nextQueuedChat);

                availableAgent = _agentQueueService.GetNextAvailableAgent();
                nextQueuedChat = _chatSessionQueueService.GetNextChatForAgent();
            }
        }

        private async Task AssignAgentToChat(SupportAgentDto agent, ChatWindow chat)
        {
            chat.AssignedAgent = agent;
            agent.ActiveChats++;
            agent.IsAvailable = agent.ActiveChats < agent.ConcurrentCapacity;

            // Remove the agent from the front of the que and add it back if capacity is still available to the back of the queue.
            _agentQueueService.RemoveAgentFromQueue(agent);
            _agentQueueService.AddAgentToQueue(agent);

            // Remove the chat from the session queue and assign to the ActiveChats.
            _chatSessionQueueService.RemoveChatFromQueue(chat);

            _chatSessionService.ActiveChats.Add(chat);

            // await _azureQueueService.SendAsync(new SystemEvent { EventName = EventNames.AgentAssignedToChatEvent, EventPayload = agent }, _cloudStorageInformation.CloudQueueNames.AgentEventQueue);
        }
    }
}
