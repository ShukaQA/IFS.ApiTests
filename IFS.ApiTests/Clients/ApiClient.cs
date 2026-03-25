using RestSharp;
using IFS.ApiTests.Helpers;
using Microsoft.Extensions.Configuration;

namespace IFS.ApiTests.Clients
{
    public class ApiClient
    {
        private readonly RestClient _client;
        private const int DefaultRetries = 3;
        private const int RetryDelayMs = 1000;

        public ApiClient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = config["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("BaseUrl is not configured.");

            var timeoutSeconds = int.Parse(config["ApiSettings:TimeoutSeconds"] ?? "30");

            var options = new RestClientOptions(baseUrl)
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };

            _client = new RestClient(options);
        }

        public RestResponse<T> Get<T>(string endpoint) where T : notnull
        {
            var request = new RestRequest(endpoint, Method.Get);
            return ExecuteWithRetry<T>(request);
        }

        public RestResponse<T> Post<T>(string endpoint, object body) where T : notnull
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            return ExecuteWithRetry<T>(request);
        }

        public RestResponse<T> Put<T>(string endpoint, object body) where T : notnull
        {
            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(body);
            return ExecuteWithRetry<T>(request);
        }

        public RestResponse Delete(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            TestLogger.LogRequest(request);
            var response = _client.Execute(request);
            TestLogger.LogResponse(response);
            return response;
        }

        private RestResponse<T> ExecuteWithRetry<T>(RestRequest request, int retries = DefaultRetries) where T : notnull
        {
            for (int attempt = 1; attempt <= retries; attempt++)
            {
                TestLogger.LogRequest(request);
                var response = _client.Execute<T>(request);
                TestLogger.LogResponse(response);

                if (response.IsSuccessful || (int)response.StatusCode == 404)
                    return response;

                Console.WriteLine($"Attempt {attempt} failed. Retrying in {RetryDelayMs}ms...");
                Thread.Sleep(RetryDelayMs);
            }

            TestLogger.LogRequest(request);
            var finalResponse = _client.Execute<T>(request);
            TestLogger.LogResponse(finalResponse);
            return finalResponse;
        }
    }
}