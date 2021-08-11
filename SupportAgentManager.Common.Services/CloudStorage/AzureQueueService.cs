using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SupportAgentManager.Common.Domain.Contracts.CloudStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportAgentManager.Common.Services.CloudStorage
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly ICloudQueueClientFactory _cloudQueueClientFactory;

        public AzureQueueService(ICloudQueueClientFactory cloudQueueClientFactory)
        {
            _cloudQueueClientFactory = cloudQueueClientFactory;
        }

        public T Read<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

        public async Task SendAsync<T>(T obj, string queueName)
        {
            var queueReference = _cloudQueueClientFactory.GetCloudQueueReference(queueName);
            await queueReference.CreateIfNotExistsAsync();

            var serializedMessage = JsonConvert.SerializeObject(obj);
            var queueMessage = new CloudQueueMessage(serializedMessage);

            await queueReference.AddMessageAsync(queueMessage);
        }
    }
}
