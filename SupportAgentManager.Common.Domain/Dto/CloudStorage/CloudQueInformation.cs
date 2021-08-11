namespace SupportAgentManager.Common.Domain.Dto.CloudStorage
{
    /// <summary>
    /// class to store cloud queue names.
    /// </summary>
    public class CloudQueInformation
    {
        /// <summary>
        /// Gets or sets the queue name of Agent events.
        /// </summary>
        public string AgentEventQueue { get; set; }

        /// <summary>
        /// Gets or sets the queue name for Chat events.
        /// </summary>
        public string ChatEventQueue { get; set; }
    }
}
