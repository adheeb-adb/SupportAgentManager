using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto;
using SupportAgentManager.Domain.Dto.ApiRequest;
using SupportAgentManager.Domain.Dto.ApiResponse;
using SupportAgentManager.Domain.Dto.ConfigInformation;
using SupportAgentManager.Domain.Dto.Queues;

namespace SupportAgentManager.Services
{
    public class ChatSessionQueueService : IChatSessionQueueService
    {
        private readonly IAgentService _agentService;
        private readonly ChatSessionQueueInformation _chatSessionQueueInformation;

        public ChatSessionQueueService(IAgentService agentService, ChatSessionQueueInformation chatSessionQueueInformation)
        {
            _agentService = agentService;
            _chatSessionQueueInformation = chatSessionQueueInformation;

            InitializeService();
        }

        public ChatSessionQueue ChatSessionQueue { get; private set;  }

        public void InitializeService()
        {
            if (ChatSessionQueue == null)
            {
                ChatSessionQueue = new ChatSessionQueue { ChatQueue = new List<ChatWindow>() };
            }

            ChatSessionQueue.QueueCapacity = GetChatSessionQueueCapacity();
            ChatSessionQueue.IsAvailable = ChatSessionQueue.ChatQueue.Count < ChatSessionQueue.QueueCapacity;
        }

        public ChatWindow GetNextChatForAgent()
        {
            return ChatSessionQueue.ChatQueue.FirstOrDefault(c => c.IsActive);
        }

        public ChatRequestResponse HandleChatRequest(ChatRequest chatRequest)
        {
            if (ChatSessionQueue.IsAvailable)
            {
                ChatWindow newChat = GetNewChatWindoForRequest(chatRequest);
                ChatSessionQueue.ChatQueue.Add(newChat);

                HandleChatAddedOrRemoved();

                return new ChatRequestResponse { ChatId = newChat.Id, Result = ApiResponse.ChatRequestOk };
            }

            return new ChatRequestResponse { Result = ApiResponse.ChatRequestRejected };
        }

        public void HandleKeepAlivePoll(string chatId)
        {
            ChatWindow polledChat = ChatSessionQueue.ChatQueue.FirstOrDefault(c => c.Id == chatId);

            if (polledChat != null)
            {
                polledChat.UnpolledCount = 0;
            }
        }

        public void RemoveChatFromQueue(ChatWindow chat)
        {
            if (ChatSessionQueue.ChatQueue.Contains(chat))
            {
                ChatSessionQueue.ChatQueue.Remove(chat);
                HandleChatAddedOrRemoved();
            }
        }

        public void ManageQueuedChats()
        {
            if (ChatSessionQueue.ChatQueue.Any())
            {
                // Set unpolled count for queued chat, and set IsActive to false for chats that have exceeded the allowed unpolled count.
                ChatSessionQueue.ChatQueue.ForEach(c =>
                {
                    c.UnpolledCount++;

                    if (c.UnpolledCount >= _chatSessionQueueInformation.AllowedUnpolledCount)
                    {
                        c.IsActive = false;
                    }
                });

                // Remove all inactive chats.
                RemoveInactiveChats();
            }
        }

        #region: private methods

        private int GetChatSessionQueueCapacity()
        {
            var calculatedCapacity = _agentService.AgentsServiceCapacity * _chatSessionQueueInformation.QueueCapacityCoefficient;
            return Convert.ToInt32(Math.Floor(calculatedCapacity));
        }

        private ChatWindow GetNewChatWindoForRequest(ChatRequest chatRequest)
        {
            ChatWindow newChat = new ChatWindow
            {
                Id = Guid.NewGuid().ToString(),
                ClientName = chatRequest.ClientName
            };

            return newChat;
        }

        private void RemoveInactiveChats()
        {
            if (ChatSessionQueue.ChatQueue.Where(c => !c.IsActive).Any())
            {
                ChatSessionQueue.ChatQueue.RemoveAll(c => !c.IsActive);

                HandleChatAddedOrRemoved();
            }
        }

        private void HandleChatAddedOrRemoved()
        {
            ChatSessionQueue.IsAvailable = ChatSessionQueue.ChatQueue.Count < ChatSessionQueue.QueueCapacity;
        }

        #endregion
    }
}
