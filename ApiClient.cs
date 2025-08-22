using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ThreeCDeveloperTest
{
    /// <summary>
    /// API Client for fetching data from 3C Solomon API
    /// </summary>
    public class ThreeCApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://3csolomon.com/api/it/exam";

        public ThreeCApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "3C-Developer-Test-Client/1.0");
            // Set timeout for requests
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Fetches data from the 3C Solomon API endpoint
        /// </summary>
        /// <returns>API response containing company information</returns>
        public async Task<ApiResponse<string>> GetExamDataAsync()
        {
            try
            {
                Console.WriteLine($"Making GET request to: {BaseUrl}");
                
                HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("✅ Request successful!");
                    
                    return new ApiResponse<string>
                    {
                        Success = true,
                        Data = content,
                        StatusCode = (int)response.StatusCode,
                        Message = "Data retrieved successfully"
                    };
                }
                else
                {
                    string errorMessage = $"Request failed with status code: {response.StatusCode}";
                    Console.WriteLine($"❌ {errorMessage}");
                    
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Data = null,
                        StatusCode = (int)response.StatusCode,
                        Message = errorMessage
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"HTTP Request error: {ex.Message}";
                Console.WriteLine($"❌ {errorMessage}");
                return CreateErrorResponse<string>(errorMessage);
            }
            catch (TaskCanceledException ex)
            {
                string errorMessage = $"Request timeout: {ex.Message}";
                Console.WriteLine($"❌ {errorMessage}");
                return CreateErrorResponse<string>(errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Unexpected error: {ex.Message}";
                Console.WriteLine($"❌ {errorMessage}");
                return CreateErrorResponse<string>(errorMessage);
            }
        }


      
        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                StatusCode = 0,
                Message = message
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    /// <summary>
    /// Represents the company information returned by the API
    /// </summary>
    public class CompanyInfo
    {
        public string Company { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Company: {Company}, Department: {Department}, Position: {Position}";
        }
    }

    /// <summary>
    /// Generic API response wrapper
    /// </summary>
    /// <typeparam name="T">Type of data returned by the API</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
