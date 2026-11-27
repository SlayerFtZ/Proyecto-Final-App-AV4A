using System.Net;
using System.Text.Json.Serialization;

namespace AppFinalAv4A.Core.Network;

public sealed class ApiException : Exception
{
    public ApiException(HttpStatusCode? statusCode, string message, string? code = null, Exception? inner = null)
        : base(message, inner)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode? StatusCode { get; }

    public string? Code { get; }

    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;
}

public sealed class ApiErrorResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("codigo")]
    public string? Code { get; set; }

    [JsonPropertyName("mensaje")]
    public string? Message { get; set; }
}
