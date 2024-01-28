using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FunctionApp1.Services
{
    internal class PvoService
    {
        public async Task<int> CalculateTask(Stream fileStream)
        {
            try
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var fileContent = await streamReader.ReadToEndAsync();

                    // Split the content into lines and calculate the average
                    var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 0)
                    {
                        return 0; // or throw an exception indicating that the file is empty
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

                    return average;
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error in calculation: " + ex.Message, ex);
            }
        }

    }
}