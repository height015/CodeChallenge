using System;
using System.Net;


namespace CodeChallenge.Helpers;


public static class ServiceResponseHelper
{
    public static bool IsReponseValid(this HttpResponseMessage response, out string msg)
    {
        try
        {
            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotFound &&
                response.StatusCode != HttpStatusCode.InternalServerError)
            {
                msg = "Unknown Request Status! This may be due to Service Error";
                return false;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                using var respContent = response.Content;
                string json = respContent.ReadAsStringAsync().Result;

                msg = json ?? "Invalid Request Source or Unauthorized User";
                return false;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                msg = "System Error! Invalid Request Point";
                return false;
            }

            if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                msg = "System Error! Invalid Request End";
                return false;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                msg = "Remote Server Error! Please try again later";
                return false;
            }
            msg = "";
            return true;
        }
        catch (Exception ex)
        {
            msg = "Service Response Error! Please try again later";
            return false;
        }
    }
}
