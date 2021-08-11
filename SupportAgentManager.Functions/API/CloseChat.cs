using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupportAgentManager.Domain.Contracts;

namespace SupportAgentManager.Functions.API
{
    public class CloseChat
    {
        private readonly IChatSessionService _chatSessionService;

        public CloseChat(IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }

        [FunctionName("CloseChat")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string chatId = req.Query["chatId"];
                await _chatSessionService.CloseChat(chatId.Trim());

                return new OkObjectResult("chat closed successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"ERROR: {ex.Message}");
                throw;
            }
        }
    }
}
