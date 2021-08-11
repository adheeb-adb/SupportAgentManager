using SupportAgentManager.Domain.Dto;
using SupportAgentManager.Domain.Dto.ApiRequest;
using SupportAgentManager.Domain.Dto.ApiResponse;
using SupportAgentManager.Domain.Dto.Queues;
using System.Threading.Tasks;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define services for Chat session queue.
    /// </summary>
    public interface IChatSessionQueueService
    {
        /// <summary>
        /// Gets the ChatSessionQueue object handled by the ChatSession service.
        /// </summary>
        ChatSessionQueue ChatSessionQueue { get;  }

        /// <summary>
        /// Refreshes the chat session queue by updating the capacity and setting the IsAvailable (To be called during startup and when a new shift begins).
        /// </summary>
        void InitializeService();

        /// <summary>
        /// Receive a chat request and asign it to the ChatSessionQueue if capcity is available, or reject chat request if not available.
        /// set IsAvailable of the ChatSessionQueue once added.
        /// </summary>
        /// <param name="chatRequest"> chat request object with name of client. </param>
        /// <returns> returns a ChatRequestResponse with OK as the result and chat window Id if successfully queued, If not, CHAT_REJECTED is returned. </returns>
        ChatRequestResponse HandleChatRequest(ChatRequest chatRequest);

        /// <summary>
        /// Receive a keep alive request via API and reset the UnpolledCount to 0, for the Chat window corresponding to the ID passed as the parameter.
        /// </summary>
        /// <param name="chatId"> Id of the chat window being polled. </param>
        void HandleKeepAlivePoll(string chatId);

        /// <summary>
        /// Will be invoked by a time triggered function at the specified interval.
        /// Incriments unpolled counts of queued chats and removes all chat windows from the ChatSessionQueue, that have exceeded the allowed unpolled count as configured.
        /// </summary>
        void ManageQueuedChats();

        /// <summary>
        /// Get the next active chat that needs to be assigned to an agent.
        /// </summary>
        /// <returns> returns a ChatWindow. </returns>
        ChatWindow GetNextChatForAgent();

        /// <summary>
        /// Remove a chat window from the ChatSessionQueue.
        /// </summary>
        /// <param name="chat"> the ChatWindow object to be removed. </param>
        void RemoveChatFromQueue(ChatWindow chat);
    }
}
