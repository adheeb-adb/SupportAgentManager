using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportAgentManager.Common.Domain.Contracts.CloudStorage
{
    public interface IAzureQueueService
    {
        T Read<T>(string message);

        Task SendAsync<T>(T obj, string queueName);
    }
}
