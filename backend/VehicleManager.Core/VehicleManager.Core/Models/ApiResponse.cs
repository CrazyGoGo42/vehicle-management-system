using System.Text.Json.Serialization;

namespace VehicleManager.Core.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("details")]
        public object? Details { get; set; }
    }

    public class ApiResponse : ApiResponse<object>
    {
    }
}