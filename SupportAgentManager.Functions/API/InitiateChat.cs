using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupportAgentManager.Domain.Dto.ApiRequest;
using SupportAgentManager.Domain.Dto.ApiResponse;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.ConfigInformation;

namespace SupportAgentManager.Functions.API
{
    public class InitiateChat
    {
        private readonly IChatSessionQueueService _chatSessionQueueService;

        public InitiateChat(IChatSessionQueueService chatSessionQueueService)
        {
            _chatSessionQueueService = chatSessionQueueService;
        }

        [FunctionName("InitiateChat")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function InitiateChat processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                ChatRequest chatRequest = JsonConvert.DeserializeObject<ChatRequest>(requestBody);

                var result =_chatSessionQueueService.HandleChatRequest(chatRequest);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"ERROR: {ex.Message}");
            }
        }
    }
}
