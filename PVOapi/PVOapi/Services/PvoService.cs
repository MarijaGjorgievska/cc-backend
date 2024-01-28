using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PVOapi.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace PVOapi.Services
{
    public class PvoService : IPvoService
    {
        private readonly IWebHostEnvironment _environment;
      

        public PvoService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<PvoModel> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Invalid file");
                }

                // Check if the file format is not txt
                if (Path.GetExtension(file.FileName).ToLower() != ".txt")
                {
                    throw new ArgumentException("Wrong input format. Only .txt files are allowed.");
                }

                string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);


                var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);

                // Measure the processing time for the CalculateTask method
                //var stopwatch = new Stopwatch();
                // stopwatch.Start();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Stop measuring the processing time
                // stopwatch.Stop();

                // Call the CalculateTask function on the uploaded file
                int average = await CalculateTask(file);

                

                var result = new PvoModel
                {
                    StatusCode = PvoModel.OK,
                    StatusMessage = "OK",
                    AverageValue = average,
                    //ProcessingTime = stopwatch.Elapsed.TotalMilliseconds,
                    
                };

                return result;
            }
            catch (ArgumentException ex)
            {
                return new PvoModel
                {
                    StatusCode = PvoModel.WrongInputFormat,
                    StatusMessage = ex.Message,
                };
            }
            catch (Exception ex)
            {
                return new PvoModel
                {
                    StatusCode = PvoModel.ErrorInCalculation,
                    StatusMessage = "Error in calculation: " + ex.Message,
                };
            }
        }

       

        private async Task<int> CalculateTask(IFormFile file)
        {
            try
            {
                // Measure the processing time
                // var stopwatch = new Stopwatch();
                // stopwatch.Start();

                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = await streamReader.ReadToEndAsync();

                    // Split the content into lines and calculate the average
                    var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 0)
                    {
                        return 0; 
                    }

                    int sum = 0;
                    foreach (var line in lines)
                    {
                        if (int.TryParse(line, out int value))
                        {
                            sum += value;
                        }
                    }

                    int average = sum / lines.Length;

                    // Stop measuring the processing time
                    // stopwatch.Stop();

                    // Store the calculated average in the model
                    var result = new PvoModel
                    {
                        AverageValue = average,
                        // ProcessingTime = stopwatch.Elapsed.TotalMilliseconds,
                        StatusCode = PvoModel.OK,
                        StatusMessage = "OK",
                        
                    };

                    return average;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in calculation: " + ex.Message, ex);
            }
        }

        // New method to get file path from file name
        private string GetFilePathFromFileName(string fileName)
        {
            // Specify the directory where files are stored
            var filesDirectory = Path.Combine(_environment.ContentRootPath, "Files");

            // Combine the directory and file name to get the full file path
            var filePath = Path.Combine(filesDirectory, fileName);

            // Ensure the file path is within the specified directory to avoid security issues
            if (!filePath.StartsWith(filesDirectory))
            {
                // Invalid or malicious file name
                return null;
            }

            return filePath;
        }
    }
}