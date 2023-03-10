using System.Text.Json;

namespace WorkForYou.Core.AdditionalModels;

public class ApiError
{
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string? ErrorDetails { get; set; }

    public ApiError()
    {
    }

    public ApiError(int errorCode, string errorMessage, string? errorDetails = null)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        ErrorDetails = errorDetails;
    }

    public override string ToString()
    {
        JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(this, jsonOptions);
    }
}