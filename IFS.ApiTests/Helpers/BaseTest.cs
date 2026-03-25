using IFS.ApiTests.Clients;
using NUnit.Framework;

namespace IFS.ApiTests.Helpers
{
    public abstract class BaseTest
    {
        protected ApiClient ApiClient;

        [SetUp]
        public void Setup()
        {
            ApiClient = new ApiClient();
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