namespace SupportAgentManager.Common.Domain.Dto.CloudStorage
{
    /// <summary>
    /// Class to store cloud storage settings.
    /// </summary>
    public class CloudStorageInformation
    {
        /// <summary>
        /// Gets or sets the connection string of the cloud storage resource.
        /// </summary>
        public string CloudStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the cloud queue names.
        /// </summary>
        public CloudQueInformation CloudQueueNames { get; set; }
    }
}
