using System.Net;

namespace Shared;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public HttpStatusCode? StatusCode { get; set; } = HttpStatusCode.OK;
}