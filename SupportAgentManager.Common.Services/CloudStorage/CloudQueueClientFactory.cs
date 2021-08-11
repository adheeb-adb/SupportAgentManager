using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using SupportAgentManager.Common.Domain.Contracts.CloudStorage;
using SupportAgentManager.Common.Domain.Dto.CloudStorage;
using System;

namespace SupportAgentManager.Common.Services.CloudStorage
{
    public class CloudQueueClientFactory : ICloudQueueClientFactory
    {
        private readonly CloudStorageInformation _cloudStorageInformation;
        private CloudQueueClient _cloudQueueClient;

        public CloudQueueClientFactory(CloudStorageInformation cloudStorageInformation)
        {
            _cloudStorageInformation = cloudStorageInformation;
        }

        public CloudQueueClient GetClient()
        {
            if (_cloudQueueClient != null)
            {
                return _cloudQueueClient;
            }

            var storageAccount = CloudStorageAccount.Parse(_cloudStorageInformation.CloudStorageConnectionString);
            _cloudQueueClient = storageAccount.CreateCloudQueueClient();
            return _cloudQueueClient;
        }

        public CloudQueue GetCloudQueueReference(string queueName)
        {
            return GetClient().GetQueueReference(queueName);
        }
    }
}
