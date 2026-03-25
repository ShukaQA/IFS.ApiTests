using RestSharp;
using IFS.ApiTests.Config;
using Microsoft.Extensions.Configuration;

namespace IFS.ApiTests.Clients
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<AppSettings>();

            var options = new RestClientOptions(config.ApiSettings.BaseUrl)
            {
                Timeout = TimeSpan.FromSeconds(config.ApiSettings.TimeoutSeconds) 
            };

            _client = new RestClient(options);
        }

        public RestResponse<T> Get<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            return _client.Execute<T>(request);
        }

        public RestResponse<T> Post<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            return _client.Execute<T>(request);
        }

        public RestResponse<T> Put<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(body);
            return _client.Execute<T>(request);
        }

        public RestResponse Delete(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            return _client.Execute(request);
        }
    }
}