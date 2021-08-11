using System;
using System.Collections.Generic;
using System.Linq;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.Base;
using SupportAgentManager.Domain.Dto.Queues;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Services
{
    public class AgentQueueService : IAgentQueueService
    {
        private readonly IAgentService _agentService;
        private readonly ISupportShiftService _supportShiftService;

        public AgentQueueService(IAgentService agentService, ISupportShiftService supportShiftService)
        {
            _agentService = agentService;
            _supportShiftService = supportShiftService;

            InitializeService();
        }

        public AgentQueues AgentQueues { get; private set; }

        public void InitializeService()
        {
            // Initilaize agent queues for shift.
            InitializeAgentQueusForShift();
        }

        public void AddAgentToQueue(SupportAgentDto supportAgent)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            var currentShift = _supportShiftService.CurrentShift;

            if (supportAgent.IsAvailable && (currentShift.StartTime <= now && now <= currentShift.EndTime))
            {
                var agentQueue = GetQueueForAgent(supportAgent);

                agentQueue.Add(supportAgent);
            }
        }

        public void RemoveAgentFromQueue(SupportAgentDto supportAgent)
        {
            var agentQueue = GetQueueForAgent(supportAgent);

            agentQueue.Remove(supportAgent);
        }

        public SupportAgentDto GetNextAvailableAgent()
        {
            SupportAgentDto availableAgent = AgentQueues.JuniorAgentsQueue.FirstOrDefault();

            // If no junior agents available, check for mid level agents.
            if (availableAgent == null)
            {
                availableAgent = AgentQueues.MidLevelAgentsQueue.FirstOrDefault();
            }

            // If no mid level agents available, check for senior level agents.
            if (availableAgent == null)
            {
                availableAgent = AgentQueues.SeniorAgentsQueue.FirstOrDefault();
            }

            // If no senior agents available, check for team lead level agents.
            if (availableAgent == null)
            {
                availableAgent = AgentQueues.TeamLeadAgentsQueue.FirstOrDefault();
            }

            // If no junior agents available, check for reserve level agents.
            if (availableAgent == null)
            {
                availableAgent = AgentQueues.ReserveAgentsQueue.FirstOrDefault();
            }

            return availableAgent;
        }

        #region: private methods

        private void InitializeAgentQueusForShift()
        {
            var agentsForCurrentShift = _agentService.ShiftAgents;

            SetAgentQueues(agentsForCurrentShift);
        }

        private void SetAgentQueues(List<SupportAgentDto> agents)
        {
            if (AgentQueues != null)
            {
                // Remove all agents of the previous shift.
                ClearAgentQueues();
            }
            else
            {
                InitializeAgentQueuesForStartup();
            }

            if (agents.Any())
            {
                if (agents.Any(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Reserve))
                {
                    AgentQueues.ReserveAgentsQueue.AddRange(agents.Where(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Reserve));
                }

                if (agents.Any(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Junior))
                {
                    AgentQueues.JuniorAgentsQueue.AddRange(agents.Where(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Junior));
                }

                if (agents.Any(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.MidLevel))
                {
                    AgentQueues.MidLevelAgentsQueue.AddRange(agents.Where(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.MidLevel));
                }

                if (agents.Any(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Senior))
                {
                    AgentQueues.SeniorAgentsQueue.AddRange(agents.Where(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.Senior));
                }

                if (agents.Any(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.TeamLead))
                {
                    AgentQueues.TeamLeadAgentsQueue.AddRange(agents.Where(a => a.IsAvailable && a.SeniorityLevel == SeniorityLevel.TeamLead));
                }
            }
        }

        private void ClearAgentQueues()
        {
            AgentQueues.ReserveAgentsQueue.Clear();
            AgentQueues.JuniorAgentsQueue.Clear();
            AgentQueues.MidLevelAgentsQueue.Clear();
            AgentQueues.SeniorAgentsQueue.Clear();
            AgentQueues.TeamLeadAgentsQueue.Clear();
        }

        private List<SupportAgentDto> GetQueueForAgent(SupportAgentDto supportAgent)
        {
            List<SupportAgentDto> agentQueue = null;

            if (supportAgent.SeniorityLevel == SeniorityLevel.Reserve)
            {
                agentQueue = AgentQueues.ReserveAgentsQueue;
            }

            if (supportAgent.SeniorityLevel == SeniorityLevel.Junior)
            {
                agentQueue = AgentQueues.JuniorAgentsQueue;
            }

            if (supportAgent.SeniorityLevel == SeniorityLevel.MidLevel)
            {
                agentQueue = AgentQueues.MidLevelAgentsQueue;
            }

            if (supportAgent.SeniorityLevel == SeniorityLevel.Senior)
            {
                agentQueue = AgentQueues.SeniorAgentsQueue;
            }

            if (supportAgent.SeniorityLevel == SeniorityLevel.TeamLead)
            {
                agentQueue = AgentQueues.TeamLeadAgentsQueue;
            }

            return agentQueue;
        }

        private void InitializeAgentQueuesForStartup()
        {
            // Ensure AgentQueues is null
            if (AgentQueues == null)
            {
                AgentQueues = new AgentQueues()
                {
                    JuniorAgentsQueue = new List<SupportAgentDto>(),
                    MidLevelAgentsQueue = new List<SupportAgentDto>(),
                    SeniorAgentsQueue = new List<SupportAgentDto>(),
                    TeamLeadAgentsQueue = new List<SupportAgentDto>(),
                    ReserveAgentsQueue = new List<SupportAgentDto>(),
                };
            }
        }

        #endregion

    }
}
