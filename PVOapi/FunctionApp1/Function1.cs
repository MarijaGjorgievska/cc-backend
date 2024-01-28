using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FunctionApp1.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public static class BlobTriggerFunction
{
    [FunctionName("ProcessUploadedFileBlob")]
    public static async Task ProcessUploadedFileBlob(
    [BlobTrigger("filecontainer/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream,
    string name,
    ILogger log)
    {
        var blobService = new PvoService();
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await blobStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset the position for reading

                // Call the CalculateTask method from the PvoService
                int average = await blobService.CalculateTask(memoryStream);

                log.LogInformation($"Blob processed. Average value: {average}");
            }
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing blob: {ex.Message}");
        }
    }


    private static async Task<MemoryStream> ConvertStreamReaderToMemoryStream(StreamReader reader)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                await writer.WriteAsync(await reader.ReadToEndAsync());
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }

}
