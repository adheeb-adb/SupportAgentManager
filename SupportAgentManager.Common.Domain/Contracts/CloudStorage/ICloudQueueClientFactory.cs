using Microsoft.WindowsAzure.Storage.Queue;

namespace SupportAgentManager.Common.Domain.Contracts.CloudStorage
{
    public interface ICloudQueueClientFactory
    {
        CloudQueueClient GetClient();

        CloudQueue GetCloudQueueReference(string queueName);
    }
}
