using System.Text;
using System.Text.Json;
using VehicleManager.Core.Models;

namespace VehicleManager.Core.Services
{
    public class VehicleApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public VehicleApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://localhost:8001/api/vehicles";
            
            // Set default headers
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Vehicle>>>(json, GetJsonOptions());
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                throw new Exception(apiResponse?.Message ?? "Failed to retrieve vehicles");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse response: {ex.Message}");
            }
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vehicle>>(json, GetJsonOptions());
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
        }

        public async Task<List<Vehicle>> SearchVehiclesAsync(string searchText)
        {
            try
            {
                var url = $"{_baseUrl}?search={Uri.EscapeDataString(searchText)}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Vehicle>>>(json, GetJsonOptions());
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                throw new Exception(apiResponse?.Message ?? "Failed to search vehicles");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicle, GetJsonOptions());
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vehicle>>(responseJson, GetJsonOptions());
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                throw new Exception(apiResponse?.Message ?? "Failed to create vehicle");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicle, GetJsonOptions());
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{_baseUrl}/{vehicle.Id}", content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Vehicle>>(responseJson, GetJsonOptions());
                
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data;
                }
                
                throw new Exception(apiResponse?.Message ?? "Failed to update vehicle");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{vehicleId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, GetJsonOptions());
                
                if (apiResponse?.Success != true)
                {
                    throw new Exception(apiResponse?.Message ?? "Failed to delete vehicle");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
        }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}