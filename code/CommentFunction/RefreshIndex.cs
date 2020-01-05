using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SearchFunction
{
    public static class RefreshIndex
    {
        [FunctionName("submit-comment")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequest request,
            ILogger log)
        {
            try
            {
                var searchAccount = Environment.GetEnvironmentVariable("search-account");

                log.LogInformation($"Comment at: {DateTime.Now}");

                return new OkObjectResult("Hi buddy");
            }
            catch(Exception ex)
            {
                log.LogError($"Exception:  {ex.Message}");
                log.LogError($"Inner Exception:  {ex.InnerException?.Message}");
                log.LogError($"Exception type:  {ex.GetType().FullName}");

                throw;
            }
        }
    }
}