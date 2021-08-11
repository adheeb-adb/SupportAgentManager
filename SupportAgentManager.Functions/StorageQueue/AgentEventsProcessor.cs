using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupportAgentManager.Domain.Dto.Base;

namespace SupportAgentManager.Functions.StorageQueue
{
    /// <summary>
    /// The function can be used to handle and susequent actions related to an event of an agent
    /// </summary>
    public class AgentEventsProcessor
    {
        [FunctionName("AgentEventsProcessor")]
        public async Task Run([QueueTrigger("%CloudStorageInformation:CloudQueueNames:AgentEventQueue%", Connection = "CloudStorageInformation:CloudStorageConnectionString")]string agentEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {agentEvent}");
            var parsedMessage = JsonConvert.DeserializeObject<SupportAgentDto>(agentEvent);
        }
    }
}
