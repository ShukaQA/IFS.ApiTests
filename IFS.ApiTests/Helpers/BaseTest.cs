using Allure.NUnit;
using IFS.ApiTests.Clients;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace IFS.ApiTests.Helpers
{
    [AllureNUnit]
    public abstract class BaseTest
    {
        protected ApiClient ApiClient = null!;
        protected int MaxResponseTimeMs;

        [SetUp]
        public void Setup()
        {
            ApiClient = new ApiClient();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            MaxResponseTimeMs = int.Parse(
                config["TestSettings:MaxResponseTimeMs"] ?? "3000");

            Console.WriteLine($"\n>>> Starting test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            var result = TestContext.CurrentContext.Result.Outcome.Status;
            Console.WriteLine($">>> Test result: {result}\n");
        }
    }
}