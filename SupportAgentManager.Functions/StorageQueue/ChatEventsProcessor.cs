using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupportAgentManager.Common.Domain.Dto;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto;

namespace SupportAgentManager.Functions.StorageQueue
{
    public class ChatEventsProcessor
    {
        private readonly IAgentQueueService _agentQueueService;

        public ChatEventsProcessor(IAgentQueueService agentQueueService)
        {
            _agentQueueService = agentQueueService;
        }

        [FunctionName("ChatEventsProcessor")]
        public void Run([QueueTrigger("%CloudStorageInformation:CloudQueueNames:ChatEventQueue%", Connection = "CloudStorageInformation:CloudStorageConnectionString")]string chatEvent, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {chatEvent}");

                var systemEvent = JsonConvert.DeserializeObject<SystemEvent>(chatEvent);

                if (systemEvent.EventName == EventNames.ChatClosedEvent)
                {
                    HandleChatClosedEvent((ChatWindow)systemEvent.EventPayload);
                }
            }
            catch (Exception ex)
            {
                log.LogError($"ERROR ChatEventsProcessor: {ex.Message}");
                throw;
            }
        }

        private void HandleChatClosedEvent(ChatWindow closedChat)
        {
            // Add logic for any closed chat events.
        }
    }
}
