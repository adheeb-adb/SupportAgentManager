using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SupportAgentManager.Domain.Contracts;

namespace SupportAgentManager.Functions.Schedule
{
    public class AgentAndQueueMonitor
    {
        IAgentChatCoordinatorService _agentChatCoordinatorService;
        IChatSessionQueueService _chatSessionQueueService;

        public AgentAndQueueMonitor(IAgentChatCoordinatorService agentChatCoordinatorService, IChatSessionQueueService chatSessionQueueService)
        {
            _agentChatCoordinatorService = agentChatCoordinatorService;
            _chatSessionQueueService = chatSessionQueueService;
        }

        [FunctionName("AgentAndQueueMonitor")]
        public void Run([TimerTrigger("%MonitorServiceCron%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function AgentAndQueueMonitor executed at: {DateTime.Now}");

            try
            {
                _agentChatCoordinatorService.AssignAgentsToChatsAsync();
                _chatSessionQueueService.ManageQueuedChats();
            }
            catch (Exception ex)
            {
                log.LogError($"ERROR executing AgentAndQueueMonitor: {ex.Message}");
                throw;
            }
        }
    }
}
