using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SupportAgentManager.Domain.Contracts;

namespace SupportAgentManager.Functions.Schedule
{
    public class InitializeShift
    {
        private readonly ISupportShiftService _supportShiftService;
        private readonly IAgentService _agentService;
        private readonly IAgentQueueService _agentQueueService;
        private readonly IChatSessionQueueService _chatSessionQueueService;

        public InitializeShift(ISupportShiftService supportShiftService,
            IAgentService agentService,
            IAgentQueueService agentQueueService,
            IChatSessionQueueService chatSessionQueueService)
        {
            _supportShiftService = supportShiftService;
            _agentService = agentService;
            _agentQueueService = agentQueueService;
            _chatSessionQueueService = chatSessionQueueService;
        }

        [FunctionName("InitializeShift")]
        public void Run([TimerTrigger("%ShiftInitializeCron%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function InitializeShift executed at: {DateTime.Now}");

            try
            {
                _supportShiftService.BeginNewShift();
                _agentService.InitializeService();
                _agentQueueService.InitializeService();
                _chatSessionQueueService.InitializeService();
            }
            catch (Exception ex)
            {
                log.LogError($"ERROR executing InitializeShift: { ex.Message }");
                throw;
            }
        }
    }
}
