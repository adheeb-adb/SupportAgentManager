using System;
using System.Collections.Generic;
using System.Linq;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.Base;
using SupportAgentManager.Domain.Dto.ConfigInformation;
using SupportAgentManager.Domain.Entities;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Services
{
    public class AgentService : IAgentService
    {
        private readonly ISupportShiftService _supportShiftService;
        private readonly AgentConfigInformation _agentConfigInformation;

        public AgentService(ISupportShiftService supportShiftService, AgentConfigInformation agentConfigInformation)
        {
            _supportShiftService = supportShiftService;
            _agentConfigInformation = agentConfigInformation;
            InitializeService();
        }

        public List<SupportAgentDto> ShiftAgents { get; private set;  }

        public int AgentsServiceCapacity { get; private set;  }

        public List<SupportAgentDto> GetAgentsForShift(SupportShiftType shiftType)
        {
            List<SupportAgentDto> agents = MapAgentsToDto(GetAgents().Where(a => a.ShiftType == shiftType).ToList());

            return agents;
        }

        public List<SupportAgentDto> GetAllAgents()
        {
            return MapAgentsToDto(GetAgents());
        }

        public void InitializeService()
        {
            SetShiftAgentsAndCapacity();
        }


        #region: private methods

        private void SetShiftAgentsAndCapacity()
        {
            List<SupportAgentDto> agentsForCurrentShift = GetAgentsForShift(_supportShiftService.CurrentShift.ShiftType);

            ShiftAgents = agentsForCurrentShift;

            AgentsServiceCapacity = ShiftAgents.Sum(a => a.ConcurrentCapacity);
        }

        private List<SupportAgentDto> MapAgentsToDto(List<SupportAgent> agents)
        {
            List<SupportAgentDto> agentDtos = agents.Select(a => new SupportAgentDto
            {
                Id = a.Id,
                Name = a.Name,
                SeniorityLevel = a.SeniorityLevel,
                ShiftType = a.ShiftType,
                ConcurrentCapacity = GetAgentConcurrentCapacity(a.SeniorityLevel)

            }).ToList();

            return agentDtos;
        }

        private int GetAgentConcurrentCapacity(SeniorityLevel agentSeniorityLevel)
        {
            double calculatedEfficiency = _agentConfigInformation.AgentEfficiencies[agentSeniorityLevel] * _agentConfigInformation.ConcurrentChatCoefficient;
            int agentCapacity = Convert.ToInt32(Math.Floor(calculatedEfficiency));

            return agentCapacity;
        }

        // Method used to define agents. Can be removed once DB is setup.
        private List<SupportAgent> GetAgents()
        {
            List<SupportAgent> supportAgents = new List<SupportAgent>
            {
                /*new SupportAgent { Id = 1, Name = "Orlando Baird", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 2, Name = "Saniya Le", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 3, Name = "Adrian Duarte", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 4, Name = "Johnathon Noble", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 5, Name = "Daniela Shea", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 6, Name = "Shaun Yang", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 7, Name = "Harold Kirk", SeniorityLevel = SeniorityLevel.Junior, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 8, Name = "Kamryn Forbes", SeniorityLevel = SeniorityLevel.Junior, ShiftType = SupportShiftType.Mid },
                new SupportAgent { Id = 9, Name = "Annabel Wise", SeniorityLevel = SeniorityLevel.Junior, ShiftType = SupportShiftType.Mid },
                new SupportAgent { Id = 10, Name = "Miya Gillespie", SeniorityLevel = SeniorityLevel.MidLevel, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 11, Name = "Rudy Mccormick", SeniorityLevel = SeniorityLevel.MidLevel, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 12, Name = "Zackery Oconnell", SeniorityLevel = SeniorityLevel.MidLevel, ShiftType = SupportShiftType.Mid },
                new SupportAgent { Id = 13, Name = "Wade James", SeniorityLevel = SeniorityLevel.MidLevel, ShiftType = SupportShiftType.Night },
                new SupportAgent { Id = 14, Name = "Sonny Horton", SeniorityLevel = SeniorityLevel.MidLevel, ShiftType = SupportShiftType.Night },
                new SupportAgent { Id = 15, Name = "Hadley Mccarty", SeniorityLevel = SeniorityLevel.TeamLead, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 16, Name = "Nigel Flowers", SeniorityLevel = SeniorityLevel.Senior, ShiftType = SupportShiftType.Mid },*/

                // Added for teseting, Cane be uncommentd to test out scenarios
                new SupportAgent { Id = 17, Name = "Jeremy Clarkson", SeniorityLevel = SeniorityLevel.Junior, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 18, Name = "James May", SeniorityLevel = SeniorityLevel.Junior, ShiftType = SupportShiftType.Business },
                new SupportAgent { Id = 19, Name = "Richard Hammond", SeniorityLevel = SeniorityLevel.Reserve, ShiftType = SupportShiftType.Business },

            };

            return supportAgents;
        }

        #endregion
    }
}
