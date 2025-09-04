using System.Net;

namespace ECP.UI.Server.Components
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }

        public override string? ToString()
        {
            return $"""
                Status-Code: {StatusCode.ToString()}
                Success: {Success}
                Message: {Message}
                """;
        }
    }
}
