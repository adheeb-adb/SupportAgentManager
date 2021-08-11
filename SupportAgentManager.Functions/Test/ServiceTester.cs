using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.ConfigInformation;
using Newtonsoft.Json;
using SupportAgentManager.Domain.Dto.ApiRequest;
using SupportAgentManager.Services;
using System.Linq;
using SupportAgentManager.Domain.Dto.Base;
using System.Collections.Generic;
using SupportAgentManager.Domain.Dto.Queues;
using SupportAgentManager.Domain.Dto;

namespace SupportAgentManager.Functions.Test
{
    public class ServiceTester
    {
        private readonly ISupportShiftService _supportShiftService;
        private readonly IAgentService _agentService;
        private readonly IAgentQueueService _agentQueueService;
        private readonly IChatSessionQueueService _chatSessionQueueService;
        private readonly IAgentChatCoordinatorService _agentChatCoordinatorService;
        private readonly IChatSessionService _chatSessionService;
        private readonly AgentConfigInformation _agentConfigInformation;

        public ServiceTester(
            ISupportShiftService supportShiftService,
            IAgentService agentService,
            IAgentQueueService agentQueueService,
            IChatSessionQueueService chatSessionQueueService,
            IAgentChatCoordinatorService agentChatCoordinatorService,
            IChatSessionService chatSessioService,
            AgentConfigInformation agentConfigInformation)
        {
            _supportShiftService = supportShiftService;
            _agentService = agentService;
            _agentQueueService = agentQueueService;
            _chatSessionQueueService = chatSessionQueueService;
            _agentChatCoordinatorService = agentChatCoordinatorService;
            _chatSessionService = chatSessioService;
            _agentConfigInformation = agentConfigInformation;
        }

        [FunctionName("ServiceTester")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {

                var queuedChats = _chatSessionQueueService.ChatSessionQueue.ChatQueue;
                var shift = _supportShiftService.CurrentShift;
                var agentsServiceCapacity = _agentService.AgentsServiceCapacity;
                var agentConfig = _agentConfigInformation;
                var agentsForCurrentShift = _agentService.ShiftAgents;
                var nextAvailableAgent = _agentQueueService.GetNextAvailableAgent();
                var agentQueues = _agentQueueService.AgentQueues;
                var activeChats = _chatSessionService.ActiveChats;
                var leadEff = agentConfig.AgentEfficiencies[Domain.Enums.SeniorityLevel.TeamLead];

                return new OkObjectResult(new
                {
                    ChatSessionQueueStatus = GetChatSesionQueueInformation(),
                    QueuedChats = GetQueuedChatsInformation(),
                    AgentsForCurrentShift = GetDetailsOfAgentsAssignedToShift(agentsForCurrentShift),
                    AgentQueueInformation = GetAgentQueueInformation(agentQueues),
                    ActiveChats = GetActiveChatsInformation(activeChats)
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"ERROR: {ex.Message}");
            }
        }

        private string GetChatSesionQueueInformation()
        {
            var chatQueueStatus = _chatSessionQueueService.ChatSessionQueue.IsAvailable.ToString();
            var capacity = _chatSessionQueueService.ChatSessionQueue.QueueCapacity.ToString();
            var queuedChatCount = _chatSessionQueueService.ChatSessionQueue.ChatQueue.Count.ToString();

            return $"Status IsAvailable: {chatQueueStatus}, Capacity: {capacity}, Unassigned QueuedChats: {queuedChatCount}";
        }

        private List<string> GetQueuedChatsInformation()
        {
            var queuedChatsinformation = _chatSessionQueueService.ChatSessionQueue.ChatQueue.Select(c => $"ClientName: {c.ClientName}, UnPolledCount: {c.UnpolledCount}, ChatId: {c.Id}").ToList();

            if (queuedChatsinformation.Any())
            {
                return queuedChatsinformation;
            }

            return new List<string> { "No chats queued pending for available agents." };

        }

        private List<string> GetDetailsOfAgentsAssignedToShift(List<SupportAgentDto> agentsForCurrentShift)
        {
            return agentsForCurrentShift.Select(a => $"Name: {a.Name} (ID: {a.Id}, Level: {a.SeniorityLevel} ), Capacity: {a.ConcurrentCapacity}, ActiveChatCount: {a.ActiveChats} IsInAgentQueue: {a.IsAvailable}").ToList();
        }

        private List<string> GetAgentQueueInformation(AgentQueues agetnQueues)
        {
            List<string> agentQueueInformation = new List<string>
            {
                $"JuniorQueue => Agents currently in queue: {agetnQueues.JuniorAgentsQueue.Count}",
                $"MidLevelQueue => Agents currently in queue: {agetnQueues.MidLevelAgentsQueue.Count}",
                $"SeniorQueue => Agents currently in queue: {agetnQueues.SeniorAgentsQueue.Count}",
                $"TeamLeadQueue => Agents currently in queue: {agetnQueues.TeamLeadAgentsQueue.Count}",
                $"ReserveQueue => Agents currently in queue: {agetnQueues.ReserveAgentsQueue.Count}",
            };

            return agentQueueInformation;
        }

        private List<string> GetActiveChatsInformation(List<ChatWindow> activeChats)
        {
            var activeChatsInformation = activeChats.Select(c => $"ClientName: {c.ClientName}, ChatId: {c.Id}, AssignedAgent: {c.AssignedAgent.Name}, (ID: {c.AssignedAgent.Id}, Level: {c.AssignedAgent.SeniorityLevel} ) ").ToList();

            if (activeChats.Any())
            {
                return activeChatsInformation;
            }

            return new List<string> { "No active chats at the moment."  };
        }
    }
}
