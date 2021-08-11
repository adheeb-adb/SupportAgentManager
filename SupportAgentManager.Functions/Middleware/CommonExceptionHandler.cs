using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportAgentManager.Functions.Middleware
{
    /*public class CommonExceptionHandler : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                // Code before function execution here
                await next(context);
                // Code after function execution here
            }
            catch (Exception ex)
            {
                var log = context.GetLogger<CommonExceptionHandler>();
                log.LogWarning(ex, string.Empty);
            }
        }
    }*/
}
