using System;
using RestSharp;

namespace IFS.ApiTests.Helpers
{
    public static class TestLogger
    {
        public static void LogRequest(RestRequest request)
        {
            Console.WriteLine("======== REQUEST ========");
            Console.WriteLine($"Timestamp : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Method    : {request.Method}");
            Console.WriteLine($"Endpoint  : {request.Resource}");

            foreach (var param in request.Parameters)
            {
                Console.WriteLine($"Param     : {param.Name} = {param.Value}");
            }

            Console.WriteLine("=========================");
        }

        public static void LogResponse(RestResponse response)
        {
            Console.WriteLine("======== RESPONSE =======");
            Console.WriteLine($"Status    : {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine($"Time      : {response.ResponseUri}");
            Console.WriteLine($"Content   : {response.Content}");
            Console.WriteLine("=========================\n");
        }
    }
}