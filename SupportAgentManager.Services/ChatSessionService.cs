using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAgentManager.Common.Domain.Contracts.CloudStorage;
using SupportAgentManager.Common.Domain.Dto;
using SupportAgentManager.Common.Domain.Dto.CloudStorage;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto;

namespace SupportAgentManager.Services
{
    public class ChatSessionService : IChatSessionService
    {
        private readonly IAgentQueueService _agentQueueService;
        private readonly IAzureQueueService _azureQueueService;
        private readonly CloudStorageInformation _cloudStorageInformation;

        public ChatSessionService(IAgentQueueService agentQueueService, IAzureQueueService azureQueueService, CloudStorageInformation cloudStorageInformation)
        {
            _agentQueueService = agentQueueService;
            _azureQueueService = azureQueueService;
            _cloudStorageInformation = cloudStorageInformation;

            InitializeService();
        }

        public List<ChatWindow> ActiveChats { get; private set; }

        public List<ChatWindow> ClosedChats { get; private set; }

        public void AddChatToActiveChats(ChatWindow chat)
        {
            if (chat.IsActive && chat.AssignedAgent != null)
            {
                ActiveChats.Add(chat);
            }
        }

        public async Task CloseChat(string chatId)
        {
            var chat = ActiveChats.FirstOrDefault(c => c.Id == chatId);

            if (chat != null)
            {
                await CloseChat(chat);
            }
        }

        public async Task CloseChat(ChatWindow chat)
        {
            if (ActiveChats.Contains(chat))
            {
                ActiveChats.Remove(chat);
                ClosedChats.Add(chat);
            }

            var assignedAgent = chat.AssignedAgent;

            assignedAgent.ActiveChats--;

            // If the agent was un available previously reaching concurrent capacity limit, they need to be added back.
            if (!assignedAgent.IsAvailable)
            {
                assignedAgent.IsAvailable = assignedAgent.ActiveChats < assignedAgent.ConcurrentCapacity;
                _agentQueueService.AddAgentToQueue(assignedAgent);
            }

            // await _azureQueueService.SendAsync( new SystemEvent { EventName = EventNames.ChatClosedEvent, EventPayload = chat }, _cloudStorageInformation.CloudQueueNames.ChatEventQueue);
        }

        private void InitializeService()
        {
            if (ActiveChats == null)
            {
                ActiveChats = new List<ChatWindow>();
            }

            if (ClosedChats == null)
            {
                ClosedChats = new List<ChatWindow>();
            }
        }
    }
}
