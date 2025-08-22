using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace ThreeCDeveloperTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(" 3C Developer Test - API Client");
            Console.WriteLine("==================================");
            
            var apiClient = new ThreeCApiClient();
            
            try
            {
              

           
                var objectResponse = await apiClient.GetExamDataAsObjectAsync();
                
                if (objectResponse.Success && objectResponse.Data != null)
                {
                
                    Console.WriteLine($"Company: {objectResponse.Data.Company}");
                    Console.WriteLine($"Department: {objectResponse.Data.Department}");
                    Console.WriteLine($"Position: {objectResponse.Data.Position}");
                    
                    // Pretty print JSON
                    string prettyJson = JsonSerializer.Serialize(objectResponse.Data, new JsonSerializerOptions 
                    { 
                        WriteIndented = true 
                    });
                    Console.WriteLine($"Formatted JSON:\n{prettyJson}");
                }
                else
                {
                    Console.WriteLine($"Failed: {objectResponse.Message}");
                }

              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Application error: {ex.Message}");
            }
            finally
            {
                apiClient.Dispose();
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}