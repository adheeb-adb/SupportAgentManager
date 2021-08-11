using System.Collections.Generic;
using SupportAgentManager.Domain.Dto.Base;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define services and properties related to shift agents.
    /// </summary>
    public interface IAgentService
    {
        /// <summary>
        /// Gets agents assigned to the current shift.
        /// </summary>
        List<SupportAgentDto> ShiftAgents { get; }

        /// <summary>
        /// Gets the capacity of all agents assigned to a shift.
        /// </summary>
        int AgentsServiceCapacity { get; }

        /// <summary>
        /// Gets agents assigned to a particular shift.
        /// </summary>
        /// <param name="shiftType"> the shift type the agents belong to. </param>
        /// <returns> list of SupportAgentDto objects. </returns>
        List<SupportAgentDto> GetAgentsForShift(SupportShiftType shiftType);

        /// <summary>
        /// Gets all agents.
        /// </summary>
        /// <returns>list of all agents. </returns>
        List<SupportAgentDto> GetAllAgents();

        /// <summary>
        /// Initializes the service by setting the agents for the current shift and setting the AgentServiceCapacity. To be called at startup and the beginning of a shift.
        /// </summary>
        /// <param name="agents"> a list of agents that need to be assigned to the current shift. </param>
        void InitializeService();
    }
}
