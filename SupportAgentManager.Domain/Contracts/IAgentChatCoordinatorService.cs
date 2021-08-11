using System.Threading.Tasks;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define services for Agent - Chat coordination.
    /// </summary>
    public interface IAgentChatCoordinatorService
    {
        /// <summary>
        /// /// Will be invoked by a time triggered function at the specified interval.
        /// Get the first chat from the ChatSessionQueue and assign to an appropriate agent, if agents have capacity.
        /// </summary>
        /// <returns> a completed task.  </returns>
        Task AssignAvailableAgentToNextChatAsync();

        /// <summary>
        /// Will be invoked by a time triggered function at the specified interval OR at startup and initialization of a shift.
        /// Assign queued chats to agents while untill agent capacity is filled or ChatSessionQueue is cleared.
        /// To be called when AgentQueue is initialized at the begenning of a shift.
        /// </summary>
        /// <returns> a completed task.  </returns>
        /// 
        Task AssignAgentsToChatsAsync();
    }
}
