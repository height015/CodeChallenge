using System;
using System.Net;

namespace CodeChallenge.Helper;

public class ApiStatusResponse
{

    public ApiStatusResponse(HttpStatusCode StatusCode, string message = null)
    {
        Status = StatusCode;
        Message = message ?? GetDefaultStatusMessage(StatusCode);
    }

    public HttpStatusCode Status { get; set; }
    public string Message { get; set; }

    private string GetDefaultStatusMessage(HttpStatusCode Status)
    {

        return Status switch
        {
            HttpStatusCode.Unauthorized => "Invalid Request Source or Unauthorize User",
            HttpStatusCode.NotFound => "System Error! Invalid Request Point",
            HttpStatusCode.MethodNotAllowed => "System Error! Invalid Request End",
            HttpStatusCode.InternalServerError => "Remote Server Error! Please Try Again Late",
            HttpStatusCode.BadRequest => "Bad Request",
            HttpStatusCode.OK => "Success",
            _ => string.Empty

        };

    }
}


