using SupportAgentManager.Domain.Dto.Base;
using SupportAgentManager.Domain.Dto.Queues;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define services related to Agent queues.
    /// </summary>
    public interface IAgentQueueService
    {
        /// <summary>
        /// Gets the AgentQues of a shift.
        /// </summary>
        AgentQueues AgentQueues { get; }

        /// <summary>
        /// Initializes the agent queues by removing agents from the previous shift  and adding agents for the current shift.
        /// </summary>
        void InitializeService();

        /// <summary>
        /// Add agent to an appropriate AgentQueue, if the agent is available and within the shift.
        /// </summary>
        /// <param name="supportAgent"> the SupportAgent to be added to a queue. </param>
        void AddAgentToQueue(SupportAgentDto supportAgent);

        /// <summary>
        /// Removes an agent from its assigned AgentQueue.
        /// </summary>
        /// <param name="supportAgent"> the SupportAgent to be removed. </param>
        void RemoveAgentFromQueue(SupportAgentDto supportAgent);

        /// <summary>
        /// Gets the next available agent, in the order of: Junior, Mid, Senior, TeamLead, Reserve, preferring lower levels.
        /// </summary>
        /// <returns>the next agent a chat can be assigned to. </returns>
        SupportAgentDto GetNextAvailableAgent();
    }
}
