using System.Collections.Generic;
using System.Threading.Tasks;
using SupportAgentManager.Domain.Dto;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define services related to ongoing chats
    /// </summary>
    public interface IChatSessionService
    {
        /// <summary>
        /// Gets the ongoing chats.
        /// </summary>
        List<ChatWindow> ActiveChats { get; }

        /// <summary>
        /// Gets the closed/completed chats.
        /// </summary>
        List<ChatWindow> ClosedChats { get; }

        /// <summary>
        /// Adds the closed chat to ClosedChats, sets the assigned agents count and reassigns to the queue if needed.
        /// </summary>
        /// <param name="chatId"> the id of the chat that needs to be closed. </param>
        /// <returns> returns a completed task. </returns>
        Task CloseChat(string chatId);

        /// <summary>
        /// Adds the closed chat to ClosedChats, sets the assigned agents count and reassigns to the queue if needed.
        /// </summary>
        /// <param name="chat"> the chat to be closed. </param>
        /// <returns> returns a completed task. </returns>
        Task CloseChat(ChatWindow chat);
    }
}
