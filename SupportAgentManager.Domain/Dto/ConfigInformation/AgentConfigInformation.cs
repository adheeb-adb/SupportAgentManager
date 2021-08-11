using System.Collections.Generic;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Dto.ConfigInformation
{
    /// <summary>
    /// Class to maintain agent config information.
    /// </summary>
    public class AgentConfigInformation
    {
        public int ConcurrentChatCoefficient { get; set; }

        public Dictionary<SeniorityLevel, double> AgentEfficiencies { get; set; }

    }
}
