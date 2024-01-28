using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public static class HttpTriggerFunction
{
    [FunctionName("UploadFileHttp")]
    
    public static async Task<IActionResult> UploadFileHttp(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
    [Blob("filecontainer/{rand-guid}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream blobStream,
    ILogger log)
    {
        try
        {
            using (var reader = new StreamReader(req.Body))
            {
                string fileContent = await reader.ReadToEndAsync();
                using (var writer = new StreamWriter(blobStream))
                {
                    await writer.WriteAsync(fileContent);
                }

                return new OkObjectResult("File uploaded successfully");
            }
        }
        catch (Exception ex)
        {
            log.LogError($"Error uploading file: {ex.Message}");
            return new StatusCodeResult(500);
        }
    }

}
