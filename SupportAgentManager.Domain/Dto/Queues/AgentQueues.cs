using System.Collections.Generic;
using SupportAgentManager.Domain.Dto.Base;

namespace SupportAgentManager.Domain.Dto.Queues
{
    /// <summary>
    /// A class to maintain agent queues.
    /// </summary>
    public class AgentQueues
    {
        /// <summary>
        /// Gets or sets queue for ReserveAgents.
        /// </summary>
        public List<SupportAgentDto> ReserveAgentsQueue { get; set; }

        /// <summary>
        /// Gets or sets queue for JuniorAgents.
        /// </summary>
        public List<SupportAgentDto> JuniorAgentsQueue { get; set; }

        /// <summary>
        /// Gets or sets queue for MidLevelAgents.
        /// </summary>
        public List<SupportAgentDto> MidLevelAgentsQueue { get; set; }

        /// <summary>
        /// Gets or sets queue for SeniorAgents.
        /// </summary>
        public List<SupportAgentDto> SeniorAgentsQueue { get; set; }

        /// <summary>
        /// Gets or sets queue for LeadAgents.
        /// </summary>
        public List<SupportAgentDto> TeamLeadAgentsQueue { get; set; }
    }
}
